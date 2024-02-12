using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AttendanceSystem;

public class Admin
{
    public string Username { get; private set; }
    public string Password { get; private set; }

    public Admin()
    {
        Username = "admin";
        Password = "123456";
    }


    public void AdminLogin(string username, string password)
    {
        var showOptions = new ExecuteSystemOperation();
        if (username == Username & password == Password)
        {
            Console.WriteLine();
            Console.WriteLine("Successfully Login as Admin!");
            Console.WriteLine();
            showOptions.OptionsForAdmin();
            
        }
        else
        {
            showOptions.LoginOptions();
        }
    }

    // Course Section

    public void AddCourse()
    {
        var context = new ProjectDbContext();
        var execute = new ExecuteSystemOperation();
        var cInfo = execute.CourseDataSet();
        var existingCourse = context.Courses.Where(x=>x.CourseTitle.Equals(cInfo.CourseTitle)).FirstOrDefault();    
        if (existingCourse != null)
        {
            if(cInfo.CourseTitle.ToUpper() == existingCourse.CourseTitle.ToUpper())
            {
                Console.WriteLine("Please give unique course title.");
                execute.OptionsForAdmin();
            }
            else
            {
                context.Courses.Add(cInfo);
                context.SaveChanges();
                Console.WriteLine();
                Console.WriteLine("Successfully Course Inserted!");
                Console.WriteLine();
                execute.OptionsForAdmin();
            }
        }
        else
        {
            context.Courses.Add(cInfo);
            context.SaveChanges();
            Console.WriteLine("Successfully Course Inserted!");
            Console.WriteLine();
            execute.OptionsForAdmin();

        }
    }


    public void UpdateCourseData()
    {
        var context = new ProjectDbContext();
        var execute = new ExecuteSystemOperation();

        Console.Write("Please enter the title of the course you want to update: ");
        var courseTitle = Console.ReadLine();

        var existingCourse = context.Courses.SingleOrDefault(x => x.CourseTitle.Equals(courseTitle));

        if (existingCourse != null)
        {
            Console.WriteLine("Current Course Information:");
            Console.WriteLine($"Title: {existingCourse.CourseTitle}");
            Console.WriteLine($"Fees: {existingCourse.Fees}");
            Console.WriteLine();

            Console.WriteLine("Enter the new course information:");

            Console.Write("New Title (Press Enter to keep current value): ");
            string newTitle = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newTitle))
            {
                existingCourse.CourseTitle = newTitle;
            }

            Console.Write("New Fees (Press Enter to keep current value): ");
            string newFeesString = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newFeesString) && int.TryParse(newFeesString, out int newFees))
            {
                existingCourse.Fees = newFees;
            }

            context.SaveChanges();

            Console.WriteLine();
            Console.WriteLine("Course Information Updated Successfully!");
        }
        else
        {
            Console.WriteLine("Course not found!");
        }

        execute.OptionsForAdmin();
    }


    public void RemoveCourse()
    {
        var context = new ProjectDbContext();
        var execute = new ExecuteSystemOperation();
        Console.Write("Please Enter Course title which is you want to remove:");
        var courseTitle = Console.ReadLine();
        var existingCourse = context.Courses.Where(x=>x.CourseTitle == courseTitle).ToList();

        if (existingCourse.Count > 0)
        {
            foreach (var removingCourse in existingCourse)
            {
                context.Courses.Remove(removingCourse);
            }
            context.SaveChanges();
            Console.WriteLine();
            Console.WriteLine("Successfully Course Removed!");
            Console.WriteLine();
            execute.OptionsForAdmin();
        }
        else
        {
            Console.WriteLine("No, course exists!");
            execute.OptionsForAdmin();  
        }
        

    }
    


    //Student Section
    public void AddStudent()
    {
        var context = new ProjectDbContext();
        var execute = new ExecuteSystemOperation();
        var sInfo = execute.StudentDataSet();

        var existingStudent = context.Students
            .FirstOrDefault(x => x.UserName.Equals(sInfo.UserName) || x.Password.Equals(sInfo.Password));

        if (existingStudent != null)
        {
            Console.WriteLine("Give Correct Information Please!");
        }
        else
        {
            context.Students.Add(sInfo);
            context.SaveChanges();
            Console.WriteLine();
            Console.WriteLine("Successfully Student Added!");
            Console.WriteLine();
        }

        execute.OptionsForAdmin();
    }


    public void CourseAssignToStudent()
    {
        var context = new ProjectDbContext();
        var execute = new ExecuteSystemOperation();
        Console.Write("Please Enter a course title which is you want to assign in a student:");
        var courseTitle = Console.ReadLine();
        Console.Write("Please Enter Student's username:");
        var suName= Console.ReadLine();

        var existingStudent = context.Students.Where(x=>x.UserName.Equals(suName)).FirstOrDefault();
        var existingCourse = context.Courses.Where(x=>x.CourseTitle.Equals(courseTitle)).FirstOrDefault();

        if (existingStudent != null & existingCourse != null)
        {
            var isStudentinCourse = context.CourseStudents
                .Where(x => x.StudentId == existingStudent.Id && x.CourseId == existingCourse.Id)
                .FirstOrDefault();

            if (isStudentinCourse == null)
            {

                if (existingStudent.UserName == suName & existingCourse.CourseTitle == courseTitle)
                {
                    var enroll = new CourseStudent();
                    enroll.StudentId = existingStudent.Id;
                    enroll.CourseId = existingCourse.Id;
                    enroll.EnrollDate = DateTime.Now;

                    context.CourseStudents.Add(enroll);
                    context.SaveChanges();
                    Console.WriteLine();
                    Console.WriteLine("Student enrolled in this course successfully!");
                    Console.WriteLine();
                    execute.OptionsForAdmin();
                }
                else
                {
                    Console.WriteLine("Wrong Data Inserted!");
                    Console.WriteLine();
                    execute.OptionsForAdmin();
                }
            }
            else
            {
                Console.WriteLine("The student previously enrolled in this course.");
                execute.OptionsForAdmin();
            }
        }
        else
        {
            Console.WriteLine("Wrong Data Inserted!");
            execute.OptionsForAdmin();
        }

    }


    public void UpdateStudentData()
    {
        var context = new ProjectDbContext();
        var execute = new ExecuteSystemOperation();

        Console.Write("Please enter the username of the student you want to update: ");
        var username = Console.ReadLine();

        var existingStudent = context.Students.SingleOrDefault(x => x.UserName.Equals(username));

        if (existingStudent != null)
        {
            Console.WriteLine("Current Student Information:");
            Console.WriteLine($"Name: {existingStudent.Name}");
            Console.WriteLine($"Username: {existingStudent.UserName}");
            Console.WriteLine($"Password: {existingStudent.Password}");
            Console.WriteLine();

            Console.WriteLine("Enter the new student information:");

            Console.Write("New Name (Press Enter to keep current value): ");
            string newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName))
            {
                existingStudent.Name = newName;
            }

            Console.Write("New Username (Press Enter to keep current value): ");
            string newUsername = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newUsername))
            {
                existingStudent.UserName = newUsername;
            }

            Console.Write("New Password (Press Enter to keep current value): ");
            string newPassword = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                existingStudent.Password = newPassword;
            }

            context.SaveChanges();

            Console.WriteLine();
            Console.WriteLine("Student Information Updated Successfully!");
        }
        else
        {
            Console.WriteLine("Student not found!");
        }

        execute.OptionsForAdmin();
    }



    public void RemoveStudent()
    {
        var context = new ProjectDbContext();
        var execute = new ExecuteSystemOperation();
        Console.Write("Please Enter Student's username which is you want to remove:");
        var username = Console.ReadLine();
        var existingStudent = context.Students.Where(x => x.UserName.Equals(username)).FirstOrDefault();

    if (existingStudent != null)
        {
            if(username == existingStudent.UserName)
            {
                var selectStudent = context.Students.Where(x=>x.UserName == username).ToList(); 

                foreach (var student  in selectStudent)
                {
                    context.Students.Remove(student);
                }
                context.SaveChanges();
                Console.WriteLine();
                Console.WriteLine("Successfully Student Removed!");
                Console.WriteLine();
                execute.OptionsForAdmin();
            }
            else
            {
                Console.WriteLine("The username and password is not exist!!");
                execute.OptionsForAdmin();
            }

        }
        else
        {
            Console.WriteLine("Data is empty!!!");
            execute.OptionsForAdmin();
        }


    }


    //Class Schedule Section
    public void AddClassSchedule()
    {
        var context = new ProjectDbContext();
        var execute = new ExecuteSystemOperation();

        Console.Write("Which course do you want to add to the schedule:");
        var courseTitle = Console.ReadLine();
        courseTitle = courseTitle.ToUpper(); 

        var isExistingCourse = context.Courses.FirstOrDefault(x => x.CourseTitle.ToUpper() == courseTitle);
        var isScheduleAvailable = context.ClassSchedules.FirstOrDefault(x => x.CourseTitle.ToUpper() == courseTitle);

        if (isExistingCourse != null)
        {
            if (isScheduleAvailable == null)
            {
                var scheduledClass = execute.CreateClassSchedule();

                foreach (var selectedDay in scheduledClass)
                {
                    selectedDay.CourseTitle = courseTitle; 
                    selectedDay.CourseId = isExistingCourse.Id;
                    context.ClassSchedules.Add(selectedDay);
                }

                context.SaveChanges();
                Console.WriteLine("Successfully Class Schedule Inserted!");
                execute.OptionsForAdmin();
            }
            else
            {
                Console.WriteLine($"The schedule is previously added for {courseTitle}");
            }
        }
        else
        {
            Console.WriteLine("This course does not exist!");
        }
    }


    //Teacher Section

    public void AddTeacher()
    {
        var context = new ProjectDbContext();
        var execute = new ExecuteSystemOperation();
        var tInfo = execute.TeacherDataSet();

        var existingTeacher = context.Teachers
            .FirstOrDefault(x => x.UserName.Equals(tInfo.UserName) || x.Password.Equals(tInfo.Password));

        if (existingTeacher != null)
        {
            Console.WriteLine("Give Correct Information Please!");
        }
        else
        {
            context.Teachers.Add(tInfo);
            context.SaveChanges();
            Console.WriteLine();
            Console.WriteLine("Successfully Teacher Added!");
            Console.WriteLine();
        }

        execute.OptionsForAdmin();
    }


    public void CourseAssignToTeacher()
    {
        var context = new ProjectDbContext();
        var execute = new ExecuteSystemOperation();
        Console.Write("Please Enter a course title which is you want to assign in a teacher:");
        var courseTitle = Console.ReadLine();
        Console.Write("Please Enter Teacher's username:");
        var tuName = Console.ReadLine();

        var existingTeacher = context.Teachers.Where(x => x.UserName.Equals(tuName)).FirstOrDefault();
        var existingCourse = context.Courses.Where(x => x.CourseTitle.Equals(courseTitle)).FirstOrDefault();

        if (existingTeacher != null & existingCourse != null)
        {
            var isTeacherinCourse = context.CourseTeachers
                .Where(x => x.TeacherId == existingTeacher.Id && x.CourseId == existingCourse.Id)
                .FirstOrDefault();

            if (isTeacherinCourse == null)
            {

                if (existingTeacher.UserName == tuName & existingCourse.CourseTitle == courseTitle)
                {
                    var courseAssign = new CourseTeacher();
                    courseAssign.TeacherId = existingTeacher.Id;
                    courseAssign.CourseId = existingCourse.Id;
                    courseAssign.CourseAssignDate = DateTime.Now;

                    context.CourseTeachers.Add(courseAssign);
                    context.SaveChanges();
                    Console.WriteLine();
                    Console.WriteLine("Teacher assigned in this course successfully!");
                    Console.WriteLine();
                    execute.OptionsForAdmin();
                }
                else
                {
                    Console.WriteLine("Wrong Data Inserted!");
                    Console.WriteLine();
                    execute.OptionsForAdmin();
                }
            }
            else
            {
                Console.WriteLine("The honorable teacher previously assigned in this course!");
                execute.OptionsForAdmin();
            }
        }
        else
        {
            Console.WriteLine("Wrong Data Inserted!");
            execute.OptionsForAdmin();
        }



    }


    public void UpdateTeacherData()
    {
        var context = new ProjectDbContext();
        var execute = new ExecuteSystemOperation();

        Console.Write("Please enter the username of the teacher you want to update: ");
        var username = Console.ReadLine();

        var existingTeacher = context.Teachers.SingleOrDefault(x => x.UserName.Equals(username));

        if (existingTeacher != null)
        {
            Console.WriteLine("Current Teacher Information:");
            Console.WriteLine($"Name: {existingTeacher.Name}");
            Console.WriteLine($"Username: {existingTeacher.UserName}");
            Console.WriteLine($"Password: {existingTeacher.Password}");
            Console.WriteLine();

            Console.WriteLine("Enter the new teacher information:");

            Console.Write("New Name (Press Enter to keep current value): ");
            string newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName))
            {
                existingTeacher.Name = newName;
            }

            Console.Write("New Username (Press Enter to keep current value): ");
            string newUsername = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newUsername))
            {
                existingTeacher.UserName = newUsername;
            }

            Console.Write("New Password (Press Enter to keep current value): ");
            string newPassword = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                existingTeacher.Password = newPassword;
            }

            context.SaveChanges();

            Console.WriteLine();
            Console.WriteLine("Teacher Information Updated Successfully!");
        }
        else
        {
            Console.WriteLine("Teacher not found!");
        }

        execute.OptionsForAdmin();
    }


    public void RemoveTeacher()
    {
        var context = new ProjectDbContext();
        var execute = new ExecuteSystemOperation();
        Console.Write("Please Enter Teacher's username which is you want to remove:");
        var username = Console.ReadLine();
        var existingTeacher = context.Teachers.Where(x => x.UserName.Equals(username)).FirstOrDefault();

        if (existingTeacher != null)
        {
            if (username == existingTeacher.UserName)
            {
                var selectTeacher = context.Teachers.Where(x => x.UserName == username).ToList();

                foreach (var teacher in selectTeacher)
                {
                    context.Teachers.Remove(teacher);
                }
                context.SaveChanges();
                Console.WriteLine();
                Console.WriteLine("Successfully Teacher Removed!");
                Console.WriteLine();
                execute.OptionsForAdmin();
            }
            else
            {
                Console.WriteLine("The username and password is not exist!!");
                execute.OptionsForAdmin();
            }

        }
        else
        {
            Console.WriteLine("Data is empty!!!");
            execute.OptionsForAdmin();
        }


    }


}