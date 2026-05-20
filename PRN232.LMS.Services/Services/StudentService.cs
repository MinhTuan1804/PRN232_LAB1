using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.Mapping;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.Services.Services;

public class StudentService(IRepository<Student> repository) : CrudService<Student, StudentModel>(repository)
{
    protected override StudentModel ToModel(Student entity) => entity.ToModel();

    protected override Student ToEntity(StudentModel model) => new()
    {
        FullName = model.FullName,
        Email = model.Email,
        DateOfBirth = model.DateOfBirth
    };

    protected override void CopyForUpdate(Student entity, StudentModel model)
    {
        entity.FullName = model.FullName;
        entity.Email = model.Email;
        entity.DateOfBirth = model.DateOfBirth;
    }
}
