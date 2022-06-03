using System.ComponentModel.DataAnnotations;

namespace Air_Skypiea.Models
{
    public class CreateFlightViewModel:EditFlightViewModel
    {
        [Display(Name = "Foto")]
        public IFormFile? ImageFile { get; set; }
    }
}
