using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.Mapping;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.Services.Services;

public class EnrollmentService(IRepository<Enrollment> repository) : CrudService<Enrollment, EnrollmentModel>(repository)
{
    protected override EnrollmentModel ToModel(Enrollment entity) => entity.ToModel();

    protected override Enrollment ToEntity(EnrollmentModel model) => new()
    {
        StudentId = model.StudentId,
        CourseId = model.CourseId,
        EnrollDate = model.EnrollDate,
        Status = model.Status
    };

    protected override void CopyForUpdate(Enrollment entity, EnrollmentModel model)
    {
        entity.StudentId = model.StudentId;
        entity.CourseId = model.CourseId;
        entity.EnrollDate = model.EnrollDate;
        entity.Status = model.Status;
    }
}
