using Microsoft.AspNetCore.Http;

namespace Instagram.Contracts.Post.UpdatePostGallery;

public record UpdatePostGalleryRequest(
    Guid Id,
    Guid PostId,
    IFormFile File,
    string? Description,
    string? Labels
    );