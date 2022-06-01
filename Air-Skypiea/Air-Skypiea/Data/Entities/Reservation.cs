using Air_Skypiea.Enums;
using System.ComponentModel.DataAnnotations;

namespace Air_Skypiea.Data.Entities
{
    public class Reservation
    {
        public int Id { get; set; }

        public Guid Code { get; set; }

        public ICollection<User> Users { get; set; }

        public FlightStatus flightStatus { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Remark { get; set; }
    }
}
