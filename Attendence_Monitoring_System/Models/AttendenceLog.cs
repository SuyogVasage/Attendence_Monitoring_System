using System;
using System.Collections.Generic;

namespace Attendence_Monitoring_System.Models
{
    public partial class AttendenceLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public double? TotalHours { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
