using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EventEaseBookingSystem.Services
{
    public class AzureBlobStorageService
    {
        private readonly string _connectionString;

        public AzureBlobStorageService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AzureBlobStorage");
        }
        // ✅ Modified method with a second argument
        public async Task<string> UploadFileAsync(IFormFile file, string containerName)
        {
            if (file == null || file.Length == 0)
                return null;

            var blobClient = new BlobContainerClient(_connectionString, containerName);
            await blobClient.CreateIfNotExistsAsync();

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var blob = blobClient.GetBlobClient(fileName);

            using (var stream = file.OpenReadStream())
            {
                await blob.UploadAsync(stream, overwrite: true);
            }

            return blob.Uri.ToString();
        }

        // ✅ Optional: delete file method for cleanup
        public async Task DeleteFileAsync(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
                return;

            Uri uri = new Uri(fileUrl);
            string containerName = uri.Segments[1].Trim('/');
            string blobName = string.Join("", uri.Segments.Skip(2));

            var blobClient = new BlobContainerClient(_connectionString, containerName);
            var blob = blobClient.GetBlobClient(blobName);

            await blob.DeleteIfExistsAsync();
        }

        public async Task<List<string>> ListBlobsAsync(string containerName)
        {
            var blobClient = new BlobContainerClient(_connectionString, containerName);
            var blobs = new List<string>();

            await foreach (var blobItem in blobClient.GetBlobsAsync())
            {
                blobs.Add(blobClient.Uri + "/" + blobItem.Name);
            }

            return blobs;
        }

    }
}
