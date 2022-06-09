using System.ComponentModel.DataAnnotations;

namespace Projet_PFE.Models
{
    public class ChargeParModule
    {   
        public long Id { get; set; }
        public string Periode { get; set; }
        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public float? VolumeHebdoParGroupe { get; set; }
   
        public int NumPeriodeDansAnnee { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public float? NbSemainesPeriode { get; set; }
        public string NatureEnseignement { get; set; }
        public string TypeCalcul { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]

        public int? NbGroupes { get; set; }
        public float VolumeTotal { get; set; }
        public long IdFiliere { get; set; }

        [Required(ErrorMessage ="Champ Obligatoire")]
        public long IdNiveau { get; set; }
        public string IntituleAbrgNiveau { get; set; }
        
        [Required(ErrorMessage ="Champ Obligatoire")]
        public long IdModule { get; set; }
        public string IntituleFrModule { get; set; }
        public string CodeAnneeUniv { get; set; }
        public long IdAUEnseignant { get; set; }
               
         public ChargeParModule()
        {}
    }
}