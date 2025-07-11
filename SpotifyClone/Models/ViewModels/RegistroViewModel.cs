using System.ComponentModel.DataAnnotations;

namespace SpotifyClone.Models.ViewModels
{
    public class RegistroViewModel
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Plan { get; set; } // "Gratis" o "Premium"
    }
}