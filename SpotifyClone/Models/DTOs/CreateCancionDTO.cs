using System;
using System.ComponentModel.DataAnnotations;

namespace SpotifyClone.Models.DTOs
{
    public class CreateCancionDTO
    {
        [Required]
        public string Titulo { get; set; }

        public string Genero { get; set; }

        [Required]

        public string Url { get; set; } // Donde se aloja la canción (puede ser local o simulada)

        public DateTime FechaSubida { get; set; }

        [Required]
        public int ArtistaId { get; set; } // ID del artista (usuario) que sube la canción

    }
}
