namespace Instagram.Infrastructure.Services.FileDownloaderService;

public class FileDownloaderSettings
{
    public const string SectionName = "Files";
    public string SavePath { get; set; } = null!;
};