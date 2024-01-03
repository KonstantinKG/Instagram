namespace Instagram.Application.Services.PostService.Commands.AddPostComment;

public record AddPostCommentCommand(
    Guid UserId,
    Guid PostId,
    Guid? ParentId,
    string Content
);
    