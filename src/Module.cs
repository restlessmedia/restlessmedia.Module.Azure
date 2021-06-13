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
      containerBuilder.Register<TableAsyncFactory>(provider => provider.Resolve<StorageAccount>().GetOrCreateTableAsync);
    }
  }
}