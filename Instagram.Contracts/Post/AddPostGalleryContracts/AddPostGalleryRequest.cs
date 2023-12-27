using Microsoft.AspNetCore.Http;

namespace Instagram.Contracts.Post.AddPostGalleryContracts;

public record AddPostGalleryRequest(
    Guid PostId,
    IFormFile File,
    string? Description,
    string? Labels
    );