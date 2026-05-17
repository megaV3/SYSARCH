using Villarin_SYSARCH.Models;

namespace Villarin_SYSARCH.ViewModels
{
    public class StudentDashboardViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Course { get; set; }
        public string CourseLevel { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int SessionsRemaining { get; set; }
        public int Points { get; set; }
        public string? ProfilePicture { get; set; }
        public IEnumerable<Announcement>? AnnouncementsList { get; set; }
    }
}
