using PRN232.LMS.Services.Models;

namespace PRN232.LMS.API.Models.Common;

public class ListQueryRequest
{
    public string? Search { get; set; }
    public string? Sort { get; set; }
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string? Fields { get; set; }
    public string? Expand { get; set; }

    public ListQueryModel ToBusinessQuery() => new()
    {
        Search = Search,
        Sort = Sort,
        Page = Page,
        PageSize = Size,
        Fields = Fields,
        Expand = Expand
    };
}
