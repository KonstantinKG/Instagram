namespace Instagram.Application.Common.Interfaces.Authentication;

public interface IJwtTokenHasher
{
    public string HashToken(string token);
}