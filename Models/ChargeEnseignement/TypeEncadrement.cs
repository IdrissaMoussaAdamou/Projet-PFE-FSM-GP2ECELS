using System.ComponentModel.DataAnnotations;
namespace Projet_PFE.Models
{
    public class TypeEncadrement
    {
        public long Id  { get; set; }
        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public string Libelle { get; set; }
        public string Cycle  { get; set; }
        public string NatureCharge { get; set; }
        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]        
        public float VolumeHebdoCharge  { get; set; }
        public string Periode { get; set; }
        public int NumPeriodeDansAnnee { get; set; }
        public float? NbSemainesPeriode { get; set; }
        public TypeEncadrement()
        {}
    }
}