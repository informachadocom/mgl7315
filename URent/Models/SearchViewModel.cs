using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using URent.Models.Model;

namespace URent.Models
{
    public class SearchViewModel
    {
        [Required]
        [Display(Name = "Date Departure")]
        public DateTime? DateDeparture { get; set; }
        [Required]
        [Display(Name = "Date Return")]
        public DateTime? DateReturn { get; set; }
        [Display(Name = "Car Category")]
        public IList<Category> ListCategory { get; set; }
        [Display(Name = "Options")]
        public IList<Option> ListOption { get; set; }
        public int CategoryId { get; set; }
        public int[] SelectedOptions { get; set; }
    }
}