using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Querying;

namespace PRN232.LMS.Repositories.Repositories;

public class CourseRepository(LmsDbContext context) : RepositoryBase<Course>(context)
{
    public override Task<Course?> GetByIdAsync(int id, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = Context.Courses.AsNoTracking();
        if (includeDetails)
        {
            query = query.Include(x => x.Semester).Include(x => x.Enrollments).ThenInclude(x => x.Student);
        }

        return query.FirstOrDefaultAsync(x => x.CourseId == id, cancellationToken);
    }

    protected override IQueryable<Course> ApplyIncludes(IQueryable<Course> query, QueryOptions options)
    {
        foreach (var expand in options.ExpandItems)
        {
            query = expand.ToLowerInvariant() switch
            {
                "semester" => query.Include(x => x.Semester),
                "enrollments" => query.Include(x => x.Enrollments),
                _ => query
            };
        }

        return query;
    }

    protected override IQueryable<Course> ApplySearch(IQueryable<Course> query, QueryOptions options) =>
        string.IsNullOrWhiteSpace(options.Search)
            ? query
            : query.Where(x => x.CourseName.Contains(options.Search) || x.SemesterId.ToString() == options.Search);

    protected override IQueryable<Course> ApplySort(IQueryable<Course> query, QueryOptions options)
    {
        var hasOrdering = false;
        foreach (var sort in options.SortItems)
        {
            var descending = sort.StartsWith('-');
            query = sort.TrimStart('-').ToLowerInvariant() switch
            {
                "coursename" => ApplyOrdering(query, x => x.CourseName, descending, ref hasOrdering),
                "semesterid" => ApplyOrdering(query, x => x.SemesterId, descending, ref hasOrdering),
                _ => query
            };
        }

        return hasOrdering ? query : query.OrderBy(x => x.CourseId);
    }
}
