using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Walks.API.Models.Domain;
using Walks.API.Models.DTO;
using Walks.API.Repositories;

namespace Walks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        public ImagesController(IImageRepository imageRepository) 
        {
            this._imageRepository = imageRepository;
        }
        /// <summary>
        /// POST A IMAGE
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> UploadImage([FromForm] ImageRequestDto model)
        {
            ValidateFileUpload(model);

            if(ModelState.IsValid)
            {
                // --- Convert Dto to Domain
                var imageD = new Image
                {
                    File = model.File,
                    FileExtension = Path.GetExtension(model.File.FileName),
                    FileSizeInBytes = model.File.Length,
                    FileName = model.FileName,
                    FileDescription = model.FileDescription
                };
                // --- User repository to upload image
                imageD = await _imageRepository.UploadImage(imageD);

                return Ok(imageD);

            }
            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageRequestDto imageRequest)
        {
            var allowedExtensions = new List<string>() { ".jpg", ".jpeg", ".png"};

            // RN01 | Check Extensions
            if (!allowedExtensions.Contains(Path.GetExtension(imageRequest.File.FileName)))
            {
                ModelState.AddModelError("file", "This file extension is unsupported");
            }
            // RN02 | Check Length
            if(imageRequest.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more than 10MB, please upload a smaller size file.");
            }
        }
    }
}
