using Microsoft.WindowsAzure.Storage.Table;

namespace SampleProject.PublicMessages
{
  public class PublicMessageEntity : TableEntity
  {
    public PublicMessageEntity()
    {
      PartitionKey = string.Empty;
    }

    public PublicMessageEntity(PublicMessage publicMessage)
      : this()
    {
      RowKey = publicMessage.MessageId;
      Title = publicMessage.Title;
      MessageType = (int)publicMessage.MessageType;
      Message = publicMessage.Message;
      Url = publicMessage.Url;
    }

    public string Title { get; set; }

    public int MessageType { get; set; }

    public string Message { get; set; }

    public string Url { get; set; }
  }
}