using System;
using System.Collections.Generic;
using System.Linq;
using URent.Models.Interfaces;

namespace URent.Models.Manager
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 28/03/2017
    /// Description: Cette classe est responsable de chercher les catégories disponibles (moteur de recherche)
    /// </summary>
    public class SearchManager : ISearch
    {
        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction cherche les catégories encore disponibles pour la réservation
        /// </summary>
        /// <param name="dateDeparture">Date de sortie</param>
        /// <param name="dateReturn">Date de retour</param>
        /// <param name="categoryId">(Optionnel) ID de la catégorie / Si pas envoyé ou zero, la fonction cherche pour toutes les catégories</param>
        /// <returns>Retourne une liste des catégories disponibles</returns>
        public IList<Model.Order> SearchAvailableCategories(DateTime dateDeparture, DateTime dateReturn, int categoryId, int reservationId = 0)
        {

            //List of categories available
            var listCategoryAvailable = GetCategoryAvailable(dateDeparture, dateReturn, categoryId, reservationId);
            var listCategoriesPrice = new List<Model.Order>();
            var objPrice = new RentPriceManager();
            listCategoriesPrice = (List<Model.Order>)objPrice.CalculatePriceSearch(listCategoryAvailable, dateDeparture, dateReturn);
            return listCategoriesPrice;
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction cherche les catégories encore disponibles pour la réservation
        /// </summary>
        /// <param name="dateDeparture">Date de sortie</param>
        /// <param name="dateReturn">Date de retour</param>
        /// <param name="categoryId">(Optionnel) ID de la catégorie / Si pas envoyé ou zero, la fonction cherche pour toutes les catégories</param>
        /// <returns>Retourne une liste des catégories disponibles</returns>
        public IList<Model.Category> GetCategoryAvailable(DateTime dateDeparture, DateTime dateReturn, int categoryId, int reservationId = 0)
        {
            var objRes = new ReservationManager();
            var objCar = new CarManager();
            var objOption = new OptionManager();
            var listR = objRes.ListReservations();
            var listC = objCar.ListCars();
            //Create a temporary table with all necessaries columns
            var table = (from r in listR
                         select
                         new
                         {
                             r.DateStartRent,
                             r.DateReturnRent,
                             r.CarId,
                             r.ReservationId,
                             CategoryId = (from c in listC where c.CarId == r.CarId select (c.CategoryId)).First()
                         });

            //Verify the categories in reservation according to dates and category
            var query = from r in table
                        where
                        (((dateDeparture >= r.DateStartRent && dateDeparture <= r.DateReturnRent) ||
                          (dateReturn >= r.DateStartRent && dateReturn <= r.DateReturnRent))
                         ||
                         ((r.DateStartRent >= dateDeparture && r.DateStartRent <= dateReturn) ||
                          (r.DateReturnRent >= dateDeparture && r.DateReturnRent <= dateReturn)))
                        && (r.CategoryId == categoryId || categoryId == 0)
                        && (r.ReservationId != reservationId || reservationId == 0)
                        select new { r.CategoryId };

            //Count the numbers of categories in reservation
            var categories = from c in query
                             group c by c.CategoryId
                into g
                             select new
                             {
                                 CategoryId = g.Key,
                                 Count = (from c in g select c.CategoryId).Count()
                             };


            var listCategory = new List<Model.Category>();
            var objCat = new CategoryManager();
            if (categoryId > 0)
            {
                listCategory = new List<Model.Category> { objCat.ListCategory(categoryId) };
            }
            else
            {
                listCategory = (List<Model.Category>)objCat.ListCategories();
            }
            //List of categories available
            var listCategoryAvailable = new List<Model.Category>();
            //Verify if exists categories cars available
            foreach (var cat in listCategory)
            {
                var count = GetCountCars(cat.CategoryId);
                var countCategory = (from c in categories where cat.CategoryId == c.CategoryId select (c.Count)).FirstOrDefault();
                count = count - countCategory;
                if (count > 0)
                {
                    var category = objCat.ListCategory(cat.CategoryId);
                    listCategoryAvailable.Add(category);
                }
            }
            return listCategoryAvailable;
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction prend une voiture disponible selon les dates et la catégorie
        /// </summary>
        /// <param name="dateDeparture">Date de sortie</param>
        /// <param name="dateReturn">Date de retour</param>
        /// <param name="categoryId">ID de la catégorie</param>
        /// <returns>Retourne un ID de la voiture disponible</returns>
        public int GetCarAvailable(DateTime dateDeparture, DateTime dateReturn, int categoryId, int reservationId = 0)
        {
            var carId = 0;
            var objRes = new ReservationManager();
            var objCar = new CarManager();
            var listR = objRes.ListReservations();
            var listC = objCar.ListCars();
            //Create a temporary table with all necessaries columns
            var table = (from r in listR
                         select
                         new
                         {
                             r.DateStartRent,
                             r.DateReturnRent,
                             r.CarId,
                             r.ReservationId,
                             CategoryId = (from c in listC where c.CarId == r.CarId select (c.CategoryId)).First()
                         });

            //Verify the categories in reservation according to dates and category
            var query = (from c in listC
                         where c.CategoryId == categoryId && !(from r in table
                                                               where (((dateDeparture >= r.DateStartRent && dateDeparture <= r.DateReturnRent) ||
                                                               (dateReturn >= r.DateStartRent && dateReturn <= r.DateReturnRent)) ||
                                                               ((r.DateStartRent >= dateDeparture && r.DateStartRent <= dateReturn) ||
                                                               (r.DateReturnRent >= dateDeparture && r.DateReturnRent <= dateReturn)) &&
                                                               (r.CategoryId == categoryId || categoryId == 0)) &&
                                                               (r.ReservationId != reservationId || reservationId == 0)
                                                               select r.CarId).Contains(c.CarId)
                         select new { c.CarId }).FirstOrDefault();

            if (query != null)
            {
                carId = query.CarId;
            }

            return carId;
        }

        /// <summary>
        /// Auter: Marcos Muranaka
        /// Description: Cette fonction vérifie si une catégorie spécifique est encore disponible pour la réservation
        /// Cette fonction doit être appelée avant de confirmer (enregistrer) la réservation pour s'assurer que la catégorie est encore disponible
        /// Ça se peut que entre le temps de consulter, sélectionner et réserver, la catégorie n'est plus disponible
        /// </summary>
        /// <param name="dateDeparture">Date de sortie</param>
        /// <param name="dateReturn">Date de retour</param>
        /// <param name="categoryId">ID de la catégorie</param>
        /// <returns>Retourne TRUE si disponible / FALSE si pas disponible</returns>
        public bool CheckAvailableCategory(DateTime dateDeparture, DateTime dateReturn, int categoryId, int reservationId = 0)
        {
            var list = SearchAvailableCategories(dateDeparture, dateReturn, categoryId, reservationId);
            return list.Any();
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction retourne la quantité existante des véhicules par catégorie
        /// </summary>
        /// <param name="categoryId">ID de la catégorie</param>
        /// <returns>Retourne la quantité de véhicules</returns>
        private int GetCountCars(int categoryId)
        {
            var obj = new CarManager();
            var list = obj.ListCarsByCategory(categoryId);
            return list.Count();
        }
    }
}