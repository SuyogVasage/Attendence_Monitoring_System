using System;
using System.Collections.Generic;

namespace Attendence_Monitoring_System.Models
{
    public partial class Regularization
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
        public string? TotalHours { get; set; }
        public string? Status { get; set; }
        public string? Reason { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
