using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Querying;

namespace PRN232.LMS.Repositories.Repositories;

public class EnrollmentRepository(LmsDbContext context) : RepositoryBase<Enrollment>(context)
{
    public override Task<Enrollment?> GetByIdAsync(int id, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = Context.Enrollments.AsNoTracking();
        if (includeDetails)
        {
            query = query.Include(x => x.Student).Include(x => x.Course).ThenInclude(x => x!.Semester);
        }

        return query.FirstOrDefaultAsync(x => x.EnrollmentId == id, cancellationToken);
    }

    protected override IQueryable<Enrollment> ApplyIncludes(IQueryable<Enrollment> query, QueryOptions options)
    {
        foreach (var expand in options.ExpandItems)
        {
            query = expand.ToLowerInvariant() switch
            {
                "student" => query.Include(x => x.Student),
                "course" => query.Include(x => x.Course).ThenInclude(x => x!.Semester),
                _ => query
            };
        }

        return query;
    }

    protected override IQueryable<Enrollment> ApplySearch(IQueryable<Enrollment> query, QueryOptions options) =>
        string.IsNullOrWhiteSpace(options.Search)
            ? query
            : query.Where(x =>
                x.Status.Contains(options.Search) ||
                x.StudentId.ToString() == options.Search ||
                x.CourseId.ToString() == options.Search);

    protected override IQueryable<Enrollment> ApplySort(IQueryable<Enrollment> query, QueryOptions options)
    {
        var hasOrdering = false;
        foreach (var sort in options.SortItems)
        {
            var descending = sort.StartsWith('-');
            query = sort.TrimStart('-').ToLowerInvariant() switch
            {
                "enrolldate" => ApplyOrdering(query, x => x.EnrollDate, descending, ref hasOrdering),
                "status" => ApplyOrdering(query, x => x.Status, descending, ref hasOrdering),
                "studentid" => ApplyOrdering(query, x => x.StudentId, descending, ref hasOrdering),
                "courseid" => ApplyOrdering(query, x => x.CourseId, descending, ref hasOrdering),
                _ => query
            };
        }

        return hasOrdering ? query : query.OrderBy(x => x.EnrollmentId);
    }
}
