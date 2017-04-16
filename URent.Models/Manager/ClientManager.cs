using Newtonsoft.Json;
using System.Collections.Generic;
using URent.Models.Interfaces;
using System.Linq;
using System;
using System.Web;
<<<<<<< HEAD
using System.Text.RegularExpressions;
=======
using URent.Models.Util;
>>>>>>> origin/master

namespace URent.Models.Manager
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 26/03/2017
    /// Description: Cette classe est responsable de gérer les usagers
    /// </summary>
    public class ClientManager : IClient
    {
        private readonly IHelper _helper;
        private readonly ICrypt _crypt;
        private const string Key = "URent17";

        public ClientManager()
        {
            _helper = new Helper();
            _crypt = new Crypt();
        }

        public ClientManager(IHelper helper, ICrypt crypt)
        {
            _helper = helper;
            _crypt = crypt;
        }

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
                var client = new Model.Client { ClientId = 1, FirstName = "Ricardo", Surname = "ricardo", Password = _crypt.Encrypt("1324", Key), Email = "ricardo@gmail.com" };
                list.Add(client);
                client = new Model.Client { ClientId = 2, FirstName = "Frederic", Surname = "fred", Password = _crypt.Encrypt("1324", Key), Email = "fred@gmail.com" };
                list.Add(client);
                client = new Model.Client { ClientId = 3, FirstName = "Marcos", Surname = "marcos", Password = _crypt.Encrypt("1324", Key), Email = "marcos@gmail.com" };
                list.Add(client);
                client = new Model.Client { ClientId = 4, FirstName = "Youssef", Surname = "youssef", Password = _crypt.Encrypt("1324", Key), Email = "youssef@gmail.com" };
                list.Add(client);
                var json = JsonConvert.SerializeObject(list);
                _helper.CreateJson("Client", json);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction génère un fichier Json avec les usagers
        /// </summary>
        /// <param name="users">Liste d'usagers</param>
        private void Generate(List<Model.Client> users)
        {
            var json = JsonConvert.SerializeObject(users);
            _helper.CreateJson("Client", json);
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction lit le fichier Json avec les données des usagers enregistrés
        /// </summary>
        /// <returns>Retourne une liste des usagers</returns>
        private IList<Model.Client> ReadClient()
        {
            var list = JsonConvert.DeserializeObject<List<Model.Client>>(_helper.ReadJson("Client"));
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
                var exist = list.Any(c => c.ClientId == id);
                if (exist)
                {
                    list.RemoveAll(u => u.ClientId == id);
                    Generate(list);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
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
<<<<<<< HEAD
                if (client.FirstName == null || client.Surname == null || client.Email == null || client.Password == null)
                {
                    client.Error = "All the fields are required";
                    return false;
                }
                if (!Regex.Match(client.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success)
                {
                    client.Error = "Email format incorrect";
                    return false;
                }

                if (CheckAvailableEmail(client.ClientId, client.Email))
                {
                    client.Error = "Email already exists";
                    return false;
                }
=======
                client.Password = _crypt.Encrypt(client.Password, Key);
>>>>>>> origin/master
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
            catch (Exception)
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
            if (client.Password == null || client.Email == null)
            {
                client.Error = "All the fields are required";
                return client;
            }

            if (!Regex.Match(client.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success)
            {
                client.Error = "Email format incorrect";
                return client;
            }
            var list = ReadClient();
            var clientLogin = list.FirstOrDefault(u => u.Email == client.Email && _crypt.Decrypt(u.Password, Key) == client.Password);
            return clientLogin;
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction check if the client is authenticated
        /// </summary>
        /// <returns>Return true if client is authenticated / False not authenticated</returns>
        public bool IsAuthenticated()
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