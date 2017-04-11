using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using URent.Models.Interfaces;
using URent.Models.Util;
using URent.Models.Model;

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
                var list = new List<Reservation>();
                var objOption = new OptionManager();
                var listOption = new List<Option>();
                var option = objOption.ListOption(1);
                listOption.Add(option);
                var reservation = new Reservation { ReservationId = 1, ClientId = 1, CarId = 1, DateReservation = DateTime.Parse("2017-03-27"), DateStartRent = DateTime.Parse("2017-03-27"), DateReturnRent = DateTime.Parse("2017-03-28"), Cost = 80, Options = listOption, Status = 1 };
                list.Add(reservation);

                listOption = new List<Option>();
                option = objOption.ListOption(1);
                listOption.Add(option);
                option = objOption.ListOption(2);
                listOption.Add(option);
                reservation = new Reservation { ReservationId = 2, ClientId = 2, CarId = 2, DateReservation = DateTime.Parse("2017-03-27"), DateStartRent = DateTime.Parse("2017-04-01"), DateReturnRent = DateTime.Parse("2017-04-03"), Cost = 80, Options = listOption, Status = 1 };
                list.Add(reservation);

                listOption = new List<Option>();
                option = objOption.ListOption(1);
                listOption.Add(option);
                option = objOption.ListOption(2);
                listOption.Add(option);
                reservation = new Reservation { ReservationId = 3, ClientId = 2, CarId = 3, DateReservation = DateTime.Parse("2017-03-27"), DateStartRent = DateTime.Parse("2017-04-01"), DateReturnRent = DateTime.Parse("2017-04-03"), Cost = 80, Options = listOption, Status = 1 };
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
        private void Generate(List<Reservation> reservations)
        {
            var json = JsonConvert.SerializeObject(reservations);
            Helper.CreateJson("Reservation", json);
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction lit le fichier Json avec les données des reservations enregistrées
        /// </summary>
        /// <returns>Retourne une liste des reservations</returns>
        private IList<Reservation> ReadReservation()
        {
            var list = JsonConvert.DeserializeObject<List<Model.Reservation>>(Helper.ReadJson("Reservation"));

            return list ?? new List<Model.Reservation>();
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction retourne une liste complète des reservations
        /// </summary>
        /// <returns>Retourne une liste des reservations</returns>
        public IList<Reservation> ListReservations()
        {
            return ReadReservation();
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction retourne une reservation par ID
        /// </summary>
        /// <param name="id">ID de la reservation</param>
        /// <returns>Retourne une reservation</returns>
        public Reservation ListReservation(int id)
        {
            var list = ReadReservation().Where(c => c.ReservationId == id).ToList();
            if (list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction retourne les reservations d'un client
        /// </summary>
        /// <param name="id">ID du client</param>
        /// <returns>Retourne toutes les reservations du client</returns>
        public IList<Model.List.Reservation> ListReservationByClient(int id)
        {
            var objC = new CarManager();
            var objCat = new CategoryManager();
            var listC = objC.ListCars();
            var listCat = objCat.ListCategories();
            var list = ReadReservation();

            var query = from r in list
                        join c in listC on r.CarId equals c.CarId
                        select new { r.ReservationId, r.DateReservation, r.DateStartRent, r.DateReturnRent, r.Cost, r.ClientId, c.CategoryId };

            var table = from q in query
                        join c in listCat on q.CategoryId equals c.CategoryId
                        where (q.ClientId == id)
                        select new { q.ReservationId, q.DateReservation, q.DateStartRent, q.DateReturnRent, q.Cost, c.Name };

            var listR = table.Select(obj => new Model.List.Reservation
            {
                ReservationId = obj.ReservationId,
                DateReservation = obj.DateReservation,
                DateStartRent = obj.DateStartRent,
                DateReturnRent = obj.DateReturnRent,
                Cost = obj.Cost,
                Category = obj.Name
            }).ToList();

            return listR;
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction retourne une liste des reservations qui n'ont pas de location
        /// </summary>
        /// <returns>List de reservations</returns>
        public IList<Reservation> ListReservationsWithNoRent()
        {
            var listRes = ReadReservation();
            var objRent = new RentManager();
            var listRen = objRent.ListRent();
            var table = (from r in listRes
                         where (listRen.Where(re => re.ReservationId == r.ReservationId).Count() <= 0)
                         select r).ToList();
            return table;
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction crée/modifie une location
        /// </summary>
        /// <param name="reservation">Objet de reservation à créer/modifier</param>
        /// <returns>Retourne true si la création/modification est faite avec succès / Sinon retourne false</returns>
        public Reservation CreateUpdate(Reservation reservation)
        {
            try
            {
                var list = (List<Reservation>)ReadReservation();
                if (reservation.ReservationId > 0)
                {
                    //Remove reservation to edit
                    list.RemoveAll(u => u.ReservationId == reservation.ReservationId);
                }
                else
                {
                    //If new reservation, we add the max ReservationId + 1
                    if (list.Count > 0)
                    {
                        reservation.ReservationId = list.Max(u => u.ReservationId) + 1;
                    }
                    else
                    {
                        reservation.ReservationId = 1;
                    }
                }
                list.Add(reservation);
                Generate(list);
                return reservation;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction vérifie si l'annulation d'une reservation est possible sans frais
        /// </summary>
        /// <param name="id">ID de la reservation</param>
        /// <returns>Retourne true si le délais d'annulation sans frais est possible</returns>
        public bool CheckCancelDelay(int id)
        {
            return true;
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction annule une reservation
        /// 0 = Annulé
        /// </summary>
        /// <param name="id">ID de la reservation</param>
        /// <returns>Retourne true si l'annulation est réalisé avec succès / Sinon retourne false</returns>
        public bool Cancel(int id)
        {
            try
            {
                var model = ListReservation(id);
                model.Status = 0;
                CreateUpdate(model);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //public void Remove(Model.Reservation reservation)
        //{
        //    var list = ReadReservation();
        //    list.RemoveAll(u => u.ReservationId == reservation.ReservationId);
        //    Generate(list);
        //}

    }
}