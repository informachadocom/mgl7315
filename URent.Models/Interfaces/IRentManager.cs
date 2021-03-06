﻿using System.Collections.Generic;

namespace URent.Models.Interfaces
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 29/03/2017
    /// Description: Définir l'interface de location
    /// </summary>
    public interface IRentManager : IManager
    {
        int CreateUpdate(Model.Rent rent);
        IList<Model.Rent> ListRent();
        bool Cancel(int id);
    }
}