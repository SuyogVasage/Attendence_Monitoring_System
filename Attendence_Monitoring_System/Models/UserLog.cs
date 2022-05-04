using System;
using System.Collections.Generic;

namespace Attendence_Monitoring_System.Models
{
    public partial class UserLog
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public DateTime? Time { get; set; }
        public string? Status { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
