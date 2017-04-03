using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace URent.Models
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 03/04/2017
    /// Description: Propriétés du client pour modifier
    /// </summary>
    public class UpdateClientViewModel
    {
        public int ClientId { get; set; }
        [Required]
        [Display(Name = "Surname")]
        public string Surname { get; set; }
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}