using System;
using System.ComponentModel.DataAnnotations; 
namespace Projet_PFE.Models
{
    public class AnneeUniversitaire
    {
       public string Code { get; set; }
       [Required(ErrorMessage="La Date de la fin de l'année Universitaire est Oblogatoire")]
       public DateTime DateDebut { get; set; }

       [Required(ErrorMessage="La Date de la fin de l'année Universitaire est Oblogatoire")]
       public DateTime DateFin { get; set; }
       public string EtatPlanEtudes { get; set; }
       public string EtatCharges { get; set; }

       
      public AnneeUniversitaire()
      {

      }
      public AnneeUniversitaire(DateTime pDateDebut, DateTime pDateFin)
      {
          this.DateDebut = pDateDebut;
          this.DateFin = pDateFin;
      }
    }
}