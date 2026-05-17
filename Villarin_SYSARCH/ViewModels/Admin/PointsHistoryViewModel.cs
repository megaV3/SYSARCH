namespace Villarin_SYSARCH.ViewModels.Admin
{
    public class PointsHistoryViewModel
    {
        public int SitId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string Purpose { get; set; }
        public string Lab { get; set; }
        public int PointsGiven { get; set; }
        public int SessionNumber { get; set; }
        public bool? isPointsGiven { get; set; } = false;
    }
}
