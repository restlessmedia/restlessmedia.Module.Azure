using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restlessmedia.Module.Azure
{
  internal class Table : ITable
  {
    private readonly CloudTable _cloudTable;

    public Table(CloudTable cloudTable)
    {
      _cloudTable = cloudTable ?? throw new ArgumentNullException(nameof(cloudTable));
    }

    public Task<TableResult> ExecuteAsync(TableOperation operation)
    {
      return _cloudTable.ExecuteAsync(operation);
    }

    public Task<TableQuerySegment<TEntity>> ExecuteQuerySegmentedAsync<TEntity>(TableQuery<TEntity> query, TableContinuationToken token)
      where TEntity : class, ITableEntity, new()
    {
      return _cloudTable.ExecuteQuerySegmentedAsync(query, token);
    }

    public Task<IList<TableResult>> ExecuteBatchAsync(TableBatchOperation batch)
    {
      return _cloudTable.ExecuteBatchAsync(batch);
    }
  }
}