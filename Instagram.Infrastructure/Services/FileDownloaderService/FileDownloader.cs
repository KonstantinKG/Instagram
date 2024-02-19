using System.Security.Cryptography;

using Instagram.Application.Common.Interfaces.Services;
using Instagram.Domain.Common.Exceptions;

using Microsoft.Extensions.Options;

namespace Instagram.Infrastructure.Services.FileDownloaderService;

public class FileDownloader : IFileDownloader
{
    private readonly FileDownloaderSettings _fileDownloaderSettings;

    public FileDownloader(IOptions<FileDownloaderSettings> fileDownloaderSettings)
    {
        _fileDownloaderSettings = fileDownloaderSettings.Value;
    }

    public async Task<string> Download(IAppFileProxy file)
    {
        try
        {
            if (!Directory.Exists(_fileDownloaderSettings.SavePath))
                Directory.CreateDirectory(_fileDownloaderSettings.SavePath);

            var fileHash = HashFileContent(file);
            var fileExtension = file.FileName().Substring(file.FileName().LastIndexOf(".", StringComparison.Ordinal));
            var uniqueIdentifier = $"{fileHash}{fileExtension}";
            var filePath = Path.Combine(_fileDownloaderSettings.SavePath, uniqueIdentifier);
            if (Path.Exists(filePath))
                return filePath;
            
            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            return filePath;
        }
        catch (Exception)
        {
            throw new FileSaveException();
        }
    }
    
    public static string HashFileContent(IAppFileProxy file)
    {
        using var stream = file.OpenReadStream();
        using var sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(stream);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
}