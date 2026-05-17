using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Villarin_SYSARCH.Models
{
    public class Account
    {
        [Key]
        public int UniqueId { get; set; }

        [Required(ErrorMessage = "ID is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Middle Name is required")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Course Level is required")]
        [Range(1,4)]
        public string CourseLevel { get; set; }

        [Required(ErrorMessage = "Course is required")]
        public string Course { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password does not match")]
        public string ConfirmPassword { get; set; }

        public int SessionsRemaining { get; set; } = 30;
        public string? ProfilePicture { get; set; }

        public int Points { get; set; } = 0;
        //public string ProfilePicture { get; set; }

    }
}
