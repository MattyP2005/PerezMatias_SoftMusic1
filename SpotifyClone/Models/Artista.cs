using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SpotifyClone.Models
{
    public class Artista
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public string? PortadaUrl { get; set; }      // Ruta a la imagen .jpg/.png

        public string Biografia { get; set; }

        public string Pais { get; set; }

        // Relación: un artista puede subir muchas canciones
        public ICollection<Cancion> Canciones { get; set; }

        public string? Email { get; set; }
    }
}
