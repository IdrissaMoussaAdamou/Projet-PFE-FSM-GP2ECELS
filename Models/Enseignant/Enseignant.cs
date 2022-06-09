using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
namespace Projet_PFE.Models
{
    public class Enseignant
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Champs Obligatoire non saisi")]
        [Remote(action: "VerifyCIN", controller: "Enseignant", AdditionalFields = "OldCIN")]
        public string CIN { get; set; }
        [Required(ErrorMessage = "Champs Obligatoire non saisi")]

        public string Nom  { get; set; }
        [Required(ErrorMessage = "Champs Obligatoire non saisi")]

        public string Prenom { get; set; }
        public  string CodeDepartement { get; set; }
        public string Grade { get; set; }
        public string Statut { get; set; }
        public string SituationAdministrative { get; set; }

        [Required(ErrorMessage = "Champs Obligatoire non saisi")]
        [EmailAddress(ErrorMessage = "Format non valide")]
        public string Email1 { get; set; }
        public string Email2 { get; set; }

        [Required(ErrorMessage = "Champs Obligatoire non saisi")]
        public string Telephone1 { get; set; }
        
        public string Telephone2 { get; set; }

        public string IntituleFrDepartement { get; set; }
        public Enseignant()
        {}

        public Enseignant(string pCIN, string pNom, string pPrenom, string pCodeDepartement, string pGrade, string pStatut, string pSituationAdministrative, string pEmail1, string pEmail2, string pTelephone1, string pTelephone2)
        {
            this.CIN = pCIN;
            this.Nom = pNom;
            this.Prenom = pPrenom;
            this.CodeDepartement = pCodeDepartement;
            this.Grade = pGrade;
            this.Statut = pStatut;
            this.SituationAdministrative = pSituationAdministrative;
            this.Email1 = pEmail1;
            this.Email2 = pEmail2;
            this.Telephone1 = pTelephone1;
            this.Telephone2 = pTelephone2;
        }

    }
}