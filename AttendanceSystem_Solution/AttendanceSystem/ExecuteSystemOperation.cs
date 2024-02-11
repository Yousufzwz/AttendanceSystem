using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem;

public class ExecuteSystemOperation
{
    
    public void LoginOptions()
    {
        int options; 

        Console.WriteLine("````Choose Option to Login`````");
        Console.WriteLine("1.Admin");
        Console.WriteLine("2.Student");
        Console.WriteLine("3.Teacher");
        Console.WriteLine("4.Logout");
        Console.WriteLine();
        Console.Write("Choose Option:");
        options = int.Parse(Console.ReadLine()); 

        Console.WriteLine();

        if (options >= 1 && options <= 4)
        {
            switch (options)
            {
                case 1:
                    Console.Write("Please Give your username:");
                    var uid = Console.ReadLine();
                    Console.Write("Please Give your password:");
                    var pass = Console.ReadLine();
                    var adminUser = new Admin();
                    adminUser.AdminLogin(uid, pass);
                    break;

                case 2:
                    StudentLoginData();
                    break;

                case 3:
                    TeacherLoginData();
                    break;

                case 4:
                    Environment.Exit(0); 
                    break;

                default:
                    Console.WriteLine("Invalid Number!!!");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Invalid Number!!!");
            LoginOptions();
        }
    }

    public void OptionsForAdmin()
    {
        int options; 

        Console.WriteLine("`````Now you're in Admin panel`````");
        Console.WriteLine();
        Console.WriteLine("1.Create Course");
        Console.WriteLine("2.Create Student");
        Console.WriteLine("3.Create Teacher");
        Console.WriteLine("4.Assign a Student in a Course");
        Console.WriteLine("5.Assign a Teacher in a Course");
        Console.WriteLine("6.Add Class Schedule for a Course");
        Console.WriteLine("7.Update an Existing Course Data");
        Console.WriteLine("8.Update an Existing Student Data");
        Console.WriteLine("9.Update an Existing Teacher Data");
        Console.WriteLine("10.Remove Course");
        Console.WriteLine("11.Remove Student");
        Console.WriteLine("12.Remove Teacher");
        Console.WriteLine("13.Back to menu");
        Console.WriteLine();
        Console.Write("Choose Option:");
        options = int.Parse(Console.ReadLine());

        Console.WriteLine();

        var admin = new Admin();

        if (options >= 1 && options <= 13)
        {
            switch (options)
            {
                case 1:
                    admin.AddCourse();
                    break;

                case 2:
                    admin.AddStudent();
                    break;

                case 3:
                    admin.AddTeacher();
                    break;

                case 4:
                    admin.CourseAssignToStudent();
                    break;

                case 5:
                    admin.CourseAssignToTeacher();
                    break;

                case 6:
                    admin.AddClassSchedule();
                    break;

                case 7:
                    admin.UpdateCourseData();
                    break;

                case 8:
                    admin.UpdateStudentData();
                    break;
                case 9:
                    admin.UpdateTeacherData();
                    break;
               
                case 10:
                    admin.RemoveCourse();
                    break;

                case 11:
                    admin.RemoveStudent();
                    break;

                case 12:
                    admin.RemoveTeacher();
                    break;

                case 13:
                    LoginOptions();
                    break;

                default:
                    Console.WriteLine("Invalid Number!!!");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Invalid Number!!!");
            OptionsForAdmin();
        }
    }



    public void StudentLoginData()
    {
        Console.Write("Student User Name:");
        var stuUserId = Console.ReadLine();
        Console.Write("Password :");
        var stuPass = Console.ReadLine();
        var student = new Student();
        student.LoginAsStudent(stuUserId, stuPass);
    }

    public void TeacherLoginData()
    {
        Console.Write("Teacher User Name:");
        var teacherUserId = Console.ReadLine();
        Console.Write("Password :");
        var teacherPass = Console.ReadLine();
        var teacher = new Teacher();
        teacher.LoginAsTeacher(teacherUserId, teacherPass);
    }

    public Course CourseDataSet()
    {
        var course = new Course();
        Console.WriteLine("Give Course Information Below-");
        Console.Write("Course Title:");
        course.CourseTitle = Console.ReadLine();
        Console.Write("Course Fees:");
        course.Fees= int.Parse(Console.ReadLine());
        Console.WriteLine();
        return course;
    }


    public Student StudentDataSet()
    {
        var student = new Student();
        Console.WriteLine("Give Student Information Below-");
        Console.Write("Student Name:");
        student.Name = Console.ReadLine();
        Console.Write("Username:");
        student.UserName = Console.ReadLine();
        Console.Write("Password:");
        student.Password = Console.ReadLine();
       

        return student;
    }

    
    public Teacher TeacherDataSet()
    {
        var teacher = new Teacher();
        Console.WriteLine("Give Teacher Information Below-");
        Console.Write("Teacher Name:");
        teacher.Name = Console.ReadLine();
        Console.Write("Username:");
        teacher.UserName = Console.ReadLine();
        Console.Write("Password:");
        teacher.Password = Console.ReadLine();

        return teacher;
    }



    public List<ClassSchedule> CreateClassSchedule()
    {
        var schedules = new List<ClassSchedule>();

        Console.Write("Set how many classes are held in a week:");
        var classDays = int.Parse(Console.ReadLine());

        if (classDays < 2 || classDays == 20)
        {
            Console.WriteLine("You can set 2 to a maximum of 20 as class schedules.");
            return CreateClassSchedule(); 
        }

        var classTime = ClassTimeForCourses(classDays);

        DateTime? startDate = null;
        bool isValidStartDate = false;

        while (!isValidStartDate)
        {
            Console.Write("Please enter the start date for the course (MM-dd-yyyy): ");
            var startDateInput = Console.ReadLine();

            if (DateTime.TryParseExact(startDateInput, "MM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                // Check if the start date matches any of the class days
                if (classTime.Any(dayTime => dayTime.DaysInWeek == parsedDate.DayOfWeek.ToString()))
                {
                    startDate = parsedDate;
                    isValidStartDate = true;
                }
                else
                {
                    Console.WriteLine("Start day does not match any of the class days.");
                }
            }
            else
            {
                Console.WriteLine("Invalid date format. Please use MM-dd-yyyy.");
            }
        }

        Console.Write("Please give the total number of classes:");
        var totalClasses = int.Parse(Console.ReadLine());

        foreach (var dy in classTime)
        {
            var schedule = new ClassSchedule
            {
                Day = dy.DaysInWeek,
                StartTime = dy.StartDayTime,
                EndTime = dy.EndDayTime,
                StartDate = CalculateStartDate(startDate.Value, dy.DaysInWeek),
                TotalClasses = totalClasses
            };

            schedules.Add(schedule);
        }

        return schedules;
    }


    // Function to calculate the adjusted start date based on the specified day
    public DateTime CalculateStartDate(DateTime startDate, string targetDay)
    {
        var today = startDate.DayOfWeek.ToString();
        var dayOffsets = new Dictionary<string, int>
    {
        {"Sunday", 0},
        {"Monday", 1},
        {"Tuesday", 2},
        {"Wednesday", 3},
        {"Thursday", 4},
        {"Friday", 5},
        {"Saturday", 6}
    };

        // Calculate the day difference between target day and current day
        var dayDifference = (dayOffsets[targetDay] + 7 - dayOffsets[today]) % 7;

        // Add the day difference to the start date
        return startDate.AddDays(dayDifference);
    }


    public List<ClassTimes>ClassTimeForCourses (int counter)
    {
        var dt = new List<ClassTimes>();
        
        for(int i=0; i < counter; i++)
        {
            Console.Write($"Please enter day {i+1} of the week:");
            var wd = WeekDays();
            Console.Write($"Start time in {wd} (example-9pm):");
            var st = DateTime.Parse(Console.ReadLine());
            Console.Write($"End time in {wd} (example-10pm):");
            var timeEnd = DateTime.Parse(Console.ReadLine());

            dt.Add(new ClassTimes() { DaysInWeek = wd, StartDayTime = st, EndDayTime = timeEnd });
        }
        return dt;
    }

    enum DaysWeek
    {
       
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday
    }

    public string WeekDays()
    {
        var dayinweek = Console.ReadLine();
        var d = string.Empty;
        if(dayinweek == DaysWeek.Sunday.ToString()
           || dayinweek == DaysWeek.Monday.ToString()
           || dayinweek == DaysWeek.Tuesday.ToString()
           || dayinweek == DaysWeek.Wednesday.ToString()
           || dayinweek == DaysWeek.Thursday.ToString()
           || dayinweek == DaysWeek.Friday.ToString()
           || dayinweek == DaysWeek.Saturday.ToString()
          )
        {
            d = dayinweek;
        }
        else
        {
            Console.WriteLine("Wrong input given");
            Console.Write("Give input again:");
            return dayinweek;
        }
        Console.WriteLine();

        return d;
    }


}