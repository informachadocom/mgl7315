using System;
using System.Collections.Generic;

namespace URent.Models.Model
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 28/03/2017
    /// Description: Propriétés de la reservation
    /// </summary>
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int ClientId { get; set; }
        public int CarId { get; set; }
        public DateTime DateReservation { get; set; }
        public DateTime DateStartRent { get; set; }
        public DateTime DateReturnRent { get; set; }
        public decimal Cost { get; set; }
        public IList<Option> Options { get; set; }
    }
}