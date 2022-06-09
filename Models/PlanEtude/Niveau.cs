using System.ComponentModel.DataAnnotations;

namespace Projet_PFE.Models
{
    public class Niveau
    {
        public long Id { get; set; }
        public long IdFiliere { get; set; }
        [Required(ErrorMessage="Champ Obligatoire non Saisi")]
        public string Code { get; set; }
        [Required(ErrorMessage="Champ Obligatoire non Saisi")]
        public string IntituleFr { get; set; }
        [Required(ErrorMessage="Champ Obligatoire non Saisi")]
        public string IntituleAr { get; set; }
        [Required(ErrorMessage="Champ Obligatoire non Saisi")]
        public string IntituleAbrg { get; set; }
        public Niveau() 
        { }

        public Niveau(string pCode, string pIntituleFr, string pIntituleAr, string pIntituleAbrg) 
        {
            this.Code=pCode;
            this.IntituleFr=pIntituleFr;
            this.IntituleAr=pIntituleAr;
            this.IntituleAbrg = pIntituleAbrg;
        } 
    }
}