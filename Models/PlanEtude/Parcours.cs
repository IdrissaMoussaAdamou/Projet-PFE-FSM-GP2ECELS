using System.ComponentModel.DataAnnotations;

namespace Projet_PFE.Models
{
    public class Parcours
    {

        public long IdFiliere { get; set; }
        public long Id { get; set; }

        [Required(ErrorMessage="Champ Obligatoire non Saisi")]
        public string Code { get; set; }
        
        [Required(ErrorMessage="Champ Obligatoire non Saisi")]
        public string IntituleFr { get; set; }
        
        [Required(ErrorMessage="Champ Obligatoire non Saisi")]
        public string IntituleAr { get; set; }
        
        [Required(ErrorMessage="Champ Obligatoire non Saisi")]
        public string IntituleAbrg { get; set; }
        
        [Required(ErrorMessage="Champ Obligatoire non Saisi")]
        public string PeriodeHabilitation { get; set; }

        [Required(ErrorMessage="Champ Obligatoire non Saisi")]
        public int? PeriodeDebut { get; set; }
        
        [Required(ErrorMessage="Champ Obligatoire non Saisi")]
        public int? PeriodeFin { get; set; }
        
        public Parcours() 
        { }

        public Parcours(string pCode, string pIntituleFr, string pIntituleAr, string pIntituleAbrg, long pIdFiliere)
        {
            this.Code = pCode;
            this.IntituleFr = pIntituleFr;
            this.IntituleAr = pIntituleAr;
            this.IntituleAbrg = pIntituleAbrg;
            this.IdFiliere = pIdFiliere;
        }

    }
}