using Microsoft.WindowsAzure.Storage.Blob;

namespace restlessmedia.Module.Azure
{
  public class CloudBlobContainer : IBlobContainer
  {
    private readonly Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer _container;

    public CloudBlobContainer(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container)
    {
      _container = container;
    }

    public CloudBlockBlob GetBlockBlobReference(string name)
    {
      return _container.GetBlockBlobReference(name);
    }
  }
}