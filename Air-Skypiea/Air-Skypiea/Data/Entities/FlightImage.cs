﻿using System.ComponentModel.DataAnnotations;

namespace Air_Skypiea.Data.Entities
{
    public class FlightImage
    {
        public int Id { get; set; }

        public Flight Flight { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:7057/images/noimage.png"
            : $"https://airskypiea.blob.core.windows.net/destinations/{ImageId}";
    }
}
