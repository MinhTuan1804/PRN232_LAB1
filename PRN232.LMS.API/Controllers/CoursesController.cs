using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Mapping;
using PRN232.LMS.API.Models.Requests;
using PRN232.LMS.API.Models.Responses;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.API.Controllers;

[ApiController]
[Route("api/courses")]
public class CoursesController(ICrudService<CourseModel> service)
    : CrudController<CourseModel, CourseRequest, CourseResponse>(service)
{
    protected override CourseModel ToModel(CourseRequest request) => request.ToModel();
    protected override CourseResponse ToResponse(CourseModel model) => model.ToResponse();
    protected override int GetId(CourseResponse response) => response.CourseId;
}
