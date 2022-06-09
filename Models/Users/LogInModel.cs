using System.ComponentModel.DataAnnotations;

namespace Projet_PFE.Models
{
    public class LogInModel
    {
        [Required(ErrorMessage = "Champ Obligatoire Non Saisi")]
        [EmailAddress(ErrorMessage = "Format Invalide")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire Non Saisi")]
        public string Password { get; set; }

        public LogInModel()
        {}
    }
}