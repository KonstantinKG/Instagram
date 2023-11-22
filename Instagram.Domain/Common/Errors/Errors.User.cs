using ErrorOr;

namespace Instagram.Domain.Common.Errors;

public static class Errors
{
    public static class User
    {
        public static Error DuplicateEmail => Error.Conflict(
            code: "User.DuplicateEmail",
            description: "User with provided email already exists");
        public static Error InvalidCredentials => Error.Validation(
            code: "User.InvalidCredentials",
            description: "Invalid credentials");
    }
}