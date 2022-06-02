
namespace Attendence_Monitoring_System.Models
{
    public partial class Section
    {
        public Section()
        {
            UserDetails = new HashSet<UserDetail>();
        }

        public int SectionId { get; set; }
        public string Description { get; set; } = null!;

        public virtual ICollection<UserDetail> UserDetails { get; set; }
    }
}
