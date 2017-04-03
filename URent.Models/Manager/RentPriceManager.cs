using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using URent.Models.Interfaces;
using URent.Models.Model;
using URent.Models.Util;

namespace URent.Models.Manager
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 28/03/2017
    /// Description: Cette classe est responsable de gérer les tarifications de location
    /// </summary>
    public class RentPriceManager : IRentPrice
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
                var list = new List<RentPrice>();
                var rent = new RentPrice { RentPriceId = 1, CategoryId = 1, Price = (decimal)20.5, SaleDay = EnumDays.Days.Monday };
                list.Add(rent);
                rent = new RentPrice { RentPriceId = 2, CategoryId = 1, Price = (decimal)20.5, SaleDay = EnumDays.Days.Tuesday };
                list.Add(rent);
                rent = new RentPrice { RentPriceId = 3, CategoryId = 1, Price = (decimal)20.5, SaleDay = EnumDays.Days.Wednesday };
                list.Add(rent);
                rent = new RentPrice { RentPriceId = 4, CategoryId = 1, Price = (decimal)20.5, SaleDay = EnumDays.Days.Thursday };
                list.Add(rent);
                rent = new RentPrice { RentPriceId = 5, CategoryId = 1, Price = 25, SaleDay = EnumDays.Days.Friday };
                list.Add(rent);
                rent = new RentPrice { RentPriceId = 6, CategoryId = 1, Price = (decimal)30.5, SaleDay = EnumDays.Days.Saturday };
                list.Add(rent);
                rent = new RentPrice { RentPriceId = 7, CategoryId = 1, Price = (decimal)30.5, SaleDay = EnumDays.Days.Sunday };
                list.Add(rent);
                rent = new RentPrice { RentPriceId = 8, CategoryId = 1, Price = 40, SaleDate = new DateTime(2017, 12, 25) };
                list.Add(rent);
                //================================================================================================================
                rent = new RentPrice { RentPriceId = 1, CategoryId = 2, Price = (decimal)22.5, SaleDay = EnumDays.Days.Monday };
                list.Add(rent);
                rent = new RentPrice { RentPriceId = 2, CategoryId = 2, Price = (decimal)22.5, SaleDay = EnumDays.Days.Tuesday };
                list.Add(rent);
                rent = new RentPrice { RentPriceId = 3, CategoryId = 2, Price = (decimal)22.5, SaleDay = EnumDays.Days.Wednesday };
                list.Add(rent);
                rent = new RentPrice { RentPriceId = 4, CategoryId = 2, Price = (decimal)22.5, SaleDay = EnumDays.Days.Thursday };
                list.Add(rent);
                rent = new RentPrice { RentPriceId = 5, CategoryId = 2, Price = 27, SaleDay = EnumDays.Days.Friday };
                list.Add(rent);
                rent = new RentPrice { RentPriceId = 6, CategoryId = 2, Price = (decimal)35.5, SaleDay = EnumDays.Days.Saturday };
                list.Add(rent);
                rent = new RentPrice { RentPriceId = 7, CategoryId = 2, Price = (decimal)32.5, SaleDay = EnumDays.Days.Sunday };
                list.Add(rent);
                rent = new RentPrice { RentPriceId = 8, CategoryId = 2, Price = 42, SaleDate = new DateTime(2017, 12, 25) };
                list.Add(rent);
                //================================================================================================================
                rent = new RentPrice { RentPriceId = 1, CategoryId = 3, Price = (decimal)25.5, SaleDay = EnumDays.Days.Monday };
                list.Add(rent);
                rent = new RentPrice { RentPriceId = 2, CategoryId = 3, Price = (decimal)25.5, SaleDay = EnumDays.Days.Tuesday };
                list.Add(rent);
                rent = new RentPrice { RentPriceId = 3, CategoryId = 3, Price = (decimal)25.5, SaleDay = EnumDays.Days.Wednesday };
                list.Add(rent);
                rent = new RentPrice { RentPriceId = 4, CategoryId = 3, Price = (decimal)25.5, SaleDay = EnumDays.Days.Thursday };
                list.Add(rent);
                rent = new RentPrice { RentPriceId = 5, CategoryId = 3, Price = 30, SaleDay = EnumDays.Days.Friday };
                list.Add(rent);
                rent = new RentPrice { RentPriceId = 6, CategoryId = 3, Price = (decimal)40.5, SaleDay = EnumDays.Days.Saturday };
                list.Add(rent);
                rent = new RentPrice { RentPriceId = 7, CategoryId = 3, Price = (decimal)38.5, SaleDay = EnumDays.Days.Sunday };
                list.Add(rent);
                rent = new RentPrice { RentPriceId = 8, CategoryId = 3, Price = 49, SaleDate = new DateTime(2017, 12, 25) };
                list.Add(rent);
                var json = JsonConvert.SerializeObject(list);
                Helper.CreateJson("RentPrice", json);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction lit le fichier Json avec les données des prix de location enregistrées
        /// </summary>
        /// <returns>Retourne une liste des prix de location</returns>
        private IList<RentPrice> ReadRentPrice()
        {
            return JsonConvert.DeserializeObject<List<RentPrice>>(Helper.ReadJson("RentPrice"));
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction prend la valeur selon la catégorie et la date d'entrée
        /// En premier la fonction va chercher la valeur par la date spécifique (SaleDate)
        /// Sinon elle prend la valeur du jour de la semaine (SaleDay)
        /// </summary>
        /// <param name="date">Date de location</param>
        /// <param name="categoryId">ID de la catégorie</param>
        /// <returns>Retourne le prix pour la date et catégorie spécifiées</returns>
        public decimal GetPrice(DateTime date, int categoryId)
        {
            var list = ReadRentPrice();
            var price = list?.Where(c => c.CategoryId == categoryId && c.SaleDate == date).Select(c => c.Price).ToList();
            if (price.Count == 0)
            {
                var day = (int)date.DayOfWeek;
                price = list?.Where(c => { return c.SaleDay != null && (c.CategoryId == categoryId && (int)c.SaleDay == day); }).Select(c => c.Price).ToList();
            }
            return price != null && price.Count > 0 ? price[0] : 0;
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction calcule le total et la valeur pour chaque jour
        /// </summary>
        /// <param name="categories">Liste de catégories disponibles</param>
        /// <param name="departureDate">Date de sortie</param>
        /// <param name="returnDate">Date de retour</param>
        /// <returns>Retourne une liste de catégories avec les prix calculés</returns>
        public IList<Order> CalculatePriceSearch(IList<Category> categories, DateTime departureDate, DateTime returnDate)
        {
            var list = new List<Order>();

            foreach (var c in categories)
            {
                decimal total = 0;
                var objOrder = new Order();
                var listPrice = new List<OrderPrice>();
                objOrder.Category = c;
                for (var currentDate = departureDate; currentDate <= returnDate; currentDate = currentDate.AddDays(1))
                {
                    var objPrice = new OrderPrice();
                    var price = GetPrice(currentDate, c.CategoryId);
                    total += price;
                    objPrice.Price = price;
                    objPrice.RentDate = currentDate;
                    listPrice.Add(objPrice);

                }
                objOrder.TotalCar = total;
                objOrder.Total = total;
                objOrder.OrderPrice = listPrice;
                objOrder.DateDeparture = departureDate;
                objOrder.DateReturn = returnDate;
                list.Add(objOrder);
            }
            return list;
        }

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction calculate the values for the order
        /// </summary>
        /// <param name="orderPrice">Liste avec items choisis</param>
        /// <returns>Retourne la liste avec les totals calculés</returns>
        public IList<Order> RecalculatePriceSearch(IList<Order> orderPrice)
        {
            if (orderPrice != null)
            {
                foreach (var o in orderPrice)
                {
                    decimal totalCar = 0;
                    if (o.OrderPrice != null)
                    {
                        foreach (var car in o.OrderPrice)
                        {
                            totalCar += car.Price;
                        }
                    }

                    decimal totalOption = 0;
                    if (o.Options != null)
                    {
                        var days = o.DateReturn.Subtract(o.DateDeparture).TotalDays;
                        foreach (var op in o.Options)
                        {
                            totalOption += op.Price * (decimal)days;
                        }
                    }
                    o.TotalOption = totalOption;
                    o.Total = o.TotalCar + o.TotalOption;
                }
            }
            return orderPrice;
        }

        //public IList<Model.RentPrice> ListRentPrices()
        //{
        //    return ReadRentPrice();
        //}

        //public IList<Model.RentPrice> ListRentPrice(int id)
        //{
        //    var list = ReadRentPrice();
        //    return list.Where(c => c.RentPriceId == id).ToList();
        //}

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction
        /// </summary>
        /// <param name="rent">L'objet du prix à creer ou modifier</param>
        //public void CreateUpdate(Model.RentPrice rent)
        //{
        //    var list = (List<Model.RentPrice>)ReadRentPrice();
        //    if (rent.RentPriceId > 0)
        //    {
        //        //Remove RentPrice to edit
        //        list.RemoveAll(u => u.RentPriceId == rent.RentPriceId);
        //    }
        //    else
        //    {
        //        //If new RentPrice, we add the max RentPriceId + 1
        //        rent.RentPriceId = list.Max(u => u.RentPriceId) + 1;
        //    }
        //    list.Add(rent);
        //    Generate(list);
        //}

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction efface le prix
        /// </summary>
        /// <param name="rent"></param>
        //public void Remove(Model.RentPrice rent)
        //{
        //    var list = (List<Model.RentPrice>)ReadRentPrice();
        //    list.RemoveAll(u => u.RentPriceId == rent.RentPriceId);
        //    Generate(list);
        //}

        /// <summary>
        /// Auteur: Marcos Muranaka
        /// Description: Cette fonction génère un fichier Json avec des la liste des objets des prix de location
        /// </summary>
        /// <param name="rents">Liste des prix de location</param>
        //private void Generate(List<Model.RentPrice> rents)
        //{
        //    var json = JsonConvert.SerializeObject(rents);
        //    Util.CreateJson("RentPrice", json);
        //}

    }
}