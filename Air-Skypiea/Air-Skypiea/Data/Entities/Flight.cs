using System.ComponentModel.DataAnnotations;

namespace Air_Skypiea.Data.Entities
{
    public class Flight
    {
        public int Id { get; set; }
        public City Source  { get; set; }
        public City Target { get; set; }


        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal Price { get; set; }
        public DateTime Date { get; set; }

        public ICollection<Reservation> Reservations { get; set; }



    }
}
