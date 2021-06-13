using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq.Expressions;

namespace restlessmedia.Module.Azure
{
  public class TableQueryBuilder<TEntity>
    where TEntity : ITableEntity, new()
  {
    private readonly TableQuery<TEntity> _query;

    public TableQueryBuilder(TableQuery<TEntity> query)
    {
      _query = query;
    }

    public TableQueryBuilder<TEntity> And<TProp>(Expression<Func<TEntity, TProp>> exp, TProp value)
    {
      return And(exp, QueryComparisons.Equal, value);
    }

    public TableQueryBuilder<TEntity> And<TProp>(Expression<Func<TEntity, TProp>> exp, string operation, TProp value)
    {
      return Add(exp, operation, TableOperators.And, value);
    }

    public TableQueryBuilder<TEntity> Add<TProp>(Expression<Func<TEntity, TProp>> exp, string operation, string operatorString, TProp value)
    {
      var expression = (MemberExpression)exp.Body;
      var name = expression.Member.Name;
      var filter = GenerateFilterCondition(name, operation, value);

      if (string.IsNullOrEmpty(_query.FilterString))
      {
        _query.Where(filter);
      }
      else
      {
        // this isn't perfect (see the covering test)
        // if we already have filters they will be grouped against the new filter
        _query.Where(TableQuery.CombineFilters(_query.FilterString, operatorString, filter));
      }

      return this;
    }

    public TableQuery<TEntity> GetQuery()
    {
      return _query;
    }

    /// <summary>
    /// Returns the filter condition based on the value type of <paramref name="givenValue"/>.
    /// </summary>
    /// <param name="propertyName"></param>
    /// <param name="operation"></param>
    /// <param name="givenValue"></param>
    /// <returns></returns>
    private static string GenerateFilterCondition<T>(string propertyName, string operation, T givenValue)
    {
      // Could this all be improved?

      if (TryCast<T, string>(givenValue, out var stringValue))
      {
        return TableQuery.GenerateFilterCondition(propertyName, operation, stringValue);
      }

      if (TryCast<T, bool>(givenValue, out var boolValue))
      {
        return TableQuery.GenerateFilterConditionForBool(propertyName, operation, boolValue);
      }

      if (TryCast<T, int>(givenValue, out var intValue))
      {
        return TableQuery.GenerateFilterConditionForInt(propertyName, operation, intValue);
      }

      if (TryCast<T, DateTime>(givenValue, out var dateValue))
      {
        return TableQuery.GenerateFilterConditionForDate(propertyName, operation, dateValue);
      }

      if (TryCast<T, double>(givenValue, out var doubleValue))
      {
        return TableQuery.GenerateFilterConditionForDouble(propertyName, operation, doubleValue);
      }

      if (TryCast<T, decimal>(givenValue, out var decimalValue))
      {
        return TableQuery.GenerateFilterConditionForDouble(propertyName, operation, decimal.ToDouble(decimalValue));
      }

      if (TryCast<T, Guid>(givenValue, out var guidValue))
      {
        return TableQuery.GenerateFilterConditionForGuid(propertyName, operation, guidValue);
      }

      if (TryCast<T, long>(givenValue, out var longValue))
      {
        return TableQuery.GenerateFilterConditionForLong(propertyName, operation, longValue);
      }

      return TableQuery.GenerateFilterCondition(propertyName, operation, givenValue?.ToString());
    }

    private static bool TryCast<T, TOut>(T value, out TOut result)
    {
      result = default(TOut);

      if (value is TOut)
      {
        result = (TOut)Convert.ChangeType(value, typeof(TOut));
        return true;
      }

      return false;
    }
  }
}