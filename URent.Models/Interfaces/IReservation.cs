using System.Collections.Generic;
using URent.Models.Model;

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
        IList<Reservation> ListReservations();
        Reservation CreateUpdate(Reservation reservation);
    }
}