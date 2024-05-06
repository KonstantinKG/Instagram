using Instagram.Application.Common.Interfaces.Services;
using Instagram.Domain.Common.Exceptions;
using Instagram.Domain.Configurations;
using Instagram.Infrastructure.Persistence.S3;

using Microsoft.Extensions.Options;

namespace Instagram.Infrastructure.Services.FileProviders;

public class S3Provider : FileProvider
{
    private readonly S3Context _s3Context;

    public S3Provider(IOptions<AppConfiguration> options, S3Context s3Context) : base(options.Value)
    {
        _s3Context = s3Context;
    }
    
    public override async Task<string?> Save(IAppFileProxy fileProxy)
    {
        string? tempPath = null;
        try
        {
            var hash = HashFileContent(fileProxy);
            var key = MakeSaveKey(hash);
            var metadata = MakeFileMetadata(fileProxy);

            tempPath = await SaveToTemporaryFile(fileProxy);
            await _s3Context.Save(key, _configuration.FileProviders.S3.Bucket, tempPath, metadata);
            
            return key;
        }
        catch (Exception) { throw new FileSaveException(); }
        finally { if (tempPath != null) File.Delete(tempPath); }
    }

    public override Task<MemoryStream?> Get(string path)
    {
        return _s3Context.Get(path, _configuration.FileProviders!.S3!.Bucket);
    }

    public override Task Delete(string path)
    {
        return _s3Context.Remove(path, _configuration.FileProviders!.S3!.Bucket);
    }

    private static string MakeSaveKey(string hash)
    {
        return Path.Combine(hash, "src");
    }

    private async static Task<string> SaveToTemporaryFile(IAppFileProxy fileProxy)
    {
        var temporaryFile = Guid.NewGuid().ToString();
        await using var saveStream = new FileStream(temporaryFile, FileMode.Create);
        await fileProxy.CopyToAsync(saveStream);
        return temporaryFile;
    }

    private static Dictionary<string, string> MakeFileMetadata(IAppFileProxy fileProxy)
    {
        return new Dictionary<string, string> { { "Name", fileProxy.FileName() } };
    }
}