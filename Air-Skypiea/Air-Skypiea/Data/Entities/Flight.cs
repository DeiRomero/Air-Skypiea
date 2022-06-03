using System.ComponentModel.DataAnnotations;

namespace Air_Skypiea.Data.Entities
{
    public class Flight
    {
        public int Id { get; set; }
        public City Source  { get; set; }
        public City? Target { get; set; }


        public DateTime Date { get; set; }

        [DisplayFormat(DataFormatString ="{0:C2}")]
        [Display(Name = "Precio")]
        [Required(ErrorMessage ="El campo {0} es obligatorio.")]
        public decimal Price { get; set; }

        public ICollection<FlightImage> FlightImages { get; set; }

        [Display(Name = "Fotos")]
        public int ImagesNumber => FlightImages == null ? 0 : FlightImages.Count;

        [Display(Name = "Foto")]
        public string ImageFullPath => FlightImages == null || FlightImages.Count == 0
            ? $"https://localhost:7161/.azurewebsites.net/images/noimage.png"
            : FlightImages.FirstOrDefault().ImageFullPath;

    }
}
