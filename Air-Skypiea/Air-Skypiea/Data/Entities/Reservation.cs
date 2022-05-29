using Air_Skypiea.Enums;
using System.ComponentModel.DataAnnotations;

namespace Air_Skypiea.Data.Entities
{
    public class Reservation
    {
        public int Id { get; set; }

        public int Code { get; set; }

        public ICollection<User> Users { get; set; }

        //hay 2 formas
        //[Display(Name = "Estado")]
        //public Boolean Name { get; set; }

        //[Display(Name = "Estado")]
        //public OrderStatus OrderStatus { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Description { get; set; }


    }
}
