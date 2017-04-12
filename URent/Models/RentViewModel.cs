namespace URent.Models
{
    public class RentViewModel : Model.List.Rent
    {
        public string Category { get; set; }
        public string Car { get; set; }
        public string ClientName { get; set; }
    }
}