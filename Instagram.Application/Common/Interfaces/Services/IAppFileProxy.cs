namespace Instagram.Application.Common.Interfaces.Services;

public interface IAppFileProxy
{
    public string Name();
    public string FileName();
    public string ContentType();
    public long Length();
    
    public void CopyTo(Stream target);

    public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default);
}