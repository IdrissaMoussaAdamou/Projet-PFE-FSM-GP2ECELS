using System.Collections.Generic;

namespace Projet_PFE.Models
{
    public class PlanEtude
    {
        public List<Filiere> ListFilieres { get; set; }

        //Information Relatives à une Filiere
        public List<Niveau> ListNiveaux { get; set; }
        public List<Parcours> ListParcours { get; set; }

        //Information Relatives à un Niveau Parcours
        public List<Module> ListModules { get; set; }
        public List<UniteEnseignement> ListUnites { get; set; }

        public List <TypePeriode> ListTypePeriodes{ get; set; }
        public long IdFiliere { get; set; }
        public long IdParcours {get;set;}
        public long  IdNiveau {get;set;}
        public int Periode {get;set;}

        //Module et Unite D'Enseignement d'un Niveau - Parcours
        public Module NewModule { get; set; }
        public UniteEnseignement NewUniteE { get; set; }
    }
}