using System;
using System.Collections.Generic;

namespace URent.Models.Model
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 28/03/2017
    /// Description: Propriétés de la location
    /// </summary>
    public class Rent
    {
        public int RendId { get; set; }
        public int ReservationId { get; set; }
        public int ClientId { get; set; }
        public int CarId { get; set; }
        public DateTime DateDeparture { get; set; }
        public DateTime DateReturn { get; set; }
        public decimal Cost { get; set; }
        public IList<Option> Options { get; set; }
    }
}