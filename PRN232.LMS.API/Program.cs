using Microsoft.EntityFrameworkCore;
using PRN232.LMS.API.Models.Common;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Repositories;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models;
using PRN232.LMS.Services.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "PRN232 LMS REST API",
        Version = "v1",
        Description = "Learning Management System API for LAB 1 - PRN232"
    });
});

builder.Services.AddDbContext<LmsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRepository<Semester>, SemesterRepository>();
builder.Services.AddScoped<IRepository<Course>, CourseRepository>();
builder.Services.AddScoped<IRepository<Subject>, SubjectRepository>();
builder.Services.AddScoped<IRepository<Student>, StudentRepository>();
builder.Services.AddScoped<IRepository<Enrollment>, EnrollmentRepository>();

builder.Services.AddScoped<ICrudService<SemesterModel>, SemesterService>();
builder.Services.AddScoped<ICrudService<CourseModel>, CourseService>();
builder.Services.AddScoped<ICrudService<SubjectModel>, SubjectService>();
builder.Services.AddScoped<ICrudService<StudentModel>, StudentService>();
builder.Services.AddScoped<ICrudService<EnrollmentModel>, EnrollmentService>();

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new ApiResponse<object>
        {
            Success = false,
            Message = "Internal server error",
            Data = null,
            Errors = new[] { "An unexpected error occurred while processing the request." }
        });
    });
});

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<LmsDbContext>();
    dbContext.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "PRN232 LMS REST API v1");
});

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
