using System.ComponentModel.DataAnnotations;

namespace SpotifyClone.Models.ViewModels
{
    public class ElegirSubPlanViewModel
    {
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "Debes seleccionar un sub-plan.")]
        public string SubPlan { get; set; } // "Personal", "Familiar", "Empresa"
    }
}