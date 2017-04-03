using System.ComponentModel.DataAnnotations;

namespace URent.Models
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 03/04/2017
    /// Description: Propriétés du client pour modifier
    /// </summary>
    public class ClientViewModel : UpdateClientViewModel
    {
        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}