using System;
using System.IO;

namespace URent.Models.Util
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 26/03/2017
    /// Description: Cette classe contient des fonctions utilitaires
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction génère un fichier Json avec des données
        /// </summary>
        /// <param name="name">Le nom du fichier</param>
        /// <param name="json">La structure de données en format Json</param>
        public static void CreateJson(string name, string json)
        {
            File.WriteAllText($@"{ReturnPathData()}\{name}.json", json);
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction lit le fichier Json
        /// </summary>
        /// <param name="name">Le nom du fichier</param>
        /// <returns>Retourne le contenu du fichier Json</returns>
        public static string ReadJson(string name)
        {
            var file = $@"{ReturnPathData()}\{name}.json";
            if (File.Exists(file))
            {
                return File.ReadAllText(file);
            }
            return string.Empty;
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction retourne le chemin du dossier App_Data du projet
        /// </summary>
        /// <returns>Retourne le chemin du dossier App_Data</returns>
        private static string ReturnPathData()
        {
            return AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
        }
    }
}