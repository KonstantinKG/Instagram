using ErrorOr;

namespace Instagram.Domain.Common.Errors;

public static partial class Errors
{
    public static class File
    {
        public static Error DownloadFailed => Error.Failure(
            code: "errors.file.download_failed"
        );
    }
}