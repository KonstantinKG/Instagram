using Instagram.Domain.Configurations;

using Microsoft.Extensions.Options;

using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace Instagram.Infrastructure.Persistence.S3;

public class S3Context
{
    private readonly AppConfiguration _configuration;

    public S3Context(IOptions<AppConfiguration> options)
    {
        _configuration = options.Value;
    }

    private IMinioClient CreateClient()
    {
        return new MinioClient()
            .WithEndpoint(_configuration.Connections.Minio.Url)
            .WithCredentials(_configuration.Connections.Minio.AccessKey, _configuration.Connections.Minio.SecretKey)
            .WithSSL(false)
            .Build();
    }
    
    public async Task<MemoryStream?> Get(string key, string bucket)
    {
        try
        {
            using var s3Client = CreateClient();
            var fileStream = new MemoryStream();
            
            var request = new GetObjectArgs()
                .WithBucket(bucket)
                .WithObject(key)
                .WithCallbackStream(stream => stream.CopyTo(fileStream));

            _ = await s3Client.GetObjectAsync(request);
            return fileStream;
        }
        catch (ObjectNotFoundException)
        {
            return null;
        }
        
    } 
    
    public async Task Save(string key, string bucket, string path, Dictionary<string, string> metadata)
    {
        var s3Client = CreateClient();

        var stream = new FileStream(path, FileMode.Open);
        var args = new PutObjectArgs()
            .WithBucket(bucket)
            .WithObject(key)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithHeaders(metadata)
            .WithContentType("application/octet-stream");
        await s3Client.PutObjectAsync(args);
        await stream.DisposeAsync();
    }

    public async Task Remove(string key, string bucket)
    {
        var client = CreateClient();
        var args = new RemoveObjectArgs()
            .WithBucket(bucket)
            .WithObject(key);
        
        await client.RemoveObjectAsync(args);
    }
}