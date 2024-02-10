using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem;

public class CourseTeacher
{
    public int CourseId { get; set; } 
    public int TeacherId { get; set ; }
    public DateTime CourseAssignDate { get; set; }
    public Teacher Teacher { get; set ;}
    public Course Course { get; set; }
 

}
