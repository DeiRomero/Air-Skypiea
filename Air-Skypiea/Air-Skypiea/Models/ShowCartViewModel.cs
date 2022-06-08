
using Air_Skypiea.Data.Entities;
using Air_Skypiea.Enums;
using System.ComponentModel.DataAnnotations;

namespace Air_Skypiea.Models
{
    public class ShowCartViewModel
    {
        public int Id { get; set; }



        public User User { get; set; }



        [Display(Name = "Codigo")]
        public Guid Code { get; set; }




        public ICollection<Reservation> Reservations { get; set; }

        

        [Display(Name = "Estado del vuelo")]
        public FlightStatus flightStatus { get; set; }

        [Display(Name = "Tipo de documento")]
        public DocumentType DocumentType { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Remark { get; set; }

        [Display(Name = "Documento")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Document { get; set; }

        [Display(Name = "Nombres y Apellidos")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FullName { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime Date { get; set; }



        [Display(Name = "Origen")]
        public City Source { get; set; }



        [Display(Name = "Destino")]
        public City Target { get; set; }
    }
}
