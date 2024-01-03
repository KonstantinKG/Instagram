using System.Runtime.Serialization;

namespace Instagram.Domain.Common.Exceptions;

[Serializable]
public class FileSaveException : Exception
{
    public DateTime Timestamp { get; }
    public string? AdditionalInformation { get; }
    
    public FileSaveException()
    {
        Timestamp = DateTime.UtcNow;
    }

    public FileSaveException(string message) : base(message)
    {
        Timestamp = DateTime.UtcNow;
    }

    public FileSaveException(string message, Exception innerException) : base(message, innerException)
    {
        Timestamp = DateTime.UtcNow;
    }
    
    protected FileSaveException(SerializationInfo info, StreamingContext context) : base(info, context)
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