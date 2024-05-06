namespace Instagram.Application.Services.PostService.Queries._Common;

public record AllResult<T>(
    long Current,
    long Pages,
    long Total,
    List<T> Data
    );