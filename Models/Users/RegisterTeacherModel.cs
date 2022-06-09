using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
namespace Projet_PFE.Models
{
    public class RegisterTeacherModel
    {

        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        [Remote(action: "VerifyCIN", controller: "Auth")]
        public string CIN { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        [EmailAddress(ErrorMessage = "Format Invalide")]
        [Remote(action: "CheckEmailUnicity", controller: "Auth", AdditionalFields = "OldEmail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public string Password { get; set; }

        [Compare("Password",ErrorMessage = "Invalide")]
        public string ConfirmPassword { get; set; }
        
    }
}