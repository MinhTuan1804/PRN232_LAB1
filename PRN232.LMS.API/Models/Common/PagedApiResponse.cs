using PRN232.LMS.Services.Models;

namespace PRN232.LMS.API.Models.Common;

public class PagedApiResponse<T> : ApiResponse<T>
{
    public PaginationModel Pagination { get; set; } = new();
}
