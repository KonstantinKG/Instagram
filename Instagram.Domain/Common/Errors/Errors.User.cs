using ErrorOr;

namespace Instagram.Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error InvalidCredentials => Error.Validation(
            code: "errors.user.invalid_credentials"
        );
        
        public static Error UniqueEmail => Error.Conflict(
            code: "errors.user.unique_email"
        );
        
        public static Error UniqueUsername => Error.Conflict(
            code: "errors.user.unique_username"
        );
        
        public static Error UniquePhone => Error.Conflict(
            code: "errors.user.unique_phone"
        );
    }
}