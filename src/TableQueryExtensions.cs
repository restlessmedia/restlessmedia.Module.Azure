using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq.Expressions;

namespace restlessmedia.Module.Azure
{
  public static class TableQueryExtensions
  {
    public static TableQuery<TEntity> Equal<TEntity, TProp>(this TableQuery<TEntity> query, Expression<Func<TEntity, TProp>> exp, TProp value)
      where TEntity : ITableEntity, new()
    {
      return new TableQueryBuilder<TEntity>(query)
        .And(exp, value)
        .GetQuery();
    }

    public static TableQuery<TEntity> NotEqual<TEntity, TProp>(this TableQuery<TEntity> query, Expression<Func<TEntity, TProp>> exp, TProp value)
      where TEntity : ITableEntity, new()
    {
      return new TableQueryBuilder<TEntity>(query)
        .And(exp, QueryComparisons.NotEqual, value)
        .GetQuery();
    }

    public static TableQueryBuilder<TEntity> CreateBuilder<TEntity>(this TableQuery<TEntity> query)
      where TEntity : ITableEntity, new()
    {
      return new TableQueryBuilder<TEntity>(query);
    }
  }
}
