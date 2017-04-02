using System;

namespace URent.Models.Model
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 28/03/2017
    /// Description: Propriétés des tarifications de la location
    /// </summary>
    public class RentPrice
    {
        public int RentPriceId { get; set; }
        public int CategoryId { get; set; }
        public DateTime? SaleDate { get; set; }
        public EnumDays.Days? SaleDay { get; set; }
        public decimal Price { get; set; }
    }
}