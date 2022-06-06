using Air_Skypiea.Data.Entities;

namespace Air_Skypiea.Models
{
    public class HomeViewModel
    {
        //public PaginatedList<Flight> Flights { get; set; }


        public ICollection<City> Cities { get; set; }
       
    }

}
