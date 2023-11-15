using Employees.Data.Entities;
using Employees.Domain.Model;

namespace Employees.Data.Mapping;

public interface IMapper<TModel, TEntity>
    where TModel : BaseObject
    where TEntity : IEntity
{
    TModel? ToModel(TEntity? entity);
    TEntity? ToEntity(TModel? model);
    IEnumerable<TModel?>? ToModels(IEnumerable<TEntity>? entities);
    IEnumerable<TEntity?>? ToEntities(IEnumerable<TModel>? models);
}
