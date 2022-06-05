using System.ComponentModel.DataAnnotations;

namespace Air_Skypiea.Data.Entities
{
    public class FlightImage
    {       
       
            public int Id { get; set; }

            public Flight Flight { get; set; }

            [Display(Name = "Foto")]
            public Guid ImageId { get; set; }

            //TODO: Pending to change to the correct path
            [Display(Name = "Foto")]
            public string ImageFullPath => ImageId == Guid.Empty              
                ? $"https://localhost:7161/images/noimage.png"
            : $"https://airskyp1ea.blob.core.windows.net/flightimage/{ImageId}";
        
    }
}
