namespace Instagram.Application.Services._Common;

public record SliderResult<T>(
    Guid? Previous,
    Guid? Next,
    T Data
    );