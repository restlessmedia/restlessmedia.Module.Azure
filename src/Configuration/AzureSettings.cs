
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;


namespace restlessmedia.Module.Azure.Configuration
{
  public class AzureSettings : SerializableConfigurationSection, IAzureSettings
  {
    public IAzureStorageSettings Storage
    {
      get
      {
        return StorageElement;
      }
    }

    [ConfigurationProperty(_storageProperty)]
    private AzureStorageSettings StorageElement
    {
      get
      {
        return (AzureStorageSettings)this[_storageProperty];
      }
    }

    private const string _storageProperty = "storage";
  }
}
