using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 
namespace Projet_PFE.Models
{
    [IgnoreAntiforgeryToken(Order = 2000)]
    public class Filiere
    {
        public string CodeTypeDiplome { get; set; }
        public string CodeDepartement { get; set; }
        public string CodeTypePeriode { get; set; }


        public long Id { get; set; }
        
        [Required(ErrorMessage="Champ obligatoire non Saisi")]
        public string Code { get; set; }
        
        [Required(ErrorMessage="Champ obligatoire non Saisi")]
        public string IntituleFr { get; set; }
        
        [Required(ErrorMessage="Champ obligatoire non Saisi")]
        public string IntituleAr { get; set; }
        
        [Required(ErrorMessage="Champ obligatoire non Saisi")]
        public string IntituleAbrg { get; set; }
        
        [Required(ErrorMessage="Champ obligatoire non Saisi")]
        public string Domaine { get; set; }
        
        [Required(ErrorMessage="Champ obligatoire non Saisi")]
        public string Mention { get; set; }
        
        [Required(ErrorMessage="Champ obligatoire non Saisi")]
        public string PeriodeHabilitation { get; set; }

        [Required(ErrorMessage="Champ obligatoire non Saisi")]
        public int? NbPeriodes { get; set; }
        public string IntituleFrDepartement { get; set; }
        public string IntituleFrTypeDiplome { get; set; }
        public string TypePeriode { get; set; }
        
        public Filiere()
        { }

        public Filiere(string pCode, string pIntituleFr, string pIntituleAr, string pIntituleAbrg, string pDomaine, string pMention)
        {
            this.Code=pCode;
            this.IntituleFr=pIntituleFr;
            this.IntituleAr=pIntituleAr;
            this.IntituleAbrg = pIntituleAbrg;
            this.Domaine=pDomaine;
            this.Mention = pMention;
        }

    }
    public class FiliereViewModel
    {
        public List<Filiere> Filieres { get; set; }
        public List<Departement> Departements { get; set; }
        public List<TypeDiplome> TypeDiplomes { get; set; }

        public List<TypePeriode> Periodes;
        public Filiere Flre { get; set; }
    }
    public class FiliereNiveauParcoursViewModel
    {
        public Filiere Flre { get; set; }
        public List<Departement> Departements { get; set; }
        public List<TypeDiplome> TypeDiplomes { get; set; }
         public List<TypePeriode> Periodes;
        public List<Niveau> Niveaux { get; set; }
        public List<Parcours> Parcours { get; set; }
        public Parcours Pars {get; set;}
        
    }
  
}