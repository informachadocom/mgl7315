namespace URent.Models
{
    public class RentViewModel : Model.List.Rent
    {
        public string Car { get; set; }
        public string ClientName { get; set; }
    }
}