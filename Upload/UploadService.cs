using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using Shop_Backend.Upload;

namespace Shop_Backend.UploadService
{
    public class UploadService : IUploadService
    {
        private readonly HttpClient _httpClient;
        private readonly SupabaseSettings _settings;

        private static readonly Dictionary<string, string> AllowedTypes = new()
        {
            ["image/jpeg"] = ".jpg",
            ["image/png"] = ".png",
            ["image/webp"] = ".webp"
        };

        private static readonly HashSet<string> AllowedFolders = new(StringComparer.OrdinalIgnoreCase)
        {
            "products", "profiles", "categories", "banner"
        };

        private const long MaxFileSize = 5 * 1024 * 1024;

        public UploadService(
            IHttpClientFactory httpClientFactory,
            IOptions<SupabaseSettings> options)
        {
            _httpClient = httpClientFactory.CreateClient(nameof(UploadService));
            _settings = options.Value;
        }
        public async Task<string> UploadImageAsync(IFormFile file, string folder)
        {
            if (file is null || file.Length == 0)
                throw new UploadException("ไม่พบไฟล์ที่อัปโหลด");

            if (file.Length > MaxFileSize)
                throw new UploadException("ไฟล์ต้องไม่เกิน 5MB");

            if (!AllowedTypes.TryGetValue(file.ContentType, out var safeExtension))
                throw new UploadException("อนุญาตเฉพาะไฟล์ .jpg .png .webp");

            if (!await IsValidImageAsync(file))
                throw new UploadException("ไฟล์ไม่ใช่รูปภาพที่ถูกต้อง");

            if (string.IsNullOrWhiteSpace(folder) || !AllowedFolders.Contains(folder))
                throw new UploadException($"Folder '{folder}' ไม่ได้รับอนุญาต");


            var safeFileName = $"{Guid.NewGuid():N}{safeExtension}";
            var objectPath = $"{folder.ToLowerInvariant()}/{safeFileName}";

            await UploadToSupabaseAsync(file, objectPath);

            return $"{_settings.Url}/storage/v1/object/public/{_settings.Bucket}/{objectPath}";
        }

        private async Task UploadToSupabaseAsync(IFormFile file, string objectPath)
        {
            await using var stream = file.OpenReadStream();
            using var content = new StreamContent(stream);
            content.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_settings.Url}/storage/v1/object/{_settings.Bucket}/{objectPath}");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _settings.Key);
            request.Headers.Add("apikey", _settings.Key);
            request.Content = content;

            using var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new UploadException(
                    $"อัปโหลดรูปไม่สำเร็จ: {response.StatusCode}");
            }
        }

        private static async Task<bool> IsValidImageAsync(IFormFile file)
        {
            await using var stream = file.OpenReadStream();
            var header = new byte[12];
            var read = await stream.ReadAsync(header.AsMemory(0, 12));
            stream.Position = 0;

            if (read < 4) return false;

            if (header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF)
            {
                return true;
            }


            if (header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47)
            {
                return true;
            }


            if (read >= 12 &&
                header[0] == 0x52 && header[1] == 0x49 && header[2] == 0x46 && header[3] == 0x46 &&
                header[8] == 0x57 && header[9] == 0x45 && header[10] == 0x42 && header[11] == 0x50)
            {
                return true;
            }

            return false;
        }
    }
}