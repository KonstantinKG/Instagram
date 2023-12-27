using Microsoft.AspNetCore.Http;

namespace Instagram.Contracts.Post.EditPostGalleryContracts;

public record EditPostGalleryRequest(
    Guid Id,
    Guid PostId,
    IFormFile File,
    string? Description,
    string? Labels
    );