using Microsoft.AspNetCore.Http;

namespace Instagram.Contracts.User.UpdateUserProfileContracts;

public record UpdateUserProfileRequest(
    string Fullname,
    IFormFile? Image,
    string? Bio,
    string? Gender
);