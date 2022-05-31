namespace Air_Skypiea.Data.Entities
{
    public class Flight
    {
        public int Id { get; set; }
        public City? Source  { get; set; }
        public City? Target { get; set; }


        public DateTime Date { get; set; }

        public decimal Price { get; set; }  
    }
}
