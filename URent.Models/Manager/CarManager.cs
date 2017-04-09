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
    /// Date: 29/03/2017
    /// Description: Cette classe est responsable de gérer les voitures
    /// </summary>
    public class CarManager : ICar
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
                var list = new List<Model.Car>();
                var car = new Model.Car { CarId = 1, Name = "Civic", CategoryId = 1 };
                list.Add(car);
                car = new Model.Car { CarId = 2, Name = "Corolla", CategoryId = 1 };
                list.Add(car);
                car = new Model.Car { CarId = 3, Name = "Edge", CategoryId = 2 };
                list.Add(car);
                car = new Model.Car { CarId = 4, Name = "Fiesta", CategoryId = 3 };
                list.Add(car);
                var json = JsonConvert.SerializeObject(list);
                Helper.CreateJson("Car", json);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction lit le fichier Json avec les données des voitures enregistrées
        /// </summary>
        /// <returns>Retourne une liste des voitures</returns>
        private List<Model.Car> ReadCar()
        {
            var list = JsonConvert.DeserializeObject<List<Model.Car>>(Helper.ReadJson("Car"));
            return list ?? new List<Model.Car>();
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction lit le fichier Json avec la liste complète des voitures enregistrées
        /// </summary>
        /// <returns>Retourne la liste complète des voitures</returns>
        public IList<Model.Car> ListCars()
        {
            return ReadCar();
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction retourne une voiture par ID
        /// </summary>
        /// <returns>Retourne une voiture</returns>
        public Model.Car ListCar(int id)
        {
            var list = ReadCar();
            list = list.Where(c => c.CarId == id).ToList();
            return list[0];
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction lit le fichier Json avec les données des voitures d'une catégorie
        /// </summary>
        /// <param name="id">ID de la catégorie</param>
        /// <returns>Retourne toutes les voitures d'une catégorie</returns>
        public IList<Model.Car> ListCarsByCategory(int id)
        {
            var list = ReadCar();
            return list.Where(c => c.CategoryId == id).ToList();
        }

        //public void GenerateCar(List<Model.Car> cars)
        //{
        //    var json = JsonConvert.SerializeObject(cars);
        //    Util.CreateJson("Car", json);
        //}


        //public List<Model.Car> ListCar(int id)
        //{
        //    var list = ReadCar();
        //    return list.Where(c => c.CarId == id).ToList();
        //}

        //public void AddModify(Model.Car car)
        //{
        //    var list = ReadCar();
        //    if (car.CarId > 0)
        //    {
        //        //Remove car to edit
        //        list.RemoveAll(u => u.CarId == car.CarId);
        //    }
        //    else
        //    {
        //        //If new car, we add the max CarId + 1
        //        car.CarId = list.Max(u => u.CarId) + 1;
        //    }
        //    list.Add(car);
        //    GenerateCar(list);
        //}

        //public void Remove(Model.Car car)
        //{
        //    var list = ReadCar();
        //    list.RemoveAll(u => u.CarId == car.CarId);
        //    GenerateCar(list);
        //}

    }
}