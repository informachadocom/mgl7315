using System;

namespace URent.Models.Model
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 28/03/2017
    /// Description: Propriétés de la commande par jour
    /// </summary>
    public class OrderPrice
    {
        public DateTime RentDate { get; set; }
        public decimal Price { get; set; }
    }
}