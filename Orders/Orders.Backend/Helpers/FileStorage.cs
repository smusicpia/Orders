using System.Reflection.Metadata;

using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

using Microsoft.AspNetCore.Http.HttpResults;

namespace Orders.Backend.Helpers;

public class FileStorage : IFileStorage
{
    private readonly GoogleCredential _googleCredential;
    private readonly StorageClient _storageClient;
    private readonly string _bucketName;

    public FileStorage(IConfiguration configuration)
    {
        _googleCredential = GoogleCredential.FromFile(configuration.GetValue<string>("GoogleCredentialFile"));
        _storageClient = StorageClient.Create(_googleCredential);
        _bucketName = configuration.GetValue<string>("GoogleCloudStorageBucket");
    }


    public async Task RemoveFileAsync(string path)
    {
        ////Azure Blob Storage
        //var client = new BlobContainerClient(_connectionString, containerName);
        //await client.CreateIfNotExistsAsync();
        //var fileName = Path.GetFileName(path);
        //var blob = client.GetBlobClient(fileName);
        //await blob.DeleteIfExistsAsync();

        // Google Blob Storage
        var client = StorageClient.Create();
        var fileName = Path.GetFileName(new Uri(path).LocalPath);
        await client.DeleteObjectAsync(_bucketName, fileName);
    }

    public async Task<string> SaveFileAsync(byte[] content, string extention)
    {
        ////Azure Blob Storage
        //var client = new BlobContainerClient(_connectionString, containerName);
        //await client.CreateIfNotExistsAsync();
        //client.SetAccessPolicy(PublicAccessType.Blob);
        //var fileName = $"{Guid.NewGuid()}{extention}";
        //var blob = client.GetBlobClient(fileName);

        //using (var ms = new MemoryStream(content))
        //{
        //    await blob.UploadAsync(ms);
        //}

        //return blob.Uri.ToString();

        // Google Blob Storage
        var obj = await _storageClient.UploadObjectAsync(
            bucket: _bucketName,
            objectName: $"{Guid.NewGuid()}{extention}",
            contentType: "application/octet-stream",
            source: new MemoryStream(content)
        );

        return obj.MediaLink.ToString();
    }
}