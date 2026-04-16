using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop_Backend.UploadService;

namespace Shop_Backend.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UploadController : ControllerBase
    {
        private readonly IUploadService _uploadService;

        public UploadController(IUploadService uploadService)
        {
            _uploadService = uploadService;
        }

        [HttpPost("product")]
        public async Task<IActionResult> UploadProductImage(IFormFile file)
        {
            var url = await _uploadService.UploadImageAsync(file, "products");
            return Ok(new { imageUrl = url });
        }

        [HttpPost("profile")]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            var url = await _uploadService.UploadImageAsync(file, "profiles");
            return Ok(new { imageUrl = url });
        }
    }
}