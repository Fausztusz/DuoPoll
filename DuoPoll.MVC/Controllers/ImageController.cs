using System.Web.Helpers;
using Azure.Storage.Blobs;
using DuoPoll.Dal.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;


namespace DuoPoll.MVC.Controllers
{
    public class ImageController : Controller
    {
        private readonly ImageService _imageService;
        public ImageController(IAzureClientFactory<BlobServiceClient> blob)
        {
            _imageService = new ImageService(blob);
        }

        public IActionResult Upload(IFormFile file)
        {
            var url=_imageService.UploadBlob(file);
            return Json(new {url = url});
        }
    }
}