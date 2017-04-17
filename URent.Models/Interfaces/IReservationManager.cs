using System.Collections.Generic;
using URent.Models.Model;

namespace URent.Models.Interfaces
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 29/03/2017
    /// Description: Définir l'interface de reservation
    /// </summary>
    public interface IReservationManager : IManager
    {
        IList<Reservation> ListReservations();
        Reservation ListReservation(int id);
        IList<Model.List.Reservation> ListReservationByClient(int id);
        Reservation CreateUpdate(Reservation reservation);
        IList<Reservation> ListReservationsWithNoRent();
        bool CheckCancelDelay(int id);
        bool Cancel(int id);
    }
}