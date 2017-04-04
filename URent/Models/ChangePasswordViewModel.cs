using System.ComponentModel.DataAnnotations;

namespace URent.Models
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 03/04/2017
    /// Description: Propriétés du client pour modifier le mot de passe
    /// </summary>
    public class ChangePasswordViewModel
    {
        public int ClientId { get; set; }

        [Required]
        [Display(Name = "Current Password")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm New Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}