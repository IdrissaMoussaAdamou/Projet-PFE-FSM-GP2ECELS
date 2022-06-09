using System.ComponentModel.DataAnnotations; 
using Microsoft.AspNetCore.Mvc;
namespace Projet_PFE.Models
{
    public class AnneeUniversitaireNiveauParcoursPeriode
    {
       public long Id { get; set; } 
       public string CodeAnneeUniv { get; set; }
       [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
       public int? NbGroupesC { get; set; }

       [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
       public int? NbGroupesTD { get; set; }

       [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
       public int? NbGroupesTP { get; set; }

       [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
       public int? NbGroupesCI { get; set; }

       [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
       public int? NbEtudiants { get; set; }
       public long IdFiliere { get; set; }
       public long IdNiveau { get; set; }
       [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
       public int? Periode { get; set; }
       public string IntituleFrNiveau { get; set; }
       public string IntituleAbrgNiveau { get; set; }    
       public long IdParcours { get; set; }
       public string IntituleFrParcours { get; set; }
      public AnneeUniversitaireNiveauParcoursPeriode()
      {

      }
      public AnneeUniversitaireNiveauParcoursPeriode(string pCodeAnneeUniv, int pNbGroupesC, int pNbGroupesTD, int pNbGroupesTP, int pNbGroupesCI, int pNbEtudiants, long pIdFiliere, long pIdNiveau, long pIdParcours, int pPeriode)
      {
          this.CodeAnneeUniv = pCodeAnneeUniv;
          this.NbGroupesC = pNbGroupesC;
          this.NbGroupesTD = pNbGroupesTD;
          this.NbGroupesTP = pNbGroupesTP;
          this.NbGroupesCI = pNbGroupesCI;
          this.NbEtudiants = pNbEtudiants;
          this.IdFiliere = pIdFiliere;
          this.IdNiveau = pIdNiveau;
          this.IdParcours = pIdParcours; 
          this.Periode = pPeriode;
      }
    }
}