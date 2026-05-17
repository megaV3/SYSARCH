using Villarin_SYSARCH.Models;

namespace Villarin_SYSARCH.ViewModels
{
    public class DashboardViewModel
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Author { get; set; }
        public IEnumerable<Announcement>? AnnouncementsList { get; set; }
    }
}
