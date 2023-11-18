using Instagram.Application.Common.Interfaces.Services;

namespace Instagram.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}