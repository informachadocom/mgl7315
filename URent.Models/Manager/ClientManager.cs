using Newtonsoft.Json;
using System.Collections.Generic;
using URent.Models.Interfaces;
using System.Linq;
using URent.Models.Util;
using System;
using System.Web;

namespace URent.Models.Manager
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 26/03/2017
    /// Description: Cette classe est responsable de gérer les usagers
    /// </summary>
    public class ClientManager : IClient
    {
        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction génère un fichier Json avec des données pré-définies
        /// </summary>
        /// <returns>Retourne true si la génération des données est faite avec succès / Sinon retourne false</returns>
        public bool Generate()
        {
            try
            {
                var list = new List<Model.Client>();
                var client = new Model.Client { ClientId = 1, FirstName = "Ricardo", Surname = "ricardo", Password = "1324", Email = "ricardo@gmail.com" };
                list.Add(client);
                client = new Model.Client { ClientId = 2, FirstName = "Frederic", Surname = "fred", Password = "1324", Email = "fred@gmail.com" };
                list.Add(client);
                client = new Model.Client { ClientId = 3, FirstName = "Marcos", Surname = "marcos", Password = "1324", Email = "marcos@gmail.com" };
                list.Add(client);
                client = new Model.Client { ClientId = 4, FirstName = "Youssef", Surname = "youssef", Password = "1324", Email = "youssef@gmail.com" };
                list.Add(client);
                var json = JsonConvert.SerializeObject(list);
                Helper.CreateJson("Client", json);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction génère un fichier Json avec les usagers
        /// </summary>
        /// <param name="Clients">Liste d'usagers</param>
        private void Generate(List<Model.Client> Clients)
        {
            var json = JsonConvert.SerializeObject(Clients);
            Helper.CreateJson("Client", json);
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction lit le fichier Json avec les données des usagers enregistrés
        /// </summary>
        /// <returns>Retourne une liste des usagers</returns>
        private IList<Model.Client> ReadClient()
        {
            var list = JsonConvert.DeserializeObject<List<Model.Client>>(Helper.ReadJson("Client"));
            if (list != null)
            {
                return list;
            }
            return new List<Model.Client>();
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction lit le fichier Json avec la liste complète des usagers enregistrés
        /// </summary>
        /// <returns>Retourne la liste complète des usagers</returns>
        public IList<Model.Client> ListClients()
        {
            return ReadClient();
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction lit le fichier Json avec les données des usagers enregistrés
        /// </summary>
        /// <param name="id">ID de l'usager</param>
        /// <returns>Retourne l'usager correspondant de l'ID</returns>
        public Model.Client ListClient(int id)
        {
            var list = ReadClient();
            return list.Where(c => c.ClientId == id).ToList()[0];
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction efface l'usager
        /// </summary>
        /// <param name="id">ID de l'usager</param>
        /// <returns>Succès = true / Échéc = false</returns>
        public bool Remove(int id)
        {
            try
            {
                var list = (List<Model.Client>)ReadClient();
                list.RemoveAll(u => u.ClientId == id);
                Generate(list);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction crée un nouvel usager ou modifie un usager existant
        /// </summary>
        /// <param name="client">Usager à créer ou modifier</param>
        /// <returns></returns>
        public bool CreateUpdate(Model.Client client)
        {
            try
            {
                var list = (List<Model.Client>)ReadClient();
                if (client.ClientId > 0)
                {
                    //Remove Client to edit
                    list.RemoveAll(u => u.ClientId == client.ClientId);
                }
                else
                {
                    //If new Client, we add the max ClientId + 1
                    if (list.Count > 0)
                    {
                        client.ClientId = list.Max(u => u.ClientId) + 1;
                    }
                    else
                    {
                        client.ClientId = 1;
                    }
                }
                list.Add(client);
                Generate(list);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction valide l'authentification d'usager
        /// </summary>
        /// <param name="client">Usager à authentifier</param>
        /// <returns></returns>
        public Model.Client Authentification(Model.Client client)
        {
            var list = ReadClient();
            var ClientLogin = list.FirstOrDefault(u => u.Email == client.Email && u.Password == client.Password);
            return ClientLogin;
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction check if the client is authenticated
        /// </summary>
        /// <returns>Return true if client is authenticated / False not authenticated</returns>
        public bool isAuthenticated()
        {
            return HttpContext.Current.Session["ClientId"] != null;
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction check if the email already exists
        /// </summary>
        /// <returns>Return true if the email exists / False not exists</returns>
        public bool CheckAvailableEmail(int clientId, string email)
        {
            var list = ReadClient();
            var id = list.Where(c => c.Email == email).Select(c => c.ClientId).FirstOrDefault();
            return !(id == 0 || id == clientId);
        }
    }
}