using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.UserAggregate;

public sealed class User : AggregateRoot<long>
{
    public string Username { get; private set; }
    public string Fullname { get; private set; }
    public string Email { get; private set; }
    public string? Phone { get; private set; }
    public string Password { get; private set; }


    private User(
        string username,
        string fullname,
        string email,
        string? phone,
        string password)
    {
        Username = username;
        Fullname = fullname;
        Email = email;
        Phone = phone;
        Password = password;
    }

    public static User Create(
        string username,
        string fullname,
        string email,
        string? phone,
        string password)
    {
        return new User(
            username,
            fullname,
            email,
            phone,
            password
        );
    }

    # pragma warning disable CS8618
    private User()
    {
    }
    # pragma warning disable CS8618
}