using System.ComponentModel.DataAnnotations;

namespace SpotifyClone.Models.DTOs
{
    public class CreateUsuarioDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Rol { get; set; }

        [Required]
        public string Plan { get; set; }
    }
}
