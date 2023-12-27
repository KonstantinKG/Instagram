using ErrorOr;

namespace Instagram.Domain.Common.Errors;

public static partial class Errors
{
    public static class Common
    {
        public static Error Unexpected => Error.Unexpected(
            code: "errors.common.unexpected"
        );
        
        public static Error NotFound => Error.NotFound(
            code: "errors.common.not_found"
        );
        
        public static Error AccessDenied => Error.Validation(
            code: "errors.common.access_denied"
        );
    }
}