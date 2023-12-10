using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace TodoAPI.Repositories;

public interface IRepositoryBase<TEntity>
{
    TEntity Attach(TEntity entity);
    Task<TEntity> SaveAsync(TEntity entity);
    Task<TEntity?> FindByIdAsync(Guid id);
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> criteria);
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> criteria, string[] includes);
    Task<List<TEntity>> FindAllAsync();
    Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria);
    Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, string[] includes);
    TEntity Update(TEntity entity);
    void Delete(TEntity entity);
}

public class RepositoryBase<TEntity>(AppDbContext context) : IRepositoryBase<TEntity> where TEntity : class
{
    private readonly AppDbContext _context = context;

    public TEntity Attach(TEntity entity)
    {
        var entry = _context.Set<TEntity>().Attach(entity);
        return entry.Entity;
    }

    public async Task<TEntity> SaveAsync(TEntity entity)
    {
        var entry = await _context.Set<TEntity>().AddAsync(entity);
        return entry.Entity;
    }

    public async Task<TEntity?> FindByIdAsync(Guid id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> criteria)
    {
        return await _context.Set<TEntity>().FirstOrDefaultAsync(criteria);
    }

    public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> criteria, string[] includes)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(criteria);
    }

    public async Task<List<TEntity>> FindAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public async Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria)
    {
        return await _context.Set<TEntity>().Where(criteria).ToListAsync();
    }

    public async Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, string[] includes)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.Where(criteria).ToListAsync();
    }

    public TEntity Update(TEntity entity)
    {
        Attach(entity);

        _context.Set<TEntity>().Update(entity);
        return entity;
    }

    public void Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }
}