
namespace Attendence_Monitoring_System.Models
{
    public partial class UserDetail
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string KeyName { get; set; } = null!;
        public string Value { get; set; } = null!;
        public int? SectionId { get; set; }

        public virtual Section? Section { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
