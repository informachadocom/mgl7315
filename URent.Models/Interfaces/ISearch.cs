﻿using System;
using System.Collections.Generic;

namespace URent.Models.Interfaces
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 29/03/2017
    /// Description: Définir l'interface de recherche
    /// </summary>
    public interface ISearch
    {
        IList<Model.Order> SearchAvailableCategories(DateTime dateDeparture, DateTime dateReturn, int categoryId = 0);
        IList<Model.Category> GetCategoryAvailable(DateTime dateDeparture, DateTime dateReturn, int categoryId);
        int GetCarAvailable(DateTime dateDeparture, DateTime dateReturn, int categoryId);
        bool CheckAvailableCategory(DateTime dateDeparture, DateTime dateReturn, int categoryId);
    }
}