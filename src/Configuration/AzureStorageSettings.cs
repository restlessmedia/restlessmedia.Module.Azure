using System.Configuration;

namespace restlessmedia.Module.Azure.Configuration
{
  public class AzureStorageSettings : ConfigurationElement, IAzureStorageSettings
  {
    [ConfigurationProperty(_connectionStringProperty, IsRequired = true)]
    public string ConnectionString
    {
      get
      {
        return (string)this[_connectionStringProperty];
      }
      set
      {
        this[_connectionStringProperty] = value;
      }
    }

    private const string _connectionStringProperty = "connectionString";
  }
}
