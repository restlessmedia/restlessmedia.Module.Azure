using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleProject.PublicMessages
{
  internal class PublicMessageService : IPublicMessageService
  {
    private readonly IPublicMessageStore _store;

    public PublicMessageService(IPublicMessageStore store)
    {
      _store = store ?? throw new ArgumentNullException(nameof(store));
    }

    public Task<IEnumerable<PublicMessage>> GetAllAsync()
    {
      return _store.GetAllAsync();
    }

    public Task CreateAsync(PublicMessage publicMessage)
    {
      return _store.CreateAsync(publicMessage);
    }

    public Task RemoveAsync(int messageId)
    {
      throw new NotImplementedException();
    }
  }
}