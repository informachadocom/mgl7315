using Newtonsoft.Json;
using System.Collections.Generic;
using URent.Models.Interfaces;
using System.Linq;
using URent.Models.Util;

namespace URent.Models.Manager
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 26/03/2017
    /// Description: Cette classe est responsable de gérer les options des véhicules
    /// </summary>
    public class OptionManager : IOption
    {
        private readonly IHelper _helper;

        public OptionManager()
        {
            _helper = new Helper();
        }
        public OptionManager(IHelper helper)
        {
            _helper = helper;
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
                var list = new List<Model.Option>();
                var option = new Model.Option { OptionId = 1, Name = "GPS" };
                list.Add(option);
                option = new Model.Option { OptionId = 2, Name = "Child seat" };
                list.Add(option);
                var json = JsonConvert.SerializeObject(list);
                _helper.CreateJson("Option", json);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction lit le fichier Json avec les données des options enregistrées
        /// </summary>
        /// <returns>Retourne une liste des options</returns>
        private IList<Model.Option> ReadOption()
        {
            var list = JsonConvert.DeserializeObject<List<Model.Option>>(_helper.ReadJson("Option"));
            if (list != null)
            {
                return list;
            }
            return new List<Model.Option>();
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction lit le fichier Json avec la liste complète des options enregistrées
        /// </summary>
        /// <returns>Retourne la liste complète des options</returns>
        public IList<Model.Option> ListOptions()
        {
            return ReadOption();
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction lit le fichier Json avec les données des options enregistrées
        /// </summary>
        /// <param name="id">ID de l'option</param>
        /// <returns>Retourne l'option correspondante de l'ID</returns>
        public Model.Option ListOption(int id)
        {
            var list = ReadOption();
            return list.Where(c => c.OptionId == id).ToList()[0];
        }
    }
}