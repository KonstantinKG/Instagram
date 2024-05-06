namespace Instagram.Contracts.Common;

public record AllResponse<T>(
    long current,
    long pages,
    long total,
    List<T> data
    );