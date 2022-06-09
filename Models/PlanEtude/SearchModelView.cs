using System.Collections.Generic;

namespace Projet_PFE.Models
{
    public class SearchModelView
    {
        public List<Filiere> ListFilieres { get; set; }

        //Information Relatives à une Filiere
        public List<Niveau> ListNiveaux { get; set; }
        public List<Parcours> ListParcours { get; set; }

        public string CodeAnneuniv { get; set; }

        public AnneeUniversitaireNiveauParcoursPeriode NewNiveauParcours { get; set; }

        //Information Relatives à un Niveau Parcours
        
        public List <TypePeriode> ListTypePeriodes{ get; set; }
        public string CodeFiliere { get; set; }
        public string CodeParcours {get;set;}
        public string  CodeNiveau {get;set;}
        public string CodeTypePeriode {get;set;}

    }
}