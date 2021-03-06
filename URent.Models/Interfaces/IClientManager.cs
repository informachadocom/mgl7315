﻿using System.Collections.Generic;

namespace URent.Models.Interfaces
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 26/03/2017
    /// Description: Définir l'interface du client
    /// </summary>
    public interface IClientManager : IManager
    {
        IList<Model.Client> ListClients();
        Model.Client ListClient(int id);
        bool Remove(int id);
        bool CreateUpdate(Model.Client client);
        Model.Client Authentification(Model.Client client);
        bool IsAuthenticated();
        bool CheckAvailableEmail(int clientId, string email);
    }
}