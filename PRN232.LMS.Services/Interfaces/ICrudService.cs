using PRN232.LMS.Services.Models;

namespace PRN232.LMS.Services.Interfaces;

public interface ICrudService<TModel>
{
    Task<PagedBusinessResult<TModel>> GetAsync(ListQueryModel query, CancellationToken cancellationToken = default);
    Task<TModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TModel> CreateAsync(TModel model, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, TModel model, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
