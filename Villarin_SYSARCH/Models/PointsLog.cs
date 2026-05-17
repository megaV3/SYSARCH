using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Villarin_SYSARCH.Models
{
    public class PointsLog
    {
        [Key]
        public string PointsLogId { get; set; } = Guid.NewGuid().ToString(); // Automatically generates a unique string ID

        // --- Relationship to Account ---
        [Required]
        public int AccountUniqueId { get; set; }

        [ForeignKey("AccountUniqueId")]
        public Account Account { get; set; } // Navigation property

        // --- Log Details ---
        [Required]
        public int PointsGiven { get; set; } // Can be positive (+10) or negative (-5)

        [Required]
        public string Reason { get; set; } // e.g., "Attended Sit-in Session", "Late Violation"

        [Required]
        public DateTime DateLogged { get; set; } = DateTime.Now; // Automatically sets the current date/time
    }
}
