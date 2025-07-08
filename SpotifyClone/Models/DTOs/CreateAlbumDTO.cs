using System.ComponentModel.DataAnnotations;

namespace SpotifyClone.Models.DTOs
{
    public class CreateAlbumDTO
    {
        [Required]
        public string Titulo { get; set; }
    }
}
