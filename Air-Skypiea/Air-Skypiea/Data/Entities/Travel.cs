﻿using System.ComponentModel.DataAnnotations;

namespace Air_Skypiea.Data.Entities
{
    public class Travel
    {
        public int Id { get; set; }

        public Reservation Reservation { get; set; }

        public City? Source { get; set; }

        public City? Target { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime Date { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Remark { get; set; }
    }
}
