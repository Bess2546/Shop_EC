using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Backend.UploadService
{
    public class UploadService : IUploadService
    {
        private readonly HttpClient _httpClient;
        private readonly string _supabaseUrl;
        private readonly string _supabaseKey;
        private readonly string _bucket = "image";

        public UploadService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _supabaseUrl = configuration[ "Supabase:Url"]!;
            _supabaseKey = configuration["Supabase:Key"]!;
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folder)
        {
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            if (!allowedTypes.Contains(file.ContentType))
                throw new Exception("อนุญาตเฉพาะไฟล์ .jpg .png .webp");

            if (file.Length > 5 * 1024 * 1024)
                throw new Exception("ไฟล์ต้องไม่เกิน 5MB");

            var fileName = $"{folder}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            using var stream = file.OpenReadStream();
            using var content = new StreamContent(stream);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

            var request = new HttpRequestMessage(HttpMethod.Post,
                $"{_supabaseUrl}/storage/v1/object/{_bucket}/{fileName}");
            request.Headers.Add("Authorization", $"Bearer {_supabaseKey}");
            request.Headers.Add("apikey", _supabaseKey);
            request.Content = content;

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception("อัปโหลดรูปไม่สำเร็จ");

            return $"{_supabaseUrl}/storage/v1/object/public/{_bucket}/{fileName}";
        }
    }
}