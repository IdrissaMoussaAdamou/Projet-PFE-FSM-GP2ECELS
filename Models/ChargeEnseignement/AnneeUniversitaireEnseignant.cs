using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
namespace Projet_PFE.Models
{
    public class AnneeUniversitaireEnseignant
    {
        public long Id { get; set; }

        [Remote(action: "VerifyCIN", controller: "ChargeEnseignement")]
        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public string CIN { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public string Nom  { get; set; }
  
        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public string Prenom { get; set; }
        public string Grade { get; set; }
        public string ValidationChargeDepartement { get; set; }
        public DateTime? DateValidationChargeDepartement { get; set; }
        public string ValidationChargeAdministration { get; set; }
        public DateTime? DateValidationChargeAdminsitration { get; set; }
        public string EtatSaisie {get; set; }
        public string Statut { get; set; }
        public  string CodeDepartement { get; set; }
        public string IntituleFrDepartement { get; set; }
        public  string CodeAnneeUniv { get; set; }

        public AnneeUniversitaireEnseignant()
        {}
        public AnneeUniversitaireEnseignant(ImportEnseignant pEnseignant, string pCodeAnneeUniv)
        {
            CIN = pEnseignant.CIN;
            Nom = pEnseignant.Nom;
            Prenom = pEnseignant.Prenom;
            Grade = pEnseignant.Grade;
            Statut = pEnseignant.Statut;
            CodeDepartement = pEnseignant.CodeDepartement;
            CodeAnneeUniv = pCodeAnneeUniv;
            IntituleFrDepartement = pEnseignant.IntituleFrDepartement;
        }
    }
}