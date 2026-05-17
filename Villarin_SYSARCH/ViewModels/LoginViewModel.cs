using System.ComponentModel.DataAnnotations;

namespace Villarin_SYSARCH.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public int Id { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
