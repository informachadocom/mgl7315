using System.Collections.Generic;

namespace URent.Models.Interfaces
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 29/03/2017
    /// Description: Définir l'interface des voitures
    /// </summary>
    public interface ICarManager : IManager
    {
        IList<Model.Car> ListCars();
        Model.Car ListCar(int id);
        IList<Model.Car> ListCarsByCategory(int id);
    }
}