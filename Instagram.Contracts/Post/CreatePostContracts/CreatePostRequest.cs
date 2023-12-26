using Microsoft.AspNetCore.Http;

namespace Instagram.Contracts.Post.CreatePostContracts;

public record CreatePostRequest(
    string Content,
    long? LocationId,
    bool HideStats,
    bool HideComments,
    List<CreatePostRequestImage> Images
    );
    
public record CreatePostRequestImage(
    IFormFile Image,
    string? Description,
    string? Labels
    );
    
public record CreatePostRequestImag2e(
    IFormFile Image,
    string? Description,
    string? Labels
);  