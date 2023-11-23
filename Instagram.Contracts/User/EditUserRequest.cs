using Microsoft.AspNetCore.Http;

namespace Instagram.Contracts.User;

public record EditUserRequest(
    string Username,
    string Fullname,
    string Email,
    string? Phone,
    IFormFile? Image,
    string? Bio
);