using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Querying;

namespace PRN232.LMS.Repositories.Repositories;

public class SemesterRepository(LmsDbContext context) : RepositoryBase<Semester>(context)
{
    public override Task<Semester?> GetByIdAsync(int id, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = Context.Semesters.AsNoTracking();
        if (includeDetails)
        {
            query = query.Include(x => x.Courses);
        }

        return query.FirstOrDefaultAsync(x => x.SemesterId == id, cancellationToken);
    }

    protected override IQueryable<Semester> ApplyIncludes(IQueryable<Semester> query, QueryOptions options) =>
        options.ExpandItems.Contains("courses", StringComparer.OrdinalIgnoreCase) ? query.Include(x => x.Courses) : query;

    protected override IQueryable<Semester> ApplySearch(IQueryable<Semester> query, QueryOptions options) =>
        string.IsNullOrWhiteSpace(options.Search)
            ? query
            : query.Where(x => x.SemesterName.Contains(options.Search));

    protected override IQueryable<Semester> ApplySort(IQueryable<Semester> query, QueryOptions options)
    {
        var hasOrdering = false;
        foreach (var sort in options.SortItems)
        {
            var descending = sort.StartsWith('-');
            query = sort.TrimStart('-').ToLowerInvariant() switch
            {
                "semestername" => ApplyOrdering(query, x => x.SemesterName, descending, ref hasOrdering),
                "startdate" => ApplyOrdering(query, x => x.StartDate, descending, ref hasOrdering),
                "enddate" => ApplyOrdering(query, x => x.EndDate, descending, ref hasOrdering),
                _ => query
            };
        }

        return hasOrdering ? query : query.OrderBy(x => x.SemesterId);
    }
}
