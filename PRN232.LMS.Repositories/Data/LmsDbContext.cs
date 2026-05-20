using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Data;

public class LmsDbContext(DbContextOptions<LmsDbContext> options) : DbContext(options)
{
    public DbSet<Semester> Semesters => Set<Semester>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Semester>(entity =>
        {
            entity.ToTable("Semester");
            entity.HasKey(x => x.SemesterId);
            entity.Property(x => x.SemesterName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.StartDate).HasColumnType("datetime");
            entity.Property(x => x.EndDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("Course");
            entity.HasKey(x => x.CourseId);
            entity.Property(x => x.CourseName).HasMaxLength(100).IsRequired();
            entity.HasOne(x => x.Semester).WithMany(x => x.Courses).HasForeignKey(x => x.SemesterId);
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.ToTable("Subject");
            entity.HasKey(x => x.SubjectId);
            entity.Property(x => x.SubjectCode).HasMaxLength(20).IsUnicode(false).IsRequired();
            entity.Property(x => x.SubjectName).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Student");
            entity.HasKey(x => x.StudentId);
            entity.Property(x => x.FullName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(100).IsUnicode(false).IsRequired();
            entity.Property(x => x.DateOfBirth).HasColumnType("datetime");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.ToTable("Enrollment");
            entity.HasKey(x => x.EnrollmentId);
            entity.Property(x => x.EnrollDate).HasColumnType("datetime");
            entity.Property(x => x.Status).HasMaxLength(20).IsUnicode(false).IsRequired();
            entity.HasOne(x => x.Student).WithMany(x => x.Enrollments).HasForeignKey(x => x.StudentId);
            entity.HasOne(x => x.Course).WithMany(x => x.Enrollments).HasForeignKey(x => x.CourseId);
        });

        Seed(modelBuilder);
    }

    private static void Seed(ModelBuilder modelBuilder)
    {
        var semesters = Enumerable.Range(1, 5)
            .Select(i => new Semester
            {
                SemesterId = i,
                SemesterName = $"Semester {i}",
                StartDate = new DateTime(2024 + (i - 1) / 3, ((i - 1) % 3) * 4 + 1, 1),
                EndDate = new DateTime(2024 + (i - 1) / 3, ((i - 1) % 3) * 4 + 4, 28)
            })
            .ToArray();

        var subjects = new[]
        {
            new Subject { SubjectId = 1, SubjectCode = "PRN232", SubjectName = "Advanced Cross-Platform Application Programming", Credit = 3 },
            new Subject { SubjectId = 2, SubjectCode = "DBI202", SubjectName = "Database Systems", Credit = 3 },
            new Subject { SubjectId = 3, SubjectCode = "SWE201", SubjectName = "Software Engineering", Credit = 3 },
            new Subject { SubjectId = 4, SubjectCode = "PRO192", SubjectName = "Object-Oriented Programming", Credit = 3 },
            new Subject { SubjectId = 5, SubjectCode = "CSD201", SubjectName = "Data Structures and Algorithms", Credit = 3 },
            new Subject { SubjectId = 6, SubjectCode = "MAS291", SubjectName = "Statistics and Probability", Credit = 3 },
            new Subject { SubjectId = 7, SubjectCode = "OSG202", SubjectName = "Operating Systems", Credit = 3 },
            new Subject { SubjectId = 8, SubjectCode = "NWC203", SubjectName = "Computer Networking", Credit = 3 },
            new Subject { SubjectId = 9, SubjectCode = "SWP391", SubjectName = "Application Development Project", Credit = 4 },
            new Subject { SubjectId = 10, SubjectCode = "ITE302", SubjectName = "Ethics in IT", Credit = 2 }
        };

        var courses = Enumerable.Range(1, 20)
            .Select(i => new Course
            {
                CourseId = i,
                CourseName = $"{subjects[(i - 1) % subjects.Length].SubjectCode} Course {i:00}",
                SemesterId = ((i - 1) % semesters.Length) + 1
            })
            .ToArray();

        var familyNames = new[] { "Nguyen", "Tran", "Le", "Pham", "Hoang", "Phan", "Vu", "Dang", "Bui", "Do" };
        var givenNames = new[] { "An", "Binh", "Chi", "Dung", "Giang", "Hanh", "Khoa", "Linh", "Minh", "Quang" };
        var students = Enumerable.Range(1, 50)
            .Select(i => new Student
            {
                StudentId = i,
                FullName = $"{familyNames[(i - 1) % familyNames.Length]} {givenNames[(i - 1) % givenNames.Length]} {i:00}",
                Email = $"student{i:00}@lms.local",
                DateOfBirth = new DateTime(2000 + (i % 5), ((i - 1) % 12) + 1, ((i - 1) % 27) + 1)
            })
            .ToArray();

        var statuses = new[] { "Active", "Completed", "Dropped", "Pending" };
        var enrollments = Enumerable.Range(1, 500)
            .Select(i => new Enrollment
            {
                EnrollmentId = i,
                StudentId = ((i - 1) % students.Length) + 1,
                CourseId = ((i - 1) % courses.Length) + 1,
                EnrollDate = new DateTime(2024, 1, 1).AddDays(i % 365),
                Status = statuses[(i - 1) % statuses.Length]
            })
            .ToArray();

        modelBuilder.Entity<Semester>().HasData(semesters);
        modelBuilder.Entity<Subject>().HasData(subjects);
        modelBuilder.Entity<Course>().HasData(courses);
        modelBuilder.Entity<Student>().HasData(students);
        modelBuilder.Entity<Enrollment>().HasData(enrollments);
    }
}
