using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Projet_PFE.Models;
using Projet_PFE.MyUtilities;
using Projet_PFE.Dal;

namespace Projet_PFE.Bll
{
    public class Bll_AnneeUniversitaireNiveauParcoursPeriode
    {
        public static void Add(AnneeUniversitaireNiveauParcoursPeriode AnneeUniNivPars) => Dal_AnneeUniversitaireNiveauParcoursPeriode.Add(AnneeUniNivPars);
        
        public static void Delete(long Id) => Dal_AnneeUniversitaireNiveauParcoursPeriode.Delete(Id);

        public static void Update(AnneeUniversitaireNiveauParcoursPeriode AnneeUniNivPars) => Dal_AnneeUniversitaireNiveauParcoursPeriode.Update(AnneeUniNivPars);

        public static List <AnneeUniversitaireNiveauParcoursPeriode> SelectAll(
            long IdFiliere,
            string CodeAnneeUniv) => Dal_AnneeUniversitaireNiveauParcoursPeriode.SelectAll(IdFiliere, CodeAnneeUniv);
        
        public static AnneeUniversitaireNiveauParcoursPeriode SelectById(long Id) => Dal_AnneeUniversitaireNiveauParcoursPeriode.SelectById(Id);

        public static AnneeUniversitaireNiveauParcoursPeriode SelectByAnneeUnivNiv(
            long IdFiliere,
            long IdNiveau,
            long IdParcours,
            string CodeAnneeUniv) => Dal_AnneeUniversitaireNiveauParcoursPeriode.SelectByAnneeUnivNiv(IdFiliere, IdNiveau, IdParcours, CodeAnneeUniv);
        
        public static bool CheckAnneeUnivNiveauParcours(
            long IdFiliere,
            long IdNiveau,
            long IdParcours,
            string CodeAnneeUniv) => Dal_AnneeUniversitaireNiveauParcoursPeriode.CheckEntityUnicity(IdFiliere, IdNiveau, IdParcours, CodeAnneeUniv);
        
    }
}