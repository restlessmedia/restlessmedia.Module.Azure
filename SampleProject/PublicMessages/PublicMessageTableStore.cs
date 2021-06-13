using restlessmedia.Module.Azure;
using System.Threading.Tasks;

namespace SampleProject.PublicMessages
{
  public class PublicMessageTableStore : TableStore<PublicMessage, PublicMessageEntity>, IPublicMessageStore
  {
    public PublicMessageTableStore(TableAsyncFactory tableFactory)
      : base("PublicMessage", tableFactory) { }

    protected override PublicMessageEntity CreateEntity(PublicMessage publicMessage)
    {
      return new PublicMessageEntity(publicMessage);
    }

    protected override PublicMessage CreateFromEntity(PublicMessageEntity entity)
    {
      return new PublicMessage(
        entity.RowKey,
        entity.Title,
        (MessageType)entity.MessageType,
        entity.Message,
        entity.Url
      );
    }
  }
}