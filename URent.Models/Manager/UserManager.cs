using Newtonsoft.Json;
using System.Collections.Generic;
using URent.Models.Interfaces;
using System.Linq;
using System;
using System.Web;
using URent.Models.Util;

namespace URent.Models.Manager
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 09/04/2017
    /// Description: Cette classe est responsable de gérer les usagers admin
    /// </summary>
    public class UserManager : IUser
    {
        private readonly IHelper _helper;
        private readonly ICrypt _crypt;
        private const string Key = "URent17";

        public UserManager()
        {
            _helper = new Helper();
            _crypt = new Crypt();
        }

        public UserManager(IHelper helper, ICrypt crypt)
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
                var list = new List<Model.User>();
                var user = new Model.User { UserId = 1, FirstName = "Admin", Surname = "URent",  Password = _crypt.Encrypt("1234", Key), Email = "admin@admin.com" };
                list.Add(user);
                user = new Model.User { UserId = 2, FirstName = "Admin2", Surname = "URent", Password = _crypt.Encrypt("1234", Key), Email = "admin2@admin.com" };
                list.Add(user);
                var json = JsonConvert.SerializeObject(list);
                _helper.CreateJson("User", json);
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
        private void Generate(List<Model.User> users)
        {
            var json = JsonConvert.SerializeObject(users);
            _helper.CreateJson("User", json);
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction lit le fichier Json avec les données des usagers enregistrés
        /// </summary>
        /// <returns>Retourne une liste des usagers</returns>
        private IList<Model.User> ReadUser()
        {
            var list = JsonConvert.DeserializeObject<List<Model.User>>(_helper.ReadJson("User"));
            if (list != null)
            {
                return list;
            }
            return new List<Model.User>();
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction lit le fichier Json avec la liste complète des usagers enregistrés
        /// </summary>
        /// <returns>Retourne la liste complète des usagers</returns>
        public IList<Model.User> ListUsers()
        {
            return ReadUser();
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction lit le fichier Json avec les données des usagers enregistrés
        /// </summary>
        /// <param name="id">ID de l'usager</param>
        /// <returns>Retourne l'usager correspondant de l'ID</returns>
        public Model.User ListUser(int id)
        {
            var list = ReadUser();
            return list.Where(c => c.UserId == id).ToList()[0];
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
                var list = (List<Model.User>)ReadUser();
                list.RemoveAll(u => u.UserId == id);
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
        /// Description: Cette fonction crée un nouvel usager ou modifie un usager existant
        /// </summary>
        /// <param name="user">Usager à créer ou modifier</param>
        /// <returns></returns>
        public bool CreateUpdate(Model.User user)
        {
            try
            {
                user.Password = _crypt.Encrypt(user.Password, Key);
                var list = (List<Model.User>)ReadUser();
                if (user.UserId > 0)
                {
                    //Remove User to edit
                    list.RemoveAll(u => u.UserId == user.UserId);
                }
                else
                {
                    //If new User, we add the max UserId + 1
                    if (list.Count > 0)
                    {
                        user.UserId = list.Max(u => u.UserId) + 1;
                    }
                    else
                    {
                        user.UserId = 1;
                    }
                }
                list.Add(user);
                Generate(list);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction valide l'authentification d'usager
        /// </summary>
        /// <param name="user">Usager à authentifier</param>
        /// <returns></returns>
        public Model.User Authentification(Model.User user)
        {
            var list = ReadUser();
            var userLogin = list.FirstOrDefault(u => u.Email == user.Email && u.Password == _crypt.Encrypt(user.Password, Key));
            return userLogin;
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction check if the User (admin) is authenticated
        /// </summary>
        /// <returns>Return true if user is authenticated / False not authenticated</returns>
        public bool IsAuthenticated()
        {
            return HttpContext.Current.Session["UserId"] != null;
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction check if the email already exists
        /// </summary>
        /// <returns>Return true if the email exists / False not exists</returns>
        public bool CheckAvailableEmail(int userId, string email)
        {
            var list = ReadUser();
            var id = list.Where(c => c.Email == email).Select(c => c.UserId).FirstOrDefault();
            return !(id == 0 || id == userId);
        }
    }
}