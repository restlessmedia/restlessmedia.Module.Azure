using SampleProject.PublicMessages;
using SampleProject.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace SampleProject.Controllers
{
  public class HomeController : ApiController
  {
    private readonly IPublicMessageService _publicMessageService;

    public HomeController(IPublicMessageService publicMessageService)
    {
      _publicMessageService = publicMessageService ?? throw new ArgumentNullException(nameof(publicMessageService));
    }

    [HttpGet]
    public Task<IEnumerable<PublicMessage>> GetAsync()
    {
      return _publicMessageService.GetAllAsync();
    }

    [HttpPost]
    public async Task<PublicMessage> PostAsync(CreatePublicMessageRequest request)
    {
      var publicMessage = new PublicMessage
      {
        Title = request.Title,
        MessageType = MessageType.Homepage,
        Url = request.Url,
        Message = request.Message
      };

      await _publicMessageService.CreateAsync(publicMessage);

      return publicMessage;
    }
  }
}