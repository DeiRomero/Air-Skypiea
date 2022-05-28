using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Air_Skypiea.Helpers
{
    public class BlobHelper : IBlobHelper
    {
        private readonly CloudBlobClient _blobClient;
        public BlobHelper(IConfiguration configuration)
        {
            string keys = configuration["Blob:ConnectionString"];
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(keys);
            _blobClient = storageAccount.CreateCloudBlobClient();

        }
        public Task DeleteBlobAsync(Guid id, string containerName)
        {
            throw new NotImplementedException();
        }
e 4rre
        public async Task<Guid> UploadBlobAsync(IFormFile file, string containerName)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> UploadBlobAsync(byte[] file, string containerName)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> UploadBlobAsync(string image, string containerName)
        {
            throw new NotImplementedException();
        }
    }
}
