using PRN232.LMS.Repositories.Querying;

namespace PRN232.LMS.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    Task<PagedResult<T>> GetAsync(QueryOptions options, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(int id, bool includeDetails = false, CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
