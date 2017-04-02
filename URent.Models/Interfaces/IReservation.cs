using System.Collections.Generic;

namespace URent.Models.Interfaces
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 29/03/2017
    /// Description: Définir l'interface de reservation
    /// </summary>
    public interface IReservation
    {
        bool Generate();
        IList<Model.Reservation> ListReservations();
        bool CreateUpdate(Model.Reservation reservation);
    }
}