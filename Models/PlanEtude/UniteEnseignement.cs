using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;


namespace Projet_PFE.Models
{
    public class UniteEnseignement
    {
        public long IdUniteEnseignement { get; set; }
        public long IdParcours { get; set; }
        public string IntituleFrParcours { get; set; }
        public long IdNiveau { get; set; }
        public string  IntituleFrNiveau { get; set; }
        [Remote(action: "VerifyCode", controller: "UEnseignement", AdditionalFields = "OldCode")]
        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public string IntituleFr { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public string IntituleAr { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public string IntituleAbrg { get; set; }
        public string Nature { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public float? Credits { get; set; }
        
        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public float? Coefficient { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public int? Periode { get; set; }
        public string OldCode { get; set; }
        public UniteEnseignement() 
        { }

        public UniteEnseignement(int pPeriode, string pCode, string pIntituleFr, string pIntituleAr, string pIntituleAbrg, string pNature, float pCredits, float pCoefficient)
        {
            this.Periode = pPeriode; 
            this.Code = pCode;
            this.IntituleFr = pIntituleFr;
            this.IntituleAr = pIntituleAr;
            this.IntituleAbrg = pIntituleAbrg;
            this.Nature = pNature;
            this.Credits = pCredits;
            this.Coefficient = pCoefficient;
        }
    }
    
    public class UniteEModuleViewModel
    {

        public Module NewModule { get; set; }
        public UniteEnseignement NewUniteE { get; set; }
    }
}