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
    /// Description: Cette classe est responsable de gérer la reservation
    /// </summary>
    public class ReservationManager : IReservation
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
                var list = new List<Model.Reservation>();
                var objOption = new OptionManager();
                var listOption = new List<Model.Option>();
                var option = objOption.ListOption(1);
                listOption.Add(option);
                var reservation = new Model.Reservation { ReservationId = 1, ClientId = 1, CarId = 1, DateReservation = DateTime.Parse("2017-03-27"), DateStartRent = DateTime.Parse("2017-03-27"), DateReturnRent = DateTime.Parse("2017-03-28"), Cost = 80, Options = listOption };
                list.Add(reservation);

                listOption = new List<Model.Option>();
                option = objOption.ListOption(1);
                listOption.Add(option);
                option = objOption.ListOption(2);
                listOption.Add(option);
                reservation = new Model.Reservation { ReservationId = 2, ClientId = 2, CarId = 2, DateReservation = DateTime.Parse("2017-03-27"), DateStartRent = DateTime.Parse("2017-04-01"), DateReturnRent = DateTime.Parse("2017-04-03"), Cost = 80, Options = listOption };
                list.Add(reservation);
                var json = JsonConvert.SerializeObject(list);
                Helper.CreateJson("Reservation", json);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction génère un fichier Json avec des la liste des objets de reservation
        /// </summary>
        /// <param name="reservations">Liste de reservation</param>
        private void Generate(List<Model.Reservation> reservations)
        {
            var json = JsonConvert.SerializeObject(reservations);
            Helper.CreateJson("Reservation", json);
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction lit le fichier Json avec les données des reservations enregistrées
        /// </summary>
        /// <returns>Retourne une liste des reservations</returns>
        private IList<Model.Reservation> ReadReservation()
        {
            var list = JsonConvert.DeserializeObject<List<Model.Reservation>>(Helper.ReadJson("Reservation"));

            return list ?? new List<Model.Reservation>();
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction retourne une liste complète des reservations
        /// </summary>
        /// <returns>Retourne une liste des reservations</returns>
        public IList<Model.Reservation> ListReservations()
        {
            return ReadReservation();
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction crée/modifie une location
        /// </summary>
        /// <param name="reservation">Objet de reservation à créer/modifier</param>
        /// <returns>Retourne true si la création/modification est faite avec succès / Sinon retourne false</returns>
        public bool CreateUpdate(Model.Reservation reservation)
        {
            try
            {
                var list = (List<Model.Reservation>)ReadReservation();
                if (reservation.ReservationId > 0)
                {
                    //Remove reservation to edit
                    list.RemoveAll(u => u.ReservationId == reservation.ReservationId);
                }
                else
                {
                    //If new reservation, we add the max ReservationId + 1
                    reservation.ReservationId = list.Max(u => u.ReservationId) + 1;
                }
                list.Add(reservation);
                Generate(list);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //public List<Model.Reservation> ListReservation(int id)
        //{
        //    var list = ReadReservation();
        //    return list.Where(c => c.CarId == id).ToList();
        //}

        //public void Remove(Model.Reservation reservation)
        //{
        //    var list = ReadReservation();
        //    list.RemoveAll(u => u.ReservationId == reservation.ReservationId);
        //    Generate(list);
        //}

    }
}