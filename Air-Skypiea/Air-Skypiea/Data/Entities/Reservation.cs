using Air_Skypiea.Enums;
using System.ComponentModel.DataAnnotations;

namespace Air_Skypiea.Data.Entities
{
    public class Reservation
    {
        public int id { get; set; }

        [Display(Name = "Codigo")]
        public Guid Code { get; set; }

        public Flight Flight { get; set; }

        public User User { get; set; }

        //public People People { get; set; }

        [Display(Name = "Estado del vuelo")]
        public FlightStatus flightStatus { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Remark { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Valor")]
        public decimal Value => Flight == null ? 0 : (decimal)Quantity * Flight.Price;

    }
}
