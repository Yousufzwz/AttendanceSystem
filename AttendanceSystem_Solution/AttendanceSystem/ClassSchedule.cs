using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem;

public class ClassSchedule
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string CourseTitle { get; set; }
    public string Day { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int TotalClasses { get; set; }
}
