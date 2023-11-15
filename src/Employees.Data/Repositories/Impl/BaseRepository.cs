using Employees.Data.Entities;
using Employees.Data.Mapping;
using Employees.Domain.Model;
using Employees.Infrastructure.Exceptions;
using Employees.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Employees.Data.Repositories.Impl;

internal abstract class BaseRepository<TModel, TEntity> : IRepository<TModel, TEntity>
    where TModel : BaseObject
    where TEntity : BaseEntity
{
    protected readonly DataContext _context;
    protected readonly IMapper<TModel, TEntity> _mapper;
    protected readonly IMapper<SystemLog, SystemLogEntity> _logEntityMapper;
    protected readonly ResourceType _resourceType;

    public BaseRepository(
        DataContext dataContext,
        IMapper<TModel, TEntity> mapper,
        IMapper<SystemLog, SystemLogEntity> logEntityMapper,
        ResourceType resourceType)
    {
        _context = dataContext;
        _mapper = mapper;
        _logEntityMapper = logEntityMapper;
        _resourceType = resourceType;
    }

    public abstract Task<IEnumerable<TModel>> GetAsync(List<int>? ids = null);

    public virtual async Task<SystemLog> AddAsync(TModel model)
    {
        var entity = _mapper.ToEntity(model);
        if (entity is null)
        {
            throw new NullReferenceException($"Entity '{typeof(TEntity)}' is null");
        }

        var entry = await _context.AddAsync(entity);
        var result = await LogEntry(entry, EventType.Update);

        //await _context.SaveChangesAsync();

        return result;
    }

    public virtual async Task<SystemLog> UpdateAsync(TModel model)
    {
        var entity = _mapper.ToEntity(model);
        if (entity is null)
        {
            throw new NullReferenceException($"Entity '{typeof(TEntity)}' is null");
        }

        var existingEntity = await _context.FindAsync<TEntity>(model.Id);
        if (existingEntity is null)
        {
            throw new ObjectNotFoundException(model.Id, typeof(TEntity));
        }

        var entry = _context.Update(entity);
        var result = await LogEntry(entry, EventType.Update);
        
        //await _context.SaveChangesAsync();

        return result;
    }


    public virtual async Task DeleteAsync(int id)
    {
        var entity = await _context.FindAsync<TEntity>(id);
        if (entity is null)
        {
            throw new ObjectNotFoundException(id, typeof(TEntity));
        }

        entity.IsDeleted = true;
        _context.Update(entity);
        
        //awwait _context.SaveChangesAsync();
    }

    public virtual Task SaveAsync() => _context.SaveChangesAsync();

    protected virtual async Task<SystemLog> LogEntry(EntityEntry<TEntity> entry, EventType eventType)
    {
        SystemLogEntity logEntity = new()
        {
            ResourceType = _resourceType,
            Event = eventType,
        };

        if (entry.State == EntityState.Added)
        {
            logEntity.ChangeSet = entry.Properties.ToDictionary(x => x.Metadata.Name, x => x.CurrentValue)!;
        }
        else if (entry.State == EntityState.Modified)
        {
            logEntity.ChangeSet = entry
                .Properties
                .Where(x => x.OriginalValue != null && x.OriginalValue.Equals(x.CurrentValue))
                .ToDictionary(x => x.Metadata.Name, x => x.CurrentValue)!;
        }

        var logEntry = await _context.AddAsync(logEntity);

        return _logEntityMapper.ToModel(logEntry.Entity)!;
    }


}
