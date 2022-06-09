using System;
using System.ComponentModel.DataAnnotations;
namespace Projet_PFE.Models
{
    public class ImportEnseignant
    {

        public string CIN { get; set; }
        public string Nom  { get; set; }
        public string Prenom { get; set; }
        public  string CodeDepartement { get; set; }
        public string Grade { get; set; }
        public string Statut { get; set; }
        public string IntituleFrDepartement { get; set; }
        public bool Selected { get; set; }
        public ImportEnseignant()
        {}

        public ImportEnseignant(Enseignant pEnseignant)
        {
            CIN = pEnseignant.CIN;
            Nom = pEnseignant.Nom;
            Prenom = pEnseignant.Prenom;
            Grade = pEnseignant.Grade;
            Statut = pEnseignant.Statut;
            CodeDepartement = pEnseignant.CodeDepartement;
            IntituleFrDepartement = pEnseignant.IntituleFrDepartement;
        }

    }
}