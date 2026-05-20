namespace PRN232.LMS.Repositories.Querying;

public sealed class QueryOptions
{
    public string? Search { get; set; }
    public string? Sort { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Expand { get; set; }

    public IEnumerable<string> ExpandItems => Split(Expand);
    public IEnumerable<string> SortItems => Split(Sort);

    private static IEnumerable<string> Split(string? value) =>
        string.IsNullOrWhiteSpace(value)
            ? Enumerable.Empty<string>()
            : value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}
