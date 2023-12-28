namespace Instagram.Application.Services.PostService.Commands.DeletePostGallery;

public record DeletePostGalleryCommand(
    Guid Id,
    Guid UserId,
    Guid PostId
);
    