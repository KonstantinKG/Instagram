using ErrorOr;

namespace Instagram.Domain.Common.Errors;

public static partial class Errors
{
    public static class Post
    {
        public static Error GalleriesNotFound => Error.NotFound(
            code: "errors.post.galleries_not_found"
        );
    }
}