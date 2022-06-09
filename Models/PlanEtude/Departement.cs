using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace Projet_PFE.Models
{
    public class Departement
    { 
       [Remote(action: "VerifyCode", controller: "Departement", AdditionalFields = "pCode")]
        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public string IntituleFr { get; set; }

        [Required(ErrorMessage = "Champ Obligatoire non Saisi")]
        public string IntituleAr { get; set; }
        
        public Departement() 
        { }
        public Departement(string pCode, string pIntituleFr, string pIntituleAr) 
        {
            this.Code=pCode;
            this.IntituleFr = pIntituleFr;
            this.IntituleAr=pIntituleAr;
        }

    }
    
}
       







