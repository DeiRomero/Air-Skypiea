using Air_Skypiea.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Air_Skypiea.Models
{
    public class ShowCartViewModel
    {
        public User User { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string Remarks { get; set; }
    }
}
