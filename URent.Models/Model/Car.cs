namespace URent.Models.Model
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 26/03/2017
    /// Description: Propriétés de la voiture
    /// </summary>
    public class Car
    {
        public int CarId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
    }
}