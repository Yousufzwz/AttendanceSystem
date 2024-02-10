using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem;

public class Teacher
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public List<CourseTeacher> AssignedCourses { get; set; }


    public void LoginAsTeacher(string username, string password)
    {
        var context = new ProjectDbContext();
        var loginTeacher = new ExecuteSystemOperation();

        var teacherData=context.Teachers.Where(x=>x.UserName.Equals(username)).FirstOrDefault();    
        if(teacherData != null)
        {
            if(username == teacherData.UserName & password == teacherData.Password)
            {
                Console.WriteLine($"Hi, dear instructor {teacherData.Name}." );
                 StudentAttendanceRecord();
            }
        }
        else
        {
            Console.WriteLine("Wrong data provided!");
        }

    }

    
    public void StudentAttendanceRecord()
    {
        var context = new ProjectDbContext();
        var execute = new ExecuteSystemOperation();
        var mL = 0;
        DateTime dateStarted = DateTime.Now;
        DateTime dateNow = DateTime.Now;
        int counter = 0;
        int dayCounter = 0;
        int dateCounter = 1;

        string[] dayOfWeek = new string[1000];
        string[] dateOnly = new string[1000];

        Console.WriteLine("Dear instructor, you can access student's attendance records by a specific course title:");
        Console.WriteLine();
        Console.Write("Please give the course title:");
        var courseTitle = Console.ReadLine();

        var attendance = context.AttendanceRecords.Where(x => x.CourseTitle == courseTitle).ToList();
        var idOfCourse = context.Courses.FirstOrDefault(x => x.CourseTitle == courseTitle);
        var isEnStudent = context.CourseStudents.Where(x => x.CourseId == idOfCourse.Id)
            .Include(x => x.Student)
            .ToList();
        var classData = context.ClassSchedules.Where(x => x.CourseTitle == courseTitle).ToList();

        foreach (var s in isEnStudent)
        {
            var len = s.Student.Name.Length;
            if (len > mL)
            {
                mL = len;
            }
            counter++;
        }

        foreach (var day in classData)
        {
            dateStarted = day.StartDate.Date;
            dayOfWeek[dayCounter] = day.Day;
            dayCounter++;
        }

        Console.WriteLine();

        for (int i = 0; ; i++)
        {
            int token = 0;
            if (i == 0)
            {
                int length = 0;
                while (length != mL + 2)
                {
                    length++;
                }
            }
            else
            {
                if (DateTime.Compare(dateStarted, dateNow) < 0)
                {
                    for (int j = 0; j < dayCounter; j++)
                    {
                        if (dateStarted.DayOfWeek.ToString() == dayOfWeek[j])
                        {
                            Console.WriteLine("Class Date:");
                            Console.WriteLine();
                            Console.WriteLine($"{dateStarted.Date.ToShortDateString()}");
                            dateOnly[j] = dateStarted.Date.ToShortDateString();
                            dateCounter++;
                            dateStarted = dateStarted.AddDays(1).Date;
                            token = 1;
                            break;
                        }
                    }
                    if (token != 1)
                    {
                        dateStarted = dateStarted.AddDays(1).Date;
                    }
                }
                if (DateTime.Compare(dateStarted, dateNow) == 0)
                {
                    Console.WriteLine($"{dateStarted.Date}");
                    dateOnly[i] = dateStarted.Date.ToShortDateString();
                    dateCounter++;
                    break;
                }

                if (DateTime.Compare(dateStarted, dateNow) > 0)
                {
                    break;
                }
                if (dateStarted > dateNow)
                {
                    Console.WriteLine("Course class not started!");
                    break;
                }
            }
        }

        Console.WriteLine();

        foreach (var enStu in isEnStudent)
        {
            Console.Write($" Student Name: {enStu.Student.Name.PadRight(mL)}");

            for (int k = 1; k < dateCounter; k++)
            {
                var date = DateTime.Parse(dateOnly[k]);

                var attendanceRecord = attendance.FirstOrDefault(a =>
                    a.StudentId == enStu.StudentId && a.Date.Date == date.Date);

                if (attendanceRecord != null)
                {
                    Console.Write(" ✓ ");
                }
                else
                {
                    Console.Write(" X ");
                }
            }

            Console.WriteLine();
        }

        Console.WriteLine();
        execute.LoginOptions();
    }


}