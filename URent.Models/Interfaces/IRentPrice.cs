using System;
using System.Collections.Generic;

namespace URent.Models.Interfaces
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 29/03/2017
    /// Description: Définir l'interface des prix de location
    /// </summary>
    public interface IRentPrice
    {
        bool Generate();
        decimal GetPrice(DateTime date, int categoryId);
        IList<Model.Order> CalculatePriceSearch(IList<Model.Category> categories, DateTime departureDate, DateTime returnDate);
        IList<Model.Order> RecalculatePriceSearch(IList<Model.Order> orderPrice);
    }
}