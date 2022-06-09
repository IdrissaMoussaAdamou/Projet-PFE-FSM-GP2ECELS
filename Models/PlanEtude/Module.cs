using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Projet_PFE.Models
{
    public class Module
    {
        public long IdModule { get; set; }
        public long IdUniteEnseignement { get; set; }
        public string IntileFrUniteE { get; set; }

        [Required(ErrorMessage ="Champ Obligatoire non Saisi")]
        [Remote(action: "VerifyCode", controller: "Module", AdditionalFields = "OldCode")]
        public string Code { get; set; }

        [Required(ErrorMessage ="Champ Obligatoire non Saisi")]
        public string IntituleFr { get; set; }

        [Required(ErrorMessage ="Champ Obligatoire non Saisi")]
        public string IntituleAr { get; set; }
        
        [Required(ErrorMessage ="Champ Obligatoire non Saisi")]
        public string IntituleAbrg { get; set; }

        [Required(ErrorMessage ="Champ Obligatoire non Saisi")]
        public float? Credits { get; set; }

        [Required(ErrorMessage ="Champ Obligatoire non Saisi")]
        public float? Coefficient { get; set; }

        public string RegimeExamen { get; set; }
        
        [Required(ErrorMessage ="Champ Obligatoire non Saisi")]
        public string UniteVolumeHoraire { get; set; }

        [Required(ErrorMessage ="Champ Obligatoire non Saisi")]
        public float? NbHeuresCours { get; set; }
        
        [Required(ErrorMessage ="Champ Obligatoire non Saisi")]
        public float? NbHeuresTD { get; set; }

        [Required(ErrorMessage ="Champ Obligatoire non Saisi")]
        public float? NbHeuresTP { get; set; }

        [Required(ErrorMessage ="Champ Obligatoire non Saisi")]
        public float? NbHeuresCI { get; set; }
        public long IdNiveau { get; set; }
        public long IdParcours { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire Non Saisi")]
        public int? Periode { get; set; }
        public string IntituleFrNiveau { get; set; }
        public string IntituleFrParcours { get; set; }
        [Required(ErrorMessage ="Champ Obligatoire non Saisi")]
        public string DureeExamen { get; set; }
        public string Nature { get; set; }
        public string OldCode { get; set; }

        public Module()
        { }

        public Module(int pPeriode, int pIdUniteEnseignement, string pCode, string pIntituleFr, string pIntituleAr, string pIntituleAbrg, float pCredits, float pCoefficient, string pRegimeExamen, string pUniteVolumeHoraire, float pNbHeuresCours, float pNbHeuresTD, float pNbHeuresTP, float pNbHeuresCI) 
        {
            this.Periode = pPeriode;
            this.IdUniteEnseignement = pIdUniteEnseignement;
            this.Code = pCode;
            this.IntituleFr = pIntituleFr;
            this.IntituleAr = pIntituleAr;
            this.IntituleAbrg = pIntituleAbrg;
            this.Credits = pCredits;
            this.Coefficient = pCoefficient;
            this.RegimeExamen = pRegimeExamen;
            this.UniteVolumeHoraire = pUniteVolumeHoraire;
            this.NbHeuresCours = pNbHeuresCours;
            this.NbHeuresTD = pNbHeuresTD;
            this.NbHeuresTP = pNbHeuresTP;
            this.NbHeuresCI = pNbHeuresCI;
        }
    }
}