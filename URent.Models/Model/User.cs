using System.ComponentModel.DataAnnotations;

namespace URent.Models.Model
{
    /// <summary>
    /// Auteur: Marcos Muranaka
    /// Date: 09/04/2017
    /// Description: Propriétés du usager (admin)
    /// </summary>
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}