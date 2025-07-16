using System.ComponentModel.DataAnnotations;

namespace SpotifyClone.Models.ViewModels
{
    public class MetodoPagoViewModel
    {
        public int UsuarioId { get; set; }

        [Required]
        [Display(Name = "Tipo de tarjeta")]
        public string TipoTarjeta { get; set; }

        [Required]
        [Display(Name = "Número de tarjeta")]
        [CreditCard]
        public string NumeroTarjeta { get; set; }

        [Required]
        [Display(Name = "Nombre del titular")]
        public string NombreTitular { get; set; }

        [Required]
        [Display(Name = "Expira (MM/AA)")]
        public string FechaExpiracion { get; set; }

        [Required]
        [Display(Name = "CVV")]
        [StringLength(4, MinimumLength = 3)]
        public string CVV { get; set; }
    }
}
