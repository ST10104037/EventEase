using Microsoft.AspNetCore.Mvc;
using EventEase.Services;

namespace EventEase.Controllers
{
    public class UploadController : Controller
    {
        private readonly BlobStorageService _blobService;

        // ✅ Dependency Injection
        public UploadController(BlobStorageService blobService)
        {
            _blobService = blobService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file selected");

            var imageUrl = await _blobService.UploadAsync(file);

            return Ok(new { url = imageUrl });
        }
    }
}