using ErrorOr;

namespace Instagram.Domain.Common.Errors;

public static class Errors
{
    public static class User
    {
        public static Error InvalidCredentials => Error.Validation(
            code: "User.InvalidCredentials",
            description: "Некорректные данные пользователя");
    }
}