using Instagram.Application.Common.Interfaces.Services;

namespace Instagram.Application.Services.PostService.Commands;

public record CreatePostCommand(
    string UserId,
    string Content,
    long LocationId,
    bool HideStats,
    bool HideComments,
    List<CreatePostCommandImage> Images
    );
    
public record CreatePostCommandImage(
    IAppFileProxy Image,
    string Description,
    string Labels
);