using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Querying;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.Services.Services;

public abstract class CrudService<TEntity, TModel>(IRepository<TEntity> repository) : ICrudService<TModel>
    where TEntity : class
{
    public async Task<PagedBusinessResult<TModel>> GetAsync(ListQueryModel query, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new QueryOptions
        {
            Search = query.Search,
            Sort = query.Sort,
            Page = query.Page,
            PageSize = query.PageSize,
            Expand = query.Expand
        }, cancellationToken);

        return new PagedBusinessResult<TModel>
        {
            Items = result.Items.Select(ToModel).ToList(),
            Pagination = new PaginationModel
            {
                Page = result.Page,
                PageSize = result.PageSize,
                TotalItems = result.TotalItems,
                TotalPages = result.TotalPages
            }
        };
    }

    public async Task<TModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetByIdAsync(id, includeDetails: true, cancellationToken);
        return entity is null ? default : ToModel(entity);
    }

    public async Task<TModel> CreateAsync(TModel model, CancellationToken cancellationToken = default)
    {
        var created = await repository.AddAsync(ToEntity(model), cancellationToken);
        return ToModel(created);
    }

    public async Task<bool> UpdateAsync(int id, TModel model, CancellationToken cancellationToken = default)
    {
        var existing = await repository.GetByIdAsync(id, includeDetails: false, cancellationToken);
        if (existing is null)
        {
            return false;
        }

        CopyForUpdate(existing, model);
        return await repository.UpdateAsync(existing, cancellationToken);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default) =>
        repository.DeleteAsync(id, cancellationToken);

    protected abstract TModel ToModel(TEntity entity);
    protected abstract TEntity ToEntity(TModel model);
    protected abstract void CopyForUpdate(TEntity entity, TModel model);
}
