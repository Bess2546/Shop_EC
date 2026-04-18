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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadProductImage([FromForm] IFormFile file)
        {
            var url = await _uploadService.UploadImageAsync(file, "products");
            return Ok(new { imageUrl = url });
        }

        [HttpPost("profile")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadProfileImage([FromForm] IFormFile file)
        {
            var url = await _uploadService.UploadImageAsync(file, "profiles");
            return Ok(new { imageUrl = url });
        }
    }
}