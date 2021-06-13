using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace restlessmedia.Module.Azure
{
  internal class BlobStore : IBlobStore
  {
    private readonly string _containerName;
    private readonly IBlobContainerAsyncFactory _containerFactory;

    public BlobStore(string containerName, IBlobContainerAsyncFactory containerFactory)
    {
      _containerName = containerName;
      _containerFactory = containerFactory;
    }

    // <inheritdoc />
    public virtual async Task<Stream> GetStreamAsync(string name)
    {
      var (exists, blob) = await ExistsAsync(name);

      if (!exists)
      {
        return null;
      }

      return await blob.OpenReadAsync();
    }

    // <inheritdoc />
    public virtual async Task<ICloudBlob> UploadFromStreamAsync(string name, string contentType, Stream stream)
    {
      var (exists, blob) = await ExistsAsync(name);

      if (exists)
      {
        throw new Exception($"A blob with the name {name} already exists in the container {_containerName}.");
      }

      blob.Properties.ContentType = contentType;
      await blob.UploadFromStreamAsync(stream);

      return blob;
    }

    // <inheritdoc />
    public virtual async Task<Stream> OpenWriteAsync(string name, string contentType)
    {
      var blobContainer = await GetContainerAsync();
      var blob = blobContainer.GetBlockBlobReference(name);
      blob.Properties.ContentType = contentType;
      return await blob.OpenWriteAsync();
    }

    // <inheritdoc />
    public virtual async Task<(bool, ICloudBlob)> ExistsAsync(string name)
    {
      var blobContainer = await GetContainerAsync();
      var blob = blobContainer.GetBlockBlobReference(name);
      var exists = await blob.ExistsAsync();
      return (exists, blob);
    }

    // <inheritdoc />
    public virtual async Task<ICloudBlob> GetBlobAsync(string name)
    {
      var (exists, blob) = await ExistsAsync(name);
      return exists ? blob : null;
    }

    /// <summary>
    /// Returns the container or creates a new one with public access.
    /// </summary>
    /// <returns></returns>
    private Task<IBlobContainer> GetContainerAsync()
    {
      return _containerFactory(_containerName, true);
    }
  }
}