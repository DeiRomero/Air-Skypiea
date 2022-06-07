
using System.ComponentModel.DataAnnotations;

namespace Air_Skypiea.Models
{
    public class EditReservationViewModel
    {
        public int Id { get; set; }


        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remark { get; set; }

       
    }

}