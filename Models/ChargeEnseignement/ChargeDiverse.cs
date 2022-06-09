using System.ComponentModel.DataAnnotations;

namespace Projet_PFE.Models
{
    public class ChargeDiverse
    {
        public long Id  { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire")]
        public string Periode { get; set; }
        public int NumPeriodeDansAnnee { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire")]
        public float? NbSemainesPeriode { get; set; }
        public string NatureCharge { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire Non Saisi")]
        public float? Volume  { get; set; }
        public string Observations  { get; set; }
        public string UniteVolume { get; set; }
        public string CodeAnneeUniv { get; set; }
        public long IdAUEnseignant { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire")]
        public long IdTypeChargeDiverse { get; set; }
        
        public ChargeDiverse()
        {

        }
    }
}