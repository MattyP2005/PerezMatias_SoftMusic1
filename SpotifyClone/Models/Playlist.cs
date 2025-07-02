using System.ComponentModel.DataAnnotations;

namespace SpotifyClone.Models
{
    public class Playlist
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public DateTime FechaCreacion { get; set; }

        // Usuario dueño de la playlist
        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; }

        // Indica si la playlist es pública o privada
        public bool EsPublica { get; set; } = false; // Por defecto, privada

        // Relación con canciones
        public ICollection<PlaylistCancion> Canciones { get; set; }
    }
}
