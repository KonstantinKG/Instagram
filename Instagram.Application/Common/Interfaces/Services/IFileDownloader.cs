using Microsoft.AspNetCore.Http;

namespace Instagram.Application.Common.Interfaces.Services;

public interface IFileDownloader
{
    Task<string> Download(IAppFileProxy file);
}