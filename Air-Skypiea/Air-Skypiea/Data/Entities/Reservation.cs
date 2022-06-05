using System.ComponentModel.DataAnnotations;

namespace Air_Skypiea.Data.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public Guid Code { get; set; }

        public User User { get; set; }
       
       // public FlightsStatus FlightsStatus { get; set; }

        [Display(Name = "Observación")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres")]
        public string Remarks { get; set; }
        public ICollection<City> Cities { get; set; }
        public Flight Flight { get; set; }

    }
}
