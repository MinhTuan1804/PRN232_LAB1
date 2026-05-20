using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Querying;

namespace PRN232.LMS.Repositories.Repositories;

public class SubjectRepository(LmsDbContext context) : RepositoryBase<Subject>(context)
{
    protected override IQueryable<Subject> ApplySearch(IQueryable<Subject> query, QueryOptions options) =>
        string.IsNullOrWhiteSpace(options.Search)
            ? query
            : query.Where(x => x.SubjectCode.Contains(options.Search) || x.SubjectName.Contains(options.Search));

    protected override IQueryable<Subject> ApplySort(IQueryable<Subject> query, QueryOptions options)
    {
        var hasOrdering = false;
        foreach (var sort in options.SortItems)
        {
            var descending = sort.StartsWith('-');
            query = sort.TrimStart('-').ToLowerInvariant() switch
            {
                "subjectcode" => ApplyOrdering(query, x => x.SubjectCode, descending, ref hasOrdering),
                "subjectname" => ApplyOrdering(query, x => x.SubjectName, descending, ref hasOrdering),
                "credit" => ApplyOrdering(query, x => x.Credit, descending, ref hasOrdering),
                _ => query
            };
        }

        return hasOrdering ? query : query.OrderBy(x => x.SubjectId);
    }
}
