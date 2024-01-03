using Instagram.Application.Common.Interfaces.Services;

namespace Instagram.Application.Services.PostService.Commands.UpdatePostGallery;

public record UpdatePostGalleryCommand(
    Guid Id,
    Guid PostId,
    Guid UserId,
    IAppFileProxy File,
    string? Description,
    string? Labels
);
    