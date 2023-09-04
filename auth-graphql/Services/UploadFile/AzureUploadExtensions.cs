using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Path = System.IO.Path;

namespace auth_graphql.Services.UploadFile;

public class AzureUploadExtensions
{
    private const int MaxFileSizeMb = 5;
    private static string _azureConnect = "";
    private static string _containerName = "";
    private readonly IConfiguration _configuration;
    private readonly BlobContainerClient _blobContainerClient;
    public AzureUploadExtensions(IConfiguration configuration)
    {
        _configuration = configuration;
        _azureConnect = _configuration["AzureConnectionStrings"];
        _containerName = _configuration["AzureContainerName"];
        _blobContainerClient = new BlobContainerClient(_azureConnect, _containerName);
    }

    public async Task<string> UploadFile(string filePath, string userName)
    {
        var fileName = Path.GetFileName(filePath);
        var fileNameUpload = GenerateFileName(fileName, userName);
        FileInfo fileInfo = new FileInfo(filePath);
        if(fileInfo.Length > MaxFileSizeMb * 1024 * 1024)
        {
            throw new GraphQLException(new Error("File size exceeds the maximum allowed size", "FILE_SIZE_MAXIMUM"));
        }

        try
        {
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileNameUpload);
            using (var fileStream = File.OpenRead(filePath))
            {
                await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = "image/jpeg" });
            }
            var fileUri = blobClient.Uri.AbsoluteUri;
            return fileUri;
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    public async Task DeleteBlob(string fileUri)
    {
        try
        {
            var uri = new Uri(fileUri);
            var fileName = uri.Segments.Last();
            fileName = fileName.Replace("%40", "@");
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();
        }
        catch (Exception ex)
        {         
            throw new GraphQLException(new Error($"{ex}", $"{ex}"));
        }
    }

    private string GenerateFileName(string fileName, string userName)
    {
        try
        {
            string strFileName = string.Empty;
            string[] strName = fileName.Split('.');
            strFileName = userName + DateTimeOffset.UtcNow.ToUniversalTime().ToString("yyyyMMdd\\THHmmssfff") + "." + strName[strName.Length - 1];
            return strFileName;
        }
        catch (Exception ex)
        {
            
            return fileName;
        }
    }
}