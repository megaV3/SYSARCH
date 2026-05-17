using System.ComponentModel.DataAnnotations;

namespace Villarin_SYSARCH.Models
{
    public class CurrentSitIn
    {
        [Key]
        public int SitId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Purpose { get; set; }
        public string Lab {  get; set; }
        public int SessionRemaining { get; set; }

        public string Status { get; set; }
        public string? Feedback { get; set; }

        public int? Points { get; set; } = 0;
        public bool? isPointsGiven { get; set; } = false;
    }
}
