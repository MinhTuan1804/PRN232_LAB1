using PRN232.LMS.API.Models.Requests;
using PRN232.LMS.API.Models.Responses;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.API.Mapping;

public static class ApiMapper
{
    public static SemesterModel ToModel(this SemesterRequest request) => new()
    {
        SemesterName = request.SemesterName,
        StartDate = request.StartDate,
        EndDate = request.EndDate
    };

    public static CourseModel ToModel(this CourseRequest request) => new()
    {
        CourseName = request.CourseName,
        SemesterId = request.SemesterId
    };

    public static SubjectModel ToModel(this SubjectRequest request) => new()
    {
        SubjectCode = request.SubjectCode,
        SubjectName = request.SubjectName,
        Credit = request.Credit
    };

    public static StudentModel ToModel(this StudentRequest request) => new()
    {
        FullName = request.FullName,
        Email = request.Email,
        DateOfBirth = request.DateOfBirth
    };

    public static EnrollmentModel ToModel(this EnrollmentRequest request) => new()
    {
        StudentId = request.StudentId,
        CourseId = request.CourseId,
        EnrollDate = request.EnrollDate,
        Status = request.Status
    };

    public static SemesterResponse ToResponse(this SemesterModel model, bool includeCourses = true) => new()
    {
        SemesterId = model.SemesterId,
        SemesterName = model.SemesterName,
        StartDate = model.StartDate,
        EndDate = model.EndDate,
        Courses = includeCourses && model.Courses is not null
            ? model.Courses.Select(x => x.ToResponse(includeSemester: false, includeEnrollments: false)).ToList()
            : null
    };

    public static CourseResponse ToResponse(this CourseModel model, bool includeSemester = true, bool includeEnrollments = true) => new()
    {
        CourseId = model.CourseId,
        CourseName = model.CourseName,
        SemesterId = model.SemesterId,
        Semester = includeSemester && model.Semester is not null ? model.Semester.ToResponse(includeCourses: false) : null,
        Enrollments = includeEnrollments && model.Enrollments is not null
            ? model.Enrollments.Select(x => x.ToResponse(includeStudent: true, includeCourse: false)).ToList()
            : null
    };

    public static SubjectResponse ToResponse(this SubjectModel model) => new()
    {
        SubjectId = model.SubjectId,
        SubjectCode = model.SubjectCode,
        SubjectName = model.SubjectName,
        Credit = model.Credit
    };

    public static StudentResponse ToResponse(this StudentModel model, bool includeEnrollments = true) => new()
    {
        StudentId = model.StudentId,
        FullName = model.FullName,
        Email = model.Email,
        DateOfBirth = model.DateOfBirth,
        Enrollments = includeEnrollments && model.Enrollments is not null
            ? model.Enrollments.Select(x => x.ToResponse(includeStudent: false, includeCourse: true)).ToList()
            : null
    };

    public static EnrollmentResponse ToResponse(this EnrollmentModel model, bool includeStudent = true, bool includeCourse = true) => new()
    {
        EnrollmentId = model.EnrollmentId,
        StudentId = model.StudentId,
        CourseId = model.CourseId,
        EnrollDate = model.EnrollDate,
        Status = model.Status,
        Student = includeStudent && model.Student is not null ? model.Student.ToResponse(includeEnrollments: false) : null,
        Course = includeCourse && model.Course is not null ? model.Course.ToResponse(includeSemester: true, includeEnrollments: false) : null
    };
}
