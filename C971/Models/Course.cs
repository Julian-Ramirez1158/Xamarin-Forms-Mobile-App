using System;
using SQLite;
using System.Collections.Generic;
using System.Text;

namespace C971.Models
{
    public class Course
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TermId { get; set; }

        [MaxLength(50)]
        public string CourseTitle { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string CourseStatus { get; set; }
        public string InstructorName { get; set; }
        public string InstructorPhone { get; set; }
        public string InstructorEmail { get; set; }
        public string CourseNotes { get; set; }
        public bool NotificationsOn { get; set; }





    }
}
