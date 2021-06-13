using System.Threading.Tasks;

namespace restlessmedia.Module.Azure
{
  internal delegate Task<IBlobContainer> IBlobContainerAsyncFactory(string name, bool publicBlobs);
}