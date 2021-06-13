namespace SampleProject.Requests
{
  public class CreatePublicMessageRequest
  {
    public string Title { get; set; }

    public MessageType MessageType { get; set; }

    public string Message { get; set; }

    public string Url { get; set; }
  }
}