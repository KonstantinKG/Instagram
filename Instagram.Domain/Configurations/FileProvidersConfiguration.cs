namespace Instagram.Domain.Configurations;

public class FileProvidersConfiguration
{
    public S3ProviderConfiguration S3 { get; set; } = null!;
}

public class S3ProviderConfiguration
{
    public string Bucket { get; set; } = null!;
}