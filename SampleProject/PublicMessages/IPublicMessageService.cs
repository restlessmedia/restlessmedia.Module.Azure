using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleProject.PublicMessages
{
  public interface IPublicMessageService
  {
    Task<IEnumerable<PublicMessage>> GetAllAsync();

    Task CreateAsync(PublicMessage publicMessage);

    Task RemoveAsync(int messageId);
  }
}