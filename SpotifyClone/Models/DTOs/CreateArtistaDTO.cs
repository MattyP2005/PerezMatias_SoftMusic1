using System.ComponentModel.DataAnnotations;

namespace SpotifyClone.Models.DTOs
{
    public class CreateArtistaDTO
    {
        [Required]
        public string Nombre { get; set; } 
        
        public string Biografia { get; set; } 
        
        public DateTime FechaNacimiento { get; set; }
        
        public string Pais { get; set; } 
        
        public string ImagenUrl { get; set; }  // URL de la imagen del artista
    }
}
