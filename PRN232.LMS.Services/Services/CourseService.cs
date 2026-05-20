using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.Mapping;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.Services.Services;

public class CourseService(IRepository<Course> repository) : CrudService<Course, CourseModel>(repository)
{
    protected override CourseModel ToModel(Course entity) => entity.ToModel();

    protected override Course ToEntity(CourseModel model) => new()
    {
        CourseName = model.CourseName,
        SemesterId = model.SemesterId
    };

    protected override void CopyForUpdate(Course entity, CourseModel model)
    {
        entity.CourseName = model.CourseName;
        entity.SemesterId = model.SemesterId;
    }
}
