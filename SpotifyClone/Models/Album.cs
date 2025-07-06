using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SpotifyClone.Models
{
    public class Album
    {
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }

        public DateTime FechaLanzamiento { get; set; }

        // Relación con Artista
        public int ArtistaId { get; set; }
        public Artista Artista { get; set; }

        // Canciones que contiene el álbum
        public ICollection<AlbumCancion> AlbumCanciones { get; set; }
    }
}
