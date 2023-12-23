using ErrorOr;

namespace Instagram.Domain.Common.Errors;

public static partial class Errors
{
    public static class Common
    {
        public static Error Unexpected => Error.Unexpected(
            code: "errors.common.unexpected"
        );
    }
}