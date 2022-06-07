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


        public ICollection<SaleDetail> SaleDetails { get; set; }

        public ICollection<Reservation> Reservations { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Remark { get; set; }



    }
}
