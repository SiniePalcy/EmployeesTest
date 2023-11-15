using Employees.Data.Entities;
using Employees.Domain.Model;

namespace Employees.Data.Mapping;

internal abstract class BaseMapper<TModel, TEntity> : IMapper<TModel, TEntity>
    where TModel : BaseObject
    where TEntity : BaseEntity
{
    public abstract TModel? ToModel(TEntity? entity);
    public abstract TEntity? ToEntity(TModel? model);

    public virtual IEnumerable<TModel?>? ToModels(IEnumerable<TEntity>? entities)
    {
        return entities?.Select(ToModel);
    }

    public virtual IEnumerable<TEntity?>? ToEntities(IEnumerable<TModel>? models)
    {
        return models?.Select(ToEntity);
    }
}
