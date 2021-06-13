using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleProject.PublicMessages
{
  public interface IPublicMessageStore
  {
    Task<IEnumerable<PublicMessage>> GetAllAsync();

    Task<PublicMessage> CreateAsync(PublicMessage publicMessage);
  }
}