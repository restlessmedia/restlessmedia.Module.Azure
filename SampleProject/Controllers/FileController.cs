using restlessmedia.Module.Azure;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace SampleProject.Controllers
{
  public class FileController : ApiController
  {
    private readonly IBlobStore _blobStore;

    public FileController(IBlobStoreFactory storeFactory)
    {
      _blobStore = storeFactory("sample-project-images");
    }

    [HttpGet]
    public async Task<HttpResponseMessage> GetAsync(string name)
    {
      var blob = await _blobStore.GetBlobAsync(name);
      var response = Request.CreateResponse(HttpStatusCode.OK);
      response.Content = new StreamContent(blob.OpenRead());
      response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
      return response;
    }

    [HttpPost]
    public async Task<IHttpActionResult> PostAsync(string name)
    {
      try
      {
        var stream = await Request.Content.ReadAsStreamAsync();
        var blob = await _blobStore.UploadFromStreamAsync(name, "", stream);
      }
      catch (Exception e)
      {
        return InternalServerError(e);
      }

      return Ok();
    }
  }
}