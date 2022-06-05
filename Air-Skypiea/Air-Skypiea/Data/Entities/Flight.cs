using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Air_Skypiea.Data.Entities
{
    public class Flight
    {
        public int Id { get; set; }
        public City Source { get; set; }
        public City Target { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal Price { get; set; }

       // [DisplayFormat(DataFormatString = "{0:yyy/mm/dd}")]
        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime Date { get; set; }
        public ICollection<Reservation> Reservations { get; set; }

        public ICollection<FlightImage> FlightImages { get; set; }

        [Display(Name = "Fotos")]
        public int ImagesNumber => FlightImages == null ? 0 : FlightImages.Count;

        //TODO: Pending to change to the correct path
        [Display(Name = "Foto")]
        public string ImageFullPath => FlightImages == null || FlightImages.Count == 0
         ? $"https://localhost:7161/images/noimage.png"
         : FlightImages.FirstOrDefault().ImageFullPath;


    }
}
