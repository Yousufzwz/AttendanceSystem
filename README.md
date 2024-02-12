# AttendanceSystem

## Installation
:warning: Prerequisite tools for run this project: `Microsoft Visual Studio`, `Microsoft SQL Server`, and `SQL Server Management Studio`
01. Clone the project
```
https://github.com/Yousufzwz/AttendanceSystem/
```
3. Run the `.sln` file with Visual Studio

4. Set up a database using the default configuration or customize it according to your preferences

5. Now, Create and Update the migrations by executing the following commands in the `Package Manager Console`

```
dotnet ef migrations add Meaningfulname --project AttendanceSystem --context ProjectDbContext
dotnet ef database update --project AttendanceSystem --context ProjectDbContext
```
## Features
- Admin can create, update and remove teachers, students and courses.
- The system allows Admin to assign courses to both teachers and students, and also to set the schedule for the courses.
- Students can give attendance on their assigned course during the course schedule.
- Teachers have access to the attendance list for specific courses assigned to them by the administrator.

## Tech Stack
**Backend:** ASP.NET Core 7, Entity Framework Core

**Server:** Microsoft SQL Server

 
