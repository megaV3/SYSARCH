using Villarin_SYSARCH.Models;

namespace Villarin_SYSARCH.ViewModels
{
    public class AdminPointsViewModel
    {
        public IEnumerable<CurrentSitIn>? SitIns{ get; set; }
        public int Points { get; set; } = 0;
        //public string ProfilePicture { get; set; }
    }
}
