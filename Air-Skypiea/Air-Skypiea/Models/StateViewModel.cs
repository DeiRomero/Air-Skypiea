using System.ComponentModel.DataAnnotations;

namespace Air_Skypiea.Models
{
    public class StateViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Departamento/Estados")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        public int CountyId { get; set; }

    }
}
