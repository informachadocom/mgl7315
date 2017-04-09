using URent.Models.Model;

namespace URent.Models
{
    public class ReservationViewModel : Reservation
    {
        public string Category { get; set; }
        public string Car { get; set; }
        public string ClientName { get; set; }
    }
}