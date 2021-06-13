using System;

namespace SampleProject.PublicMessages
{
  public class PublicMessage
  {
    public PublicMessage()
    {
      MessageId = Guid.NewGuid().ToString();
    }

    public PublicMessage(string messageId, string title, MessageType messageType, string message, string url)
    {
      MessageId = messageId;
      Title = title;
      MessageType = messageType;
      Message = message;
      Url = url;
    }
    
    public string MessageId { get; set; }

    public string Title { get; set; }

    public MessageType MessageType { get; set; }

    public string Message { get; set; }

    public string Url { get; set; }
  }
}