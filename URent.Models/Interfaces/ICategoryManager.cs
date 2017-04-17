using System.Collections.Generic;

namespace URent.Models.Interfaces
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 26/03/2017
    /// Description: Définir l'interface des catégories
    /// </summary>
    public interface ICategoryManager : IManager
    {
        IList<Model.Category> ListCategories();
        Model.Category ListCategory(int id);
    }
}