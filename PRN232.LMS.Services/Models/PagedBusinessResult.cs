namespace PRN232.LMS.Services.Models;

public sealed class PagedBusinessResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];
    public PaginationModel Pagination { get; init; } = new();
}
