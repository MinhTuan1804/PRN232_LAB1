using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.Mapping;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.Services.Services;

public class SemesterService(IRepository<Semester> repository) : CrudService<Semester, SemesterModel>(repository)
{
    protected override SemesterModel ToModel(Semester entity) => entity.ToModel();

    protected override Semester ToEntity(SemesterModel model) => new()
    {
        SemesterName = model.SemesterName,
        StartDate = model.StartDate,
        EndDate = model.EndDate
    };

    protected override void CopyForUpdate(Semester entity, SemesterModel model)
    {
        entity.SemesterName = model.SemesterName;
        entity.StartDate = model.StartDate;
        entity.EndDate = model.EndDate;
    }
}
