using Microsoft.AspNetCore.Http;

namespace Instagram.Contracts.User.UpdateUserProfileContracts;

public record UpdateUserProfileRequest(
    string fullname,
    IFormFile? image,
    string? bio,
    string? gender
);