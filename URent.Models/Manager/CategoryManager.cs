using System;
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
    /// Description: Cette classe est responsable de gérer les catégories des véhicules
    /// </summary>
    public class CategoryManager : ICategory
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
                var list = new List<Model.Category>();
                var category = new Model.Category { CategoryId = 1, Name = "Compact" };
                list.Add(category);
                category = new Model.Category { CategoryId = 2, Name = "SUV" };
                list.Add(category);
                category = new Model.Category { CategoryId = 3, Name = "Family" };
                list.Add(category);
                var json = JsonConvert.SerializeObject(list);
                Helper.CreateJson("Category", json);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction lit le fichier Json avec les données des catégories enregistrées
        /// </summary>
        /// <returns>Retourne une liste des catégories</returns>
        private IList<Model.Category> ReadCategory()
        {
            var list = JsonConvert.DeserializeObject<List<Model.Category>>(Helper.ReadJson("Category"));
            if (list != null)
            {
                return list;
            }
            return new List<Model.Category>();
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction lit le fichier Json avec la liste complète des catégories enregistrées
        /// </summary>
        /// <returns>Retourne la liste complète des catégories</returns>
        public IList<Model.Category> ListCategories()
        {
            return ReadCategory();
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction lit le fichier Json avec les données des catégories enregistrées
        /// </summary>
        /// <param name="id">ID de la catégorie</param>
        /// <returns>Retourne la catégorie correspondante de l'ID</returns>
        public Model.Category ListCategory(int id)
        {
            var list = ReadCategory();
            return list.Where(c => c.CategoryId == id).ToList()[0];
        }
    }
}