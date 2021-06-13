using Microsoft.WindowsAzure.Storage.Blob;

namespace restlessmedia.Module.Azure
{
  public interface IBlobContainer
  {
    CloudBlockBlob GetBlockBlobReference(string name);
  }
}