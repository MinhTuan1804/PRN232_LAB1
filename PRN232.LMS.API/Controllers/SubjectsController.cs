using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Mapping;
using PRN232.LMS.API.Models.Requests;
using PRN232.LMS.API.Models.Responses;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.API.Controllers;

[ApiController]
[Route("api/subjects")]
public class SubjectsController(ICrudService<SubjectModel> service)
    : CrudController<SubjectModel, SubjectRequest, SubjectResponse>(service)
{
    protected override SubjectModel ToModel(SubjectRequest request) => request.ToModel();
    protected override SubjectResponse ToResponse(SubjectModel model) => model.ToResponse();
    protected override int GetId(SubjectResponse response) => response.SubjectId;
}
