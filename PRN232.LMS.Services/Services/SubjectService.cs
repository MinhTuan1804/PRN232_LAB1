using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.Mapping;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.Services.Services;

public class SubjectService(IRepository<Subject> repository) : CrudService<Subject, SubjectModel>(repository)
{
    protected override SubjectModel ToModel(Subject entity) => entity.ToModel();

    protected override Subject ToEntity(SubjectModel model) => new()
    {
        SubjectCode = model.SubjectCode,
        SubjectName = model.SubjectName,
        Credit = model.Credit
    };

    protected override void CopyForUpdate(Subject entity, SubjectModel model)
    {
        entity.SubjectCode = model.SubjectCode;
        entity.SubjectName = model.SubjectName;
        entity.Credit = model.Credit;
    }
}
