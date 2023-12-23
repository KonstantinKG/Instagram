using System.Runtime.Serialization;

namespace Instagram.Domain.Common.Exceptions;

[Serializable]
public class FileDownloadException : Exception
{
    public DateTime Timestamp { get; }
    public string? AdditionalInformation { get; }
    
    public FileDownloadException()
    {
        Timestamp = DateTime.UtcNow;
    }

    public FileDownloadException(string message) : base(message)
    {
        Timestamp = DateTime.UtcNow;
    }

    public FileDownloadException(string message, Exception innerException) : base(message, innerException)
    {
        Timestamp = DateTime.UtcNow;
    }
    
    protected FileDownloadException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        Timestamp = info.GetDateTime("Timestamp");
        AdditionalInformation = info.GetString("AdditionalInformation");
    }
    
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue("Timestamp", Timestamp);
        info.AddValue("AdditionalInformation", AdditionalInformation);
    }
}