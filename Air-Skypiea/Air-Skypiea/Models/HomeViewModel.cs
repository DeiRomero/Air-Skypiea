using Air_Skypiea.Data.Entities;

namespace Air_Skypiea.Models
{
    public class HomeViewModel
    {
        public ICollection<Travel> Products { get; set; }

        public float Quantity { get; set; }
    }
}
