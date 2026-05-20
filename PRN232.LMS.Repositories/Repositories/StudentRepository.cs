using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Querying;

namespace PRN232.LMS.Repositories.Repositories;

public class StudentRepository(LmsDbContext context) : RepositoryBase<Student>(context)
{
    public override Task<Student?> GetByIdAsync(int id, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = Context.Students.AsNoTracking();
        if (includeDetails)
        {
            query = query.Include(x => x.Enrollments).ThenInclude(x => x.Course).ThenInclude(x => x!.Semester);
        }

        return query.FirstOrDefaultAsync(x => x.StudentId == id, cancellationToken);
    }

    protected override IQueryable<Student> ApplyIncludes(IQueryable<Student> query, QueryOptions options) =>
        options.ExpandItems.Contains("enrollments", StringComparer.OrdinalIgnoreCase)
            ? query.Include(x => x.Enrollments).ThenInclude(x => x.Course)
            : query;

    protected override IQueryable<Student> ApplySearch(IQueryable<Student> query, QueryOptions options) =>
        string.IsNullOrWhiteSpace(options.Search)
            ? query
            : query.Where(x => x.FullName.Contains(options.Search) || x.Email.Contains(options.Search));

    protected override IQueryable<Student> ApplySort(IQueryable<Student> query, QueryOptions options)
    {
        var hasOrdering = false;
        foreach (var sort in options.SortItems)
        {
            var descending = sort.StartsWith('-');
            query = sort.TrimStart('-').ToLowerInvariant() switch
            {
                "fullname" => ApplyOrdering(query, x => x.FullName, descending, ref hasOrdering),
                "email" => ApplyOrdering(query, x => x.Email, descending, ref hasOrdering),
                "dateofbirth" => ApplyOrdering(query, x => x.DateOfBirth, descending, ref hasOrdering),
                _ => query
            };
        }

        return hasOrdering ? query : query.OrderBy(x => x.StudentId);
    }
}
