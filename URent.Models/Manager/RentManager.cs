using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using URent.Models.Interfaces;
using URent.Models.Util;

namespace URent.Models.Manager
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 28/03/2017
    /// Description: Cette classe est responsable de gérer la location
    /// </summary>
    public class RentManager : IRent
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
                var list = new List<Model.Rent>();
                var objOption = new OptionManager();
                var listOption = new List<Model.Option>();
                var option = objOption.ListOption(1);
                listOption.Add(option);
                var rent = new Model.Rent { RentId = 1, ReservationId = 1, ClientId = 1, CarId = 1, DateDeparture = DateTime.Parse("2017-03-27"), DateReturn = DateTime.Parse("2017-03-29"), Cost = 80, Options = listOption };
                list.Add(rent);

                listOption = new List<Model.Option>();
                option = objOption.ListOption(1);
                listOption.Add(option);
                option = objOption.ListOption(2);
                listOption.Add(option);
                rent = new Model.Rent { RentId = 2, ReservationId = 1, ClientId = 2, CarId = 2, DateDeparture = DateTime.Parse("2017-04-01"), DateReturn = DateTime.Parse("2017-04-03"), Cost = 90, Options = listOption };
                list.Add(rent);
                var json = JsonConvert.SerializeObject(list);
                Helper.CreateJson("Rent", json);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction génère un fichier Json avec des la liste des objets de location
        /// </summary>
        /// <param name="rent">Liste de location</param>
        private void Generate(List<Model.Rent> rent)
        {
            var json = JsonConvert.SerializeObject(rent);
            Helper.CreateJson("Rent", json);
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction lit le fichier Json avec les données des locations enregistrées
        /// </summary>
        /// <returns>Retourne une liste des locations</returns>
        private IList<Model.Rent> ReadRent()
        {
            var list = JsonConvert.DeserializeObject<List<Model.Rent>>(Helper.ReadJson("Rent"));
            return list ?? new List<Model.Rent>();
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction crée/modifie une location
        /// </summary>
        /// <param name="rent">Objet de location à créer/modifier</param>
        /// <returns>Retourne l'ID de la location</returns>
        public int CreateUpdate(Model.Rent rent)
        {
            try
            {
                var list = (List<Model.Rent>)ReadRent();
                if (rent.RentId > 0)
                {
                    //Remove rent to edit
                    list.RemoveAll(u => u.RentId == rent.RentId);
                }
                else
                {
                    //If new rent, we add the max RentId + 1
                    if (list.Count > 0)
                    {
                        rent.RentId = list.Max(u => u.RentId) + 1;
                    }
                    else
                    {
                        rent.RentId = 1;
                    }
                }
                list.Add(rent);
                Generate(list);
                return rent.RentId;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public IList<Model.Rent> ListRent()
        {
            return ReadRent();
        }

        //public IList<Model.Rent> ListRent(int id)
        //{
        //    var list = ReadRent();
        //    return list.Where(c => c.CarId == id).ToList();
        //}

        //public void Remove(Model.Rent rent)
        //{
        //    var list = (List<Model.Rent>)ReadRent();
        //    list.RemoveAll(u => u.RendId == rent.RendId);
        //    Generate(list);
        //}

    }
}