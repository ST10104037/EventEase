using Azure.Storage.Blobs;

namespace EventEase.Services
{
    public class BlobStorageService
    {
        private readonly string connectionString = "Server=tcp:eventease1.database.windows.net,1433;Initial Catalog=EventEaseDB;Persist Security Info=False;User ID=ST10104037;Password=EventEase123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
        private readonly string containerName = "images";

        public async Task<string> UploadAsync(IFormFile file)
        {
            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = containerClient.GetBlobClient(file.FileName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            return blobClient.Uri.ToString();
        }
    }
}