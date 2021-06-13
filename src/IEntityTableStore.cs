using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restlessmedia.Module.Azure
{
  public interface IEntityTableStore<TEntity>
    where TEntity : class, ITableEntity, new()
  {
    Task<TEntity> GetEntityByRowKeyAsync(string rowKey);

    Task<TEntity> InsertEntityAsync(TEntity entity);

    Task<IEnumerable<TableResult>> InsertEntitiesAsync(IEnumerable<TEntity> entities);

    Task UpdateEntityAsync(string rowKey, Dictionary<string, object> properties);
  }
}