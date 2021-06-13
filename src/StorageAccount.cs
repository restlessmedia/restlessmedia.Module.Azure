using Microsoft.WindowsAzure.Storage;
using restlessmedia.Module.Azure.Configuration;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace restlessmedia.Module.Azure
{
  /// <summary>
  /// Provides factory methods for returning cloud storage 'features' like table, queue and blob container.
  /// </summary>
  internal class StorageAccount
  {
    private readonly IAzureSettings _azureSettings;

    public StorageAccount(IAzureSettings azureSettings)
    {
      _azureSettings = azureSettings ?? throw new ArgumentNullException(nameof(azureSettings));
    }

    /// <summary>
    /// Returns a <see cref="ITable"/> for the given name using the account found in configuration settings.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<ITable> GetOrCreateTableAsync(string name)
    {
      CloudStorageAccount storageAccount = CreateAccountFromSettings();
      var cloudTable = await GetOrCreateTableAsync(storageAccount, name);
      return new Table(cloudTable);
    }

    /// <summary>
    /// Returns a <see cref="Microsoft.WindowsAzure.Storage.Table.CloudTable"/> for the given name.
    /// </summary>
    /// <param name="storageAccount"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    private async Task<Microsoft.WindowsAzure.Storage.Table.CloudTable> GetOrCreateTableAsync(CloudStorageAccount storageAccount, string name)
    {
      var tableClient = storageAccount.CreateCloudTableClient();
      var table = tableClient.GetTableReference(name);

      try
      {
        var exists = await table.ExistsAsync();

        if (!exists)
        {
          await table.CreateAsync();
        }
      }
      catch (Exception e)
      {
        Trace.TraceError($"An error checking table '{name}' existance and/or table creation failed. {e.Message}.");
        throw;
      }

      return table;
    }

    private CloudStorageAccount CreateAccountFromSettings()
    {
      CloudStorageAccount storageAccount;

      try
      {
        storageAccount = CloudStorageAccount.Parse(_azureSettings.Storage.ConnectionString);
      }
      catch (Exception e) when (e is FormatException || e is ArgumentException)
      {
        Trace.TraceError("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the configuration.");
        throw;
      }

      return storageAccount;
    }
  }
}