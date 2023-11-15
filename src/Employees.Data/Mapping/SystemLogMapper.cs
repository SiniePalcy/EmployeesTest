using Employees.Data.Entities;
using Employees.Domain.Model;

namespace Employees.Data.Mapping;

internal class SystemLogMapper : BaseMapper<SystemLog, SystemLogEntity>
{
    public override SystemLogEntity? ToEntity(SystemLog? model)
    {
        if (model is null)
        {
            return null;
        }

        return new SystemLogEntity
        {
            Id = model.Id,
            ResourceType = model.ResourceType,
            Event = model.Event,
            ChangeSet = model.ChangeSet,
            Comment = model.Comment,
        };
    }

    public override SystemLog? ToModel(SystemLogEntity? entity)
    {
        if (entity is null)
        {
            return null;
        }

        return new SystemLog
        {
            Id = entity.Id,
            ResourceType = entity.ResourceType,
            Event = entity.Event,
            ChangeSet = entity.ChangeSet,
            Comment = entity.Comment,
        };
    }
}
