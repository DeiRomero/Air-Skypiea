namespace Air_Skypiea.Data.Entities
{
    public class TemporalFightSale
    {
        public int Id { get; set; }

        public User User { get; set; }

        public Flight Flight{ get; set; }

        public float Quantity { get; set; }

        public string? Remarkas { get; set; }
    }
}
