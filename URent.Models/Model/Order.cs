using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace URent.Models.Model
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 28/03/2017
    /// Description: Propriétés de la commande
    /// </summary>
    public class Order
    {
        public int CategoryId { get; set; }
        public DateTime DateDeparture { get; set; }
        public DateTime DateReturn { get; set; }
        public IList<OrderPrice> OrderPrice { get; set; }
        public IList<Option> Options { get; set; }
        public decimal TotalCar { get; set; }
        public decimal TotalOption { get; set; }
        public decimal Total { get; set; }
    }
}