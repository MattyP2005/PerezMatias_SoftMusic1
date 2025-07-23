using System.ComponentModel.DataAnnotations;

namespace SpotifyClone.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Rol { get; set; }     // "Usuario", "Admin", "Artista"

        [Required]
        public string Plan { get; set; }    // "Free", "Personal", etc.

        // Opcionales para el perfil
        public string Nombre { get; set; }   // Nombre real o nickname

        public string? ImagenPerfilUrl { get; set; }  // URL o ruta relativa a la imagen de perfil

        public FormaPago FormaPago { get; set; }

        public ICollection<Playlist> Playlists { get; set; }

        public ICollection<Historial> Historial { get; set; }

        public ICollection<FormaPago> FormasPago { get; set; }

        public List<Album> Albumes { get; set; }
    }
}
