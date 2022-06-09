using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Projet_PFE.Models
{
    public class TypePeriode
    {
        [Remote(action: "VerifyCode", controller: "TypePeriode", AdditionalFields = "pCode")]
        [Required(ErrorMessage="Champ Obligatoire non Saisi")]
        public string Code { get; set; }
        [Required(ErrorMessage="Champ Obligatoire non Saisi")]
        public string IntituleFr { get; set; }
        [Required(ErrorMessage="Champ Obligatoire non Saisi")]
        public string IntituleAr { get; set; }
        [Required(ErrorMessage="Champ Obligatoire non Saisi")]
        public string IntituleAbrg { get; set; }
        [Required(ErrorMessage="Champ Obligatoire non Saisi")]
        public string Type { get; set; }

        [Required(ErrorMessage="Champ Obligatoire non Saisi")]
        public float? Duree { get; set; }
       
        public TypePeriode()
        { }

        public TypePeriode(string pCode, string pIntituleFr, string pIntituleAr, string pIntituleAbrg, string pType, float pDuree)
        {
            this.Code = pCode;
            this.IntituleFr = pIntituleFr;
            this.IntituleAr = pIntituleAr;
            this.IntituleAbrg = pIntituleAbrg;
            this.Type = pType;
            this.Duree = pDuree;
        }
    }

    public class TypePeriodeCoursesModelView
    {
        public TypePeriode PerD { get; set; }

        public List<Module> Modules { get; set; }
        public List<UniteEnseignement> Unites { get; set; }

    }
}