using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SpotifyClone.Models.ViewModels
{
    public class EditarPerfilViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public string ImagenPerfilUrl { get; set; }

        [Display(Name = "Nueva imagen de perfil")]
        public IFormFile ImagenPerfil { get; set; }

        [Display(Name = "Eliminar imagen actual")]
        public bool EliminarImagen { get; set; }
    }
}
