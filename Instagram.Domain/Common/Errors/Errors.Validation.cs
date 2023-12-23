using ErrorOr;

namespace Instagram.Domain.Common.Errors;

public static partial class Errors
{
    public static class Validation
    {
        public static Error MinimumLength => Error.Validation(
            code: "errors.validation.{0}_minimum_length:{1}"
        );
        
        public static Error MaximumLength => Error.Validation(
            code: "errors.validation.{0}_maximum_length:{1}"
        );
        
        public static Error Regex => Error.Validation(
            code: "errors.validation.{0}_regex"
        );
        
        public static Error Required => Error.Validation(
            code: "errors.validation.{0}_required"
        );
    }
}