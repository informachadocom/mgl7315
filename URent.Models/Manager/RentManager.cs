﻿using System;
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
    public class RentManager : IRentManager
    {
        private readonly IHelper _helper;
        private readonly IOptionManager _objOptionManager;

        public RentManager()
        {
            _helper = new Helper();
            _objOptionManager = new OptionManager();
        }

        public RentManager(IHelper helper, IOptionManager optionManager)
        {
            _helper = helper;
            _objOptionManager = optionManager;
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
                var list = new List<Model.Rent>();
                var listOption = new List<Model.Option>();
                var option = _objOptionManager.ListOption(1);
                listOption.Add(option);
                var rent = new Model.Rent { RentId = 1, ReservationId = 1, ClientId = 1, CarId = 1, DateDeparture = DateTime.Parse("2017-03-27"), DateReturn = DateTime.Parse("2017-03-29"), Cost = 80, Options = listOption, Status = 1};
                list.Add(rent);

                listOption = new List<Model.Option>();
                option = _objOptionManager.ListOption(1);
                listOption.Add(option);
                option = _objOptionManager.ListOption(2);
                listOption.Add(option);
                rent = new Model.Rent { RentId = 2, ReservationId = 1, ClientId = 2, CarId = 2, DateDeparture = DateTime.Parse("2017-04-01"), DateReturn = DateTime.Parse("2017-04-03"), Cost = 90, Options = listOption, Status = 1 };
                list.Add(rent);
                var json = JsonConvert.SerializeObject(list);
                _helper.CreateJson("Rent", json);
                return true;
            }
            catch (Exception)
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
            _helper.CreateJson("Rent", json);
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction lit le fichier Json avec les données des locations enregistrées
        /// </summary>
        /// <returns>Retourne une liste des locations</returns>
        private IList<Model.Rent> ReadRent()
        {
            var list = JsonConvert.DeserializeObject<List<Model.Rent>>(_helper.ReadJson("Rent"));
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
            catch (Exception)
            {
                return 0;
            }
        }

        public IList<Model.Rent> ListRent()
        {
            return ReadRent();
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction annule une location
        /// 0 = Annulé
        /// </summary>
        /// <param name="id">ID de la location</param>
        /// <returns>Retourne true si l'annulation est réalisé avec succès / Sinon retourne false</returns>
        public bool Cancel(int id)
        {
            try
            {
                var model = ListRent(id);
                model.Status = 0;
                CreateUpdate(model);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction liste une location
        /// </summary>
        /// <param name="id">ID de la location</param>
        /// <returns>Retourne une location</returns>
        private Model.Rent ListRent(int id)
        {
            var list = ReadRent();
            return list.Where(c => c.RentId == id).ToList()[0];
        }

    }
}