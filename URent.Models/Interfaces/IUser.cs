using System.Collections.Generic;

namespace URent.Models.Interfaces
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 09/04/2017
    /// Description: Définir l'interface d'usager (admin)
    /// </summary>
    public interface IUser
    {
        bool Generate();
        IList<Model.User> ListUsers();
        Model.User ListUser(int id);
        bool Remove(int id);
        bool CreateUpdate(Model.User user);
        Model.User Authentification(Model.User user);
        bool IsAuthenticated();
        bool CheckAvailableEmail(int userId, string email);
    }
}