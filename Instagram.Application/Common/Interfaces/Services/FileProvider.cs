using System.Security.Cryptography;

using Instagram.Domain.Configurations;

namespace Instagram.Application.Common.Interfaces.Services;

public abstract class FileProvider
{
    protected readonly AppConfiguration _configuration;

    protected FileProvider(AppConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public abstract Task<string?> Save(IAppFileProxy fileProxy);
    public abstract Task<MemoryStream?> Get(string path);
    public abstract Task Delete(string path);
    
    protected static string HashFileContent(IAppFileProxy fileProxy)
    {
        using var sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(fileProxy.OpenReadStream());
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
}