using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace C971.Models
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string AssessmentTitle { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string AssessmentType { get; set; }
        public bool NotificationsOn { get; set; }
    }
}
