using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Threading.Tasks;

namespace restlessmedia.Module.Azure
{
  public interface IBlobStore
  {
    Task<Stream> GetStreamAsync(string name);

    /// <summary>
    /// Returns whether the blob exists and the blob for the given <paramref name="name"/>.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<(bool, ICloudBlob)> ExistsAsync(string name);

    /// <summary>
    /// Returns a <see cref="ICloudBlob"/> for the given <paramref name="name"/> if it exists or null if it doesn't.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<ICloudBlob> GetBlobAsync(string name);

    Task<ICloudBlob> UploadFromStreamAsync(string name, string contentType, Stream stream);

    Task<Stream> OpenWriteAsync(string name, string contentType);
  }
}