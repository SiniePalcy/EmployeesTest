using AutoMapper;
using Employees.Data.Entities;
using Employees.Domain.Model;
using Employees.Infrastructure.Exceptions;
using Employees.Shared.Enums;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Employees.Data.Repositories.Impl;

internal abstract class BaseRepository<TModel, TEntity> : IRepository<TModel, TEntity>
    where TModel : BaseObject
    where TEntity : BaseEntity
{
    protected readonly DataContext _context;
    protected readonly ResourceType _resourceType;
    protected readonly IMapper _mapper;

    public BaseRepository(
        DataContext dataContext,
        IMapper mapper,
        ResourceType resourceType)
    {
        _context = dataContext;
        _mapper = mapper;
        _resourceType = resourceType;
    }

    public abstract Task<IEnumerable<TModel>> GetAsync(List<int>? ids = null);

    public virtual async Task<SystemLog> AddAsync(TModel model)
    {
        var entity = _mapper.Map<TEntity>(model) 
            ?? throw new NullReferenceException($"Entity '{typeof(TEntity)}' is null");
        var entry = await _context.AddAsync(entity);

        await _context.SaveChangesAsync();

        entry.State = Microsoft.EntityFrameworkCore.EntityState.Detached;

        var result = await LogEntry(entry, EventType.Create);
        return result;
    }

    public virtual async Task<SystemLog> UpdateAsync(TModel model)
    {
        var entity = _mapper.Map<TEntity>(model) 
            ?? throw new NullReferenceException($"Entity '{typeof(TEntity)}' is null");
        _ = await _context.FindAsync<TEntity>(model.Id)
            ?? throw new ObjectNotFoundException(model.Id, typeof(TEntity));
        var entry = _context.Update(entity);
        var result = await LogEntry(entry, EventType.Update);
        return result;
    }


    public virtual async Task DeleteAsync(int id)
    {
        var entity = await _context.FindAsync<TEntity>(id) 
            ?? throw new ObjectNotFoundException(id, typeof(TEntity));
        entity.IsDeleted = true;
        _context.Update(entity);
    }

    public virtual Task SaveAsync() => _context.SaveChangesAsync();

    protected virtual async Task<SystemLog> LogEntry(EntityEntry<TEntity> entry, EventType eventType)
    {
        SystemLogEntity logEntity = new()
        {
            ResourceType = _resourceType,
            Event = eventType,
        };

        if (eventType == EventType.Create)
        {
            logEntity.ChangeSet = entry.Properties.ToDictionary(x => x.Metadata.Name, x => x.CurrentValue!.ToString())!;
        }
        else if (eventType == EventType.Update)
        {
            logEntity.ChangeSet = entry
                .Properties
                .Where(x => x.OriginalValue != null && x.OriginalValue.Equals(x.CurrentValue))
                .ToDictionary(x => x.Metadata.Name, x => x.CurrentValue!.ToString())!;
        }

        var logEntry = await _context.AddAsync(logEntity);

        await _context.SaveChangesAsync();

        return _mapper.Map<SystemLog>(logEntry.Entity)!;
    }


}
