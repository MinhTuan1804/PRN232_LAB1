using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Mapping;
using PRN232.LMS.API.Models.Requests;
using PRN232.LMS.API.Models.Responses;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.API.Controllers;

[ApiController]
[Route("api/enrollments")]
public class EnrollmentsController(ICrudService<EnrollmentModel> service)
    : CrudController<EnrollmentModel, EnrollmentRequest, EnrollmentResponse>(service)
{
    protected override EnrollmentModel ToModel(EnrollmentRequest request) => request.ToModel();
    protected override EnrollmentResponse ToResponse(EnrollmentModel model) => model.ToResponse();
    protected override int GetId(EnrollmentResponse response) => response.EnrollmentId;
}
