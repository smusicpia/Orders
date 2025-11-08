namespace Orders.Backend.Helpers;

public interface IFileStorage
{
    Task<string> SaveFileAsync(byte[] content, string extention);

    Task RemoveFileAsync(string path);

    async Task<string> EditFileAsync(byte[] content, string extention, string path)
    {
        if (path is not null)
        {
            await RemoveFileAsync(path);
        }

        return await SaveFileAsync(content, extention);
    }
}