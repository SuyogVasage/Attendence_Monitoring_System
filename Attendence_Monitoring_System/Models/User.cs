using System;
using System.Collections.Generic;

namespace Attendence_Monitoring_System.Models
{
    public partial class User
    {
        public User()
        {
            AttendenceLogs = new HashSet<AttendenceLog>();
            UserDetails = new HashSet<UserDetail>();
            UserLogs = new HashSet<UserLog>();
        }

        public int UserId { get; set; }
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<AttendenceLog> AttendenceLogs { get; set; }
        public virtual ICollection<UserDetail> UserDetails { get; set; }
        public virtual ICollection<UserLog> UserLogs { get; set; }
    }
}
