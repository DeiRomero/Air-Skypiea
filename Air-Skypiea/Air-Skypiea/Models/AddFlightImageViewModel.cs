using System.ComponentModel.DataAnnotations;

namespace Air_Skypiea.Models
{
    public class AddFlightImageViewModel
    {
        
            public int FlightId { get; set; }

            [Display(Name = "Foto")]
            [Required(ErrorMessage = "El campo {0} es obligatorio.")]
            public IFormFile ImageFile { get; set; }
        

    }
}
