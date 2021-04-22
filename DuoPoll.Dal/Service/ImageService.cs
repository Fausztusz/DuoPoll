using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Azure;


namespace DuoPoll.Dal.Service
{
    public class ImageService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public ImageService(IAzureClientFactory<BlobServiceClient> blob)
        {
            _blobServiceClient = blob.CreateClient("BlobService");
        }


        public string UploadBlob(IFormFile file)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("image-container");
            var fileName = Guid.NewGuid().ToString() + file.FileName;

            // Get a reference to a blob
            var blobClient = containerClient.GetBlobClient(fileName);

            blobClient.Upload(file.OpenReadStream(), true);
            return containerClient.Uri.AbsoluteUri+"/"+ fileName;
        }
    }
}