using System;
using System.Collections.Generic;

namespace AttendanceSystem.Domain
{
    public class Student
    {
        public int StudentId { get; set; }        
        public string FirstName { get; set; } 
        public string LastName { get; set; } = string.Empty;  
        public string Email { get; set; }      
        public string Phone { get; set; }          
        public int? ClassId { get; set; }
        public ClassEntity? ClassEntity { get; set; }        
        public List<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
    }
}
