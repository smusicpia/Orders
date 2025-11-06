using System.Reflection.Metadata;

using Google.Cloud.Storage.V1;

using Microsoft.AspNetCore.Http.HttpResults;

namespace Orders.Backend.Helpers;

public class FileStorage : IFileStorage
{
    private readonly string _connectionString;

    public FileStorage(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("GoogleStorage")!;
    }

    public async Task RemoveFileAsync(string path, string containerName)
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
        await client.DeleteObjectAsync(containerName, fileName);
    }

    public async Task<string> SaveFileAsync(byte[] content, string extention, string containerName)
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
        var client = StorageClient.Create();
        var obj = await client.UploadObjectAsync(
            bucket: containerName,
            objectName: $"{Guid.NewGuid()}{extention}",
            contentType: "application/octet-stream",
            source: new MemoryStream(content)
        );

        return obj.MediaLink.ToString();
    }
}