using Instagram.Application.Common.Interfaces.Persistence;
using Instagram.Domain.Entities;

namespace Instagram.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private static readonly List<User> Users = new();
    public User? GetUserByEmail(string email)
    {
        return Users.FirstOrDefault(u => u.Email == email);
    }

    public void AddUser(User user)
    {
        Users.Add(user);
    }
}