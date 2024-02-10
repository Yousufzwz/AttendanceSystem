using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Password { get; set;}
    public List<CourseStudent> EnrolledCourses { get; set; }
    public List<AttendanceRecord> AttendanceList { get; set; }


    public void LoginAsStudent(string username, string password)
    {
        var context = new ProjectDbContext();
        var loginStudent = new ExecuteSystemOperation();

        var studentData = context.Students.FirstOrDefault(x => x.UserName.Equals(username));

        if (studentData != null)
        {
            if (username == studentData.UserName && password == studentData.Password)
            {
                Console.WriteLine();
                Console.WriteLine($"Hi, {studentData.Name}");
                Console.WriteLine();
                Console.WriteLine($"<<<< {studentData.Name}, you're in your Attendance Profile >>>>");
                Console.WriteLine();

                var isEnrolled = context.CourseStudents.FirstOrDefault(x => x.StudentId == studentData.Id);
                var coursesEnrolled = context.CourseStudents.Where(x => x.StudentId == studentData.Id).ToList();

                if (isEnrolled != null)
                {
                    Console.WriteLine("Enrolled Courses:");

                    foreach (var enrollment in coursesEnrolled)
                    {
                        var courseTitle = context.Courses
                            .Where(x => x.Id == enrollment.CourseId)
                            .Select(t => t.CourseTitle)
                            .FirstOrDefault();

                        Console.WriteLine($"{courseTitle}");
                    }

                    AttendanceOfStudent(studentData.Password, studentData.Id);
                }
                else
                {
                    Console.WriteLine("Currently, you are not enrolled in any courses.");
                }
                
            }
            else
            {
                Console.WriteLine("Wrong login credentials given!");
            }
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("This user does not exist!");
        }
        Console.WriteLine();
    }



    public void AttendanceOfStudent(string sPass, int sId)
    {
        var execute = new ExecuteSystemOperation();
        int flag = 0;
        int temp = 0;
        var context = new ProjectDbContext();
        var reLogin = new ExecuteSystemOperation();
        DateTime currentTime = DateTime.Now;

        Console.WriteLine();
        Console.WriteLine("Which course you want to take attendance for =>");
        Console.Write("Please give the Course Title: ");
        var courseTitle = Console.ReadLine();

        var allClassSchedules = context.ClassSchedules.ToList();

        var classSchedules = allClassSchedules
            .Where(c => string.Equals(c.CourseTitle, courseTitle, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (classSchedules.Any())
        {
            var currentDayOfWeek = currentTime.DayOfWeek.ToString();

            if (classSchedules.Any(c => string.Equals(c.Day, currentDayOfWeek, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"Taking attendance for {courseTitle} on {currentDayOfWeek}");
                Console.Write("For confirmation, please enter your password:");
                var pass = Console.ReadLine();

                if (pass == sPass)
                {
                    AttendanceRecord attendanceRecord = new AttendanceRecord();
                    attendanceRecord.CourseTitle = courseTitle;
                    attendanceRecord.Date = DateTime.Now;
                    attendanceRecord.StudentId = sId;
                    context.AttendanceRecords.Add(attendanceRecord);
                    context.SaveChanges();
                    flag = 1;
                    Console.WriteLine("Attendance taken successfully.");
                }
                else
                {
                    Console.WriteLine("Wrong Password!");
                    Console.WriteLine();
                    reLogin.StudentLoginData();
                }
            }
            else
            {
                Console.WriteLine($"No schedule for {courseTitle} on today {currentDayOfWeek}.");
            }
        }
        else
        {
            Console.WriteLine($"No schedule found for {courseTitle}.");
        }

        if (flag != 1 && temp == 0)
        {
            Console.WriteLine("Invalid Submission!");
        }

        execute.LoginOptions();
    }




}
