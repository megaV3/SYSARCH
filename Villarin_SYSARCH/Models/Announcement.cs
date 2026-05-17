using System.ComponentModel.DataAnnotations;

namespace Villarin_SYSARCH.Models
{
    public class Announcement
    {
        [Key]
        public int AnnouncementId { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public string Author { get; set; }
    }
}
