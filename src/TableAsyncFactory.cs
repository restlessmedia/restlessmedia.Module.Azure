using System.Threading.Tasks;

namespace restlessmedia.Module.Azure
{
  public delegate Task<ITable> TableAsyncFactory(string name);
}