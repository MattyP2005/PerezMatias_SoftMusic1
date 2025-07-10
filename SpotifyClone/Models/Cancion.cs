using System.ComponentModel.DataAnnotations;

namespace SpotifyClone.Models
{
    public class Cancion
    {
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }

        public string Genero { get; set; }

        [Required]
        public string Url { get; set; } // Donde se aloja la canción (puede ser local o simulada)

        public DateTime FechaSubida { get; set; }

        // Relación con artista (usuario)
        public int ArtistaId { get; set; }

        public Artista Artista { get; set; }

        public int UsuarioId { get; set; }
        
        public Usuario Usuario { get; set; }

        public int? AlbumId { get; set; }

        public Album Album { get; set; }

        public int Reproducciones { get; set; } // para mostrar número de plays

        public TimeSpan Duracion { get; set; }  // para mostrar duración bonita

        public ICollection<AlbumCancion> AlbumCanciones { get; set; }

        public ICollection<PlaylistCancion> PlaylistCanciones { get; set; }
    }
}
