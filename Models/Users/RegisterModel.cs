using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
namespace Projet_PFE.Models
{
    public class RegisterModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        [Remote(action: "VerifyCINUnicity", controller: "Auth", AdditionalFields = "OldCIN")]
        public string CIN { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        [EmailAddress(ErrorMessage = "Format Invalide")]
        [Remote(action: "CheckEmailUnicity", controller: "Auth", AdditionalFields = "OldEmail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        [Compare("Password", ErrorMessage = "Confirmation Invalide")]
        public string ConfirmPassword { get; set; }
        public string Profil { get; set; }
        
        [Required(ErrorMessage = "Champ Obligatoire")]
        public string Affiliation { get; set; }
        
        public RegisterModel()
        {}

        public RegisterModel( AnneeUniversitaireEnseignant Teacher, string pEmail, string pPassword)
        {
            this.Nom = Teacher.Nom;
            this.Prenom = Teacher.Prenom;
            this.Profil = "Enseignant";
            this.Affiliation = "Dept" + Teacher.IntituleFrDepartement;
            this.CIN = Teacher.CIN;
            this.Email = pEmail;
            this.Password = pPassword;
        }
        
    }
}