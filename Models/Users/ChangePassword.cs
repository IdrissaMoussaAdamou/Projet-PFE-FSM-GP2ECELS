using System.ComponentModel.DataAnnotations;

namespace Projet_PFE.Models
{
    public class ChangePassword
    {
        public long Iduser { get; set; }
        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        [Compare("NewPassword", ErrorMessage = "Confirmation Invalide")]
        public string ConfirmNewPassword { get; set; }

        public ChangePassword()
        {}
    }
}