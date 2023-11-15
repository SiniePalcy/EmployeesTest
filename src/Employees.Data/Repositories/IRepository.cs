using Employees.Data.Entities;
using Employees.Domain.Model;

namespace Employees.Data.Repositories;

internal interface IRepository<TModel, TEntity>
    where TModel: BaseObject
    where TEntity: IEntity
{
    Task<IEnumerable<TModel>> GetAsync(List<int>? ids = null);
    Task<SystemLog> AddAsync(TModel model);
    Task<SystemLog> UpdateAsync(TModel model);
    Task DeleteAsync(int id);
    Task SaveAsync();
}
