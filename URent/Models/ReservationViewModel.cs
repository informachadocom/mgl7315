﻿namespace URent.Models
{
    public class ReservationViewModel : Model.List.Reservation
    {
        public string Car { get; set; }
        public string ClientName { get; set; }
    }
}