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

    public async Task<string> Download(IAppFileProxy file, string path)
    {
        try
        {
            var dirFolder = Path.Combine(_fileDownloaderSettings.SavePath, path);
            if (!Directory.Exists(dirFolder))
                Directory.CreateDirectory(dirFolder);

            var uniqueIdentifier =
                $"{Guid.NewGuid()}{file.FileName().Substring(file.FileName().LastIndexOf(".", StringComparison.Ordinal))}";
            var filePath = Path.Combine(dirFolder, uniqueIdentifier);
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
}