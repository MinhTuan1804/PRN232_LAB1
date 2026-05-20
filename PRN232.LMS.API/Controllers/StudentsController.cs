using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Mapping;
using PRN232.LMS.API.Models.Requests;
using PRN232.LMS.API.Models.Responses;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.API.Controllers;

[ApiController]
[Route("api/students")]
public class StudentsController(ICrudService<StudentModel> service)
    : CrudController<StudentModel, StudentRequest, StudentResponse>(service)
{
    protected override StudentModel ToModel(StudentRequest request) => request.ToModel();
    protected override StudentResponse ToResponse(StudentModel model) => model.ToResponse();
    protected override int GetId(StudentResponse response) => response.StudentId;
}
