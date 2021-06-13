using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restlessmedia.Module.Azure
{
  public abstract class TableStore<T, TEntity> : TableStore<TEntity>, ITableStore<T>
    where T : class
    where TEntity : class, ITableEntity, new()
  {
    public TableStore(string tableName, TableAsyncFactory tableFactory)
      : base(tableName, tableFactory) { }

    public async virtual Task<IEnumerable<T>> GetAllAsync()
    {
      var results = await ExecuteEntityQueryAsync();
      return results.Select(CreateFromEntity);
    }

    public virtual async Task<T> GetByRowKeyAsync(string rowKey)
    {
      var entity = await GetEntityByRowKeyAsync(rowKey);
      return entity != null ? CreateFromEntity(entity) : default(T);
    }

    public virtual async Task<T> CreateAsync(T obj)
    {
      var entity = CreateEntity(obj);
      var insertedEntity = await InsertEntityAsync(entity);
      return CreateFromEntity(insertedEntity);
    }

    public virtual async Task<T> UpdateAsync(string rowKey, T obj)
    {
      var entity = CreateEntity(obj);
      var tableResult = await UpdateEntityAsync(rowKey, entity);
      var updatedEntity = tableResult.Result as TEntity;
      return CreateFromEntity(updatedEntity);
    }

    protected async virtual Task<IEnumerable<T>> ExecuteQueryAsync(TableQuery<TEntity> query = null)
    {
      var result = await ExecuteEntityQueryAsync(query);
      return result.Select(CreateFromEntity);
    }

    protected abstract T CreateFromEntity(TEntity entity);

    protected abstract TEntity CreateEntity(T obj);
  }

  public abstract class TableStore<TEntity> : IEntityTableStore<TEntity>
    where TEntity : class, ITableEntity, new()
  {
    protected readonly string TableName;
    protected readonly TableAsyncFactory TableFactory;

    public TableStore(string tableName, TableAsyncFactory tableFactory)
    {
      TableName = tableName;
      TableFactory = tableFactory;
    }

    public virtual async Task<TEntity> GetEntityByRowKeyAsync(string rowKey)
    {
      var query = NewQuery();
      query.Equal(x => x.RowKey, rowKey);
      var result = await ExecuteEntityQueryAsync(query);
      return result.FirstOrDefault();
    }

    public virtual async Task<TEntity> InsertEntityAsync(TEntity entity)
    {
      var operation = TableOperation.Insert(entity);
      var table = await GetTable();
      var result = await table.ExecuteAsync(operation);
      return result.Result as TEntity;
    }

    public virtual async Task<IEnumerable<TableResult>> InsertEntitiesAsync(IEnumerable<TEntity> entities)
    {
      TableBatchOperation batchOperation = new TableBatchOperation();

      foreach (var entity in entities)
      {
        batchOperation.Insert(entity);
      }

      var table = await GetTable();

      return await table.ExecuteBatchAsync(batchOperation);
    }

    public virtual async Task UpdateEntityAsync(string rowKey, Dictionary<string, object> properties)
    {
      var entity = new DynamicTableEntity
      {
        Properties = properties.ToDictionary(prop => prop.Key, prop => EntityProperty.CreateEntityPropertyFromObject(prop.Value)),
      };
      
      await UpdateEntityAsync(rowKey, entity);
    }

    protected virtual async Task<IEnumerable<TEntity>> ExecuteEntityQueryAsync(TableQuery<TEntity> query = null)
    {
      TableContinuationToken token = null;
      var results = new List<TEntity>();

      do
      {
        var table = await GetTable();
        var resultSegment = await table.ExecuteQuerySegmentedAsync(query ?? NewQuery(), token);
        token = resultSegment.ContinuationToken;
        foreach (var entity in resultSegment.Results)
        {
          results.Add(entity);
        }
      } while (token != null);

      return results;
    }
    
    protected virtual async Task<TableResult> UpdateEntityAsync(string rowKey, ITableEntity entity)
    {
      entity.RowKey = rowKey;
      entity.ETag = "*"; // why?
      var operation = TableOperation.Merge(entity);
      var table = await GetTable();
      var result = await table.ExecuteAsync(operation);
      return result;
    }

    protected virtual async Task DeleteEntityAsync(string rowKey)
    {
      var table = await GetTable();
      var entity = await GetEntityByRowKeyAsync(rowKey);
      var operation = TableOperation.Delete(entity);
      await table.ExecuteAsync(operation);
    }

    protected TableQuery<TEntity> NewQuery()
    {
      return new TableQuery<TEntity>();
    }

    protected Task<ITable> GetTable()
    {
      return TableFactory(TableName);
    }
  }
}