using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Air_Skypiea.Models
{
    public class CreateFlightViewModel : EditFlightViewModel
    {
        
        [Display(Name = "Origen")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una ciudad.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int SourceId { get; set; }
        public IEnumerable<SelectListItem> Source { get; set; }

        [Display(Name = "Destino")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una ciudad.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int TargetId { get; set; }
        public IEnumerable<SelectListItem> Target { get; set; }

        [Display(Name = "Foto")]
        public IFormFile? ImageFile { get; set; }
       

    }
}
