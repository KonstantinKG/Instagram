using ErrorOr;

namespace Instagram.Domain.Common.Errors;

public static partial class Errors
{
    public static class Auth
    {
        public static Error InvalidToken => Error.Validation(
            code: "errors.auth.invalid_token"
        );
    }
}