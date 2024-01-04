using Instagram.Application.Common.Interfaces.Services;

using Microsoft.AspNetCore.Http;

namespace Instagram.Infrastructure.Services;

public class AppFileProxy : IAppFileProxy
{
    private readonly IFormFile _formFile;

    public AppFileProxy(IFormFile formFile)
    {
        _formFile = formFile ?? throw new ArgumentNullException(nameof(formFile));
    }

    public string Name()
    {
        return _formFile.Name;
    }

    public string FileName()
    {
        return _formFile.FileName;
    }

    public string ContentType()
    {
        return _formFile.ContentType;
    }

    public long Length()
    {
        return _formFile.Length;
    }

    public void CopyTo(Stream target)
    {
        _formFile.CopyTo(target);
    }

    public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
    {
        return _formFile.CopyToAsync(target, cancellationToken);
    }

    public Stream OpenReadStream()
    {
        return _formFile.OpenReadStream();
    }
}