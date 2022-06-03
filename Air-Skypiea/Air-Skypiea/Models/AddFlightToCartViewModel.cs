using Air_Skypiea.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Air_Skypiea.Models
{
    public class AddProductToCartViewModel
    {
        public int Id { get; set; }
        public City Source { get; set; }
        public City? Target { get; set; }


        public DateTime Date { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal Price { get; set; }

        public ICollection<FlightImage> FlightImages { get; set; }


        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string Remarks { get; set; }
    }
}
