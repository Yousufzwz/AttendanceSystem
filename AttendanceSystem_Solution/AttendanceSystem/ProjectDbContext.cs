using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem;

public class ProjectDbContext : DbContext
{
    private readonly string _connectionString;

    public ProjectDbContext()
    {
        _connectionString = "Server =.\\SQLEXPRESS;Database=AttendanceSystem;User Id=attendancesystem;Password=9876;TrustServerCertificate=True";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CourseTeacher>().HasKey(x => new {x.CourseId, x.TeacherId});

        modelBuilder.Entity<CourseTeacher>() 
                .HasOne(x => x.Course)
                .WithMany(x => x.AssignedTeachers)
                .HasForeignKey(x => x.CourseId);

        modelBuilder.Entity<CourseTeacher>() 
                .HasOne(x => x.Teacher)
                .WithMany(x => x.AssignedCourses)
                .HasForeignKey(x => x.TeacherId);

        modelBuilder.Entity<CourseStudent>().HasKey(x => new { x.CourseId, x.StudentId });

        modelBuilder.Entity<CourseStudent>() 
                .HasOne(x => x.Course)
                .WithMany(x => x.StudentsEnrolled)
                .HasForeignKey(x => x.CourseId);

        modelBuilder.Entity<CourseStudent>() 
                .HasOne(x => x.Student)
                .WithMany(x => x.EnrolledCourses)
                .HasForeignKey(x => x.StudentId);
            

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Course> Courses { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<CourseTeacher> CourseTeachers { get;set; }
    public DbSet<CourseStudent> CourseStudents { get;set; }
    public DbSet<ClassSchedule> ClassSchedules { get; set; }
    public DbSet<AttendanceRecord> AttendanceRecords { get; set; }




}
