using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Querying;

namespace PRN232.LMS.Repositories.Repositories;

public abstract class RepositoryBase<T>(LmsDbContext context) : IRepository<T> where T : class
{
    protected readonly LmsDbContext Context = context;

    public async Task<PagedResult<T>> GetAsync(QueryOptions options, CancellationToken cancellationToken = default)
    {
        var page = Math.Max(options.Page, 1);
        var pageSize = Math.Clamp(options.PageSize, 1, 100);
        var query = ApplySort(ApplySearch(ApplyIncludes(Context.Set<T>().AsNoTracking(), options), options), options);
        var totalItems = await query.CountAsync(cancellationToken);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new PagedResult<T>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems
        };
    }

    public virtual Task<T?> GetByIdAsync(int id, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return Context.Set<T>().FindAsync([id], cancellationToken).AsTask();
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        Context.Set<T>().Add(entity);
        await Context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        Context.Set<T>().Update(entity);
        return await Context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await Context.Set<T>().FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return false;
        }

        Context.Set<T>().Remove(entity);
        await Context.SaveChangesAsync(cancellationToken);
        return true;
    }

    protected virtual IQueryable<T> ApplyIncludes(IQueryable<T> query, QueryOptions options) => query;
    protected abstract IQueryable<T> ApplySearch(IQueryable<T> query, QueryOptions options);
    protected abstract IQueryable<T> ApplySort(IQueryable<T> query, QueryOptions options);

    protected static IQueryable<TItem> ApplyOrdering<TItem, TKey>(
        IQueryable<TItem> query,
        Expression<Func<TItem, TKey>> keySelector,
        bool descending,
        ref bool hasOrdering)
    {
        if (!hasOrdering)
        {
            hasOrdering = true;
            return descending ? query.OrderByDescending(keySelector) : query.OrderBy(keySelector);
        }

        var ordered = (IOrderedQueryable<TItem>)query;
        return descending ? ordered.ThenByDescending(keySelector) : ordered.ThenBy(keySelector);
    }
}
