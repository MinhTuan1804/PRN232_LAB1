using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.Services.Mapping;

public static class BusinessMapper
{
    public static SemesterModel ToModel(this Semester entity, bool includeCourses = true) => new()
    {
        SemesterId = entity.SemesterId,
        SemesterName = entity.SemesterName,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        Courses = includeCourses && entity.Courses.Count > 0
            ? entity.Courses.Select(x => x.ToModel(includeSemester: false, includeEnrollments: false)).ToList()
            : null
    };

    public static CourseModel ToModel(this Course entity, bool includeSemester = true, bool includeEnrollments = true) => new()
    {
        CourseId = entity.CourseId,
        CourseName = entity.CourseName,
        SemesterId = entity.SemesterId,
        Semester = includeSemester && entity.Semester is not null ? entity.Semester.ToModel(includeCourses: false) : null,
        Enrollments = includeEnrollments && entity.Enrollments.Count > 0
            ? entity.Enrollments.Select(x => x.ToModel(includeStudent: true, includeCourse: false)).ToList()
            : null
    };

    public static SubjectModel ToModel(this Subject entity) => new()
    {
        SubjectId = entity.SubjectId,
        SubjectCode = entity.SubjectCode,
        SubjectName = entity.SubjectName,
        Credit = entity.Credit
    };

    public static StudentModel ToModel(this Student entity, bool includeEnrollments = true) => new()
    {
        StudentId = entity.StudentId,
        FullName = entity.FullName,
        Email = entity.Email,
        DateOfBirth = entity.DateOfBirth,
        Enrollments = includeEnrollments && entity.Enrollments.Count > 0
            ? entity.Enrollments.Select(x => x.ToModel(includeStudent: false, includeCourse: true)).ToList()
            : null
    };

    public static EnrollmentModel ToModel(this Enrollment entity, bool includeStudent = true, bool includeCourse = true) => new()
    {
        EnrollmentId = entity.EnrollmentId,
        StudentId = entity.StudentId,
        CourseId = entity.CourseId,
        EnrollDate = entity.EnrollDate,
        Status = entity.Status,
        Student = includeStudent && entity.Student is not null ? entity.Student.ToModel(includeEnrollments: false) : null,
        Course = includeCourse && entity.Course is not null ? entity.Course.ToModel(includeSemester: true, includeEnrollments: false) : null
    };
}
