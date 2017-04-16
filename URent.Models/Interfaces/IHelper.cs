namespace URent.Models.Interfaces
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 15/04/2017
    /// Description: Définir l'interface de la classe Helper
    /// </summary>
    public interface IHelper
    {
        void CreateJson(string name, string json);
        string ReadJson(string name);
    }
}