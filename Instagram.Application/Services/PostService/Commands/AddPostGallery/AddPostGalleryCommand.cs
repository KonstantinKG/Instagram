using Instagram.Application.Common.Interfaces.Services;

namespace Instagram.Application.Services.PostService.Commands.AddPostGallery;

public record AddPostGalleryCommand(
    Guid UserId,
    Guid PostId,
    IAppFileProxy File,
    string? Description,
    string? Labels
);
    