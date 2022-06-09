using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Projet_PFE.Models
{
    public class TypeDiplome
    {
        [Remote(action: "VerifyCode", controller: "TypeDiplome", AdditionalFields = "pCode")]
        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public string IntituleFr { get; set; }
        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public string IntituleAr { get; set; }
        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public string IntituleAbrg { get; set; }

        public TypeDiplome() { }

        public TypeDiplome(string pCode, string pIntituleFr, string pIntituleAr, string pIntituleAbrg)
        {
            this.Code = pCode;
            this.IntituleFr = pIntituleFr;
            this.IntituleAr = pIntituleAr;
            this.IntituleAbrg = pIntituleAbrg;
        }
    }
}