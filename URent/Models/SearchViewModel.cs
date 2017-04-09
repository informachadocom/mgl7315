using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using URent.Models.Model;

namespace URent.Models
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 01/04/2017
    /// Description: Propriétés de la recherche
    /// </summary>
    public class SearchViewModel
    {
        [Required]
        [Display(Name = "Date Departure")]
        public DateTime? DateDeparture { get; set; }
        public IList<string> ListTimeDeparture { get; set; }
        public string TimeDeparture { get; set; }
        [Required]
        [Display(Name = "Date Return")]
        public DateTime? DateReturn { get; set; }
        public IList<string> ListTimeReturn { get; set; }
        public string TimeReturn { get; set; }
        [Display(Name = "Car Category")]
        public IList<Category> ListCategory { get; set; }
        [Display(Name = "Options")]
        public IList<Option> ListOption { get; set; }
        public int CategoryId { get; set; }
        public int[] SelectedOptions { get; set; }
        [Display(Name = "Client")]
        public IList<Client> ListClient { get; set; }
        public int ClientId { get; set; }
        public int ReservationId { get; set; }
    }
}