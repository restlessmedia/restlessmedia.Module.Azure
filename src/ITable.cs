using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restlessmedia.Module.Azure
{
  public interface ITable
  {
    Task<TableQuerySegment<TEntity>> ExecuteQuerySegmentedAsync<TEntity>(TableQuery<TEntity> query, TableContinuationToken token)
      where TEntity : class, ITableEntity, new();

    Task<TableResult> ExecuteAsync(TableOperation operation);

    Task<IList<TableResult>> ExecuteBatchAsync(TableBatchOperation batch);
  }
}