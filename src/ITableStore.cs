using System.Collections.Generic;
using System.Threading.Tasks;

namespace restlessmedia.Module.Azure
{
  public interface ITableStore<T>
    where T : class
  {
    Task<IEnumerable<T>> GetAllAsync();

    Task<T> CreateAsync(T obj);

    Task<T> UpdateAsync(string rowKey, T obj);

    Task<T> GetByRowKeyAsync(string rowKey);
  }
}