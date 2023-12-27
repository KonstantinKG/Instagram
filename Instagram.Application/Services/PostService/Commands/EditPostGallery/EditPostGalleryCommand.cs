using Instagram.Application.Common.Interfaces.Services;

namespace Instagram.Application.Services.PostService.Commands.EditPostGallery;

public record EditPostGalleryCommand(
    Guid Id,
    Guid PostId,
    Guid UserId,
    IAppFileProxy File,
    string? Description,
    string? Labels
);
    