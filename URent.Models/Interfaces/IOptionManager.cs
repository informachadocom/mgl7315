using System.Collections.Generic;

namespace URent.Models.Interfaces
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 26/03/2017
    /// Description: Définir l'interface des options
    /// </summary>
    public interface IOptionManager : IManager
    {
        IList<Model.Option> ListOptions();
        Model.Option ListOption(int id);
    }
}