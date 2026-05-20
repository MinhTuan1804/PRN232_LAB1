using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Mapping;
using PRN232.LMS.API.Models.Requests;
using PRN232.LMS.API.Models.Responses;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.API.Controllers;

[ApiController]
[Route("api/semesters")]
public class SemestersController(ICrudService<SemesterModel> service)
    : CrudController<SemesterModel, SemesterRequest, SemesterResponse>(service)
{
    protected override SemesterModel ToModel(SemesterRequest request) => request.ToModel();
    protected override SemesterResponse ToResponse(SemesterModel model) => model.ToResponse();
    protected override int GetId(SemesterResponse response) => response.SemesterId;
}
