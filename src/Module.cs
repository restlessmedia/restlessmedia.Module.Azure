using Autofac;
using restlessmedia.Module.Azure.Configuration;

namespace restlessmedia.Module.Azure
{
  public class Module : IModule
  {
    public void RegisterComponents(ContainerBuilder containerBuilder)
    {
      containerBuilder.RegisterSettings<IAzureSettings>("restlessmedia/azure", required: true);
      containerBuilder.RegisterType<StorageAccount>().SingleInstance();
      containerBuilder.Register<IBlobContainerAsyncFactory>(provider => provider.Resolve<StorageAccount>().GetOrCreateContainerAsync);
      containerBuilder.Register<TableAsyncFactory>(provider => provider.Resolve<StorageAccount>().GetOrCreateTableAsync);
      containerBuilder.Register<IBlobStoreFactory>(provider =>
      {
        var factory = provider.Resolve<IBlobContainerAsyncFactory>();
        return (name) => new BlobStore(name, factory);
      });
    }
  }
}