using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projet_PFE.Models;
using Projet_PFE.Dal;

namespace Projet_PFE.Bll
{
    public class Bll_AnneeUniversitaireNomOption
    {
        public static void Add(NomOption Option) => Dal_AnneeUniversitaireNomOption.Add(Option);
        
        public static void Update(NomOption Option) => Dal_AnneeUniversitaireNomOption.Update(Option);

        public static List<NomOption> SelectAll(
            long IdNiveau,
            long IdParcours,
            int Periode,
            string CodeAnneeUniv) => Dal_AnneeUniversitaireNomOption.SelectAll(IdNiveau, IdParcours, Periode, CodeAnneeUniv);
        public static NomOption SelectModuleOption(
            long IdModule,
            string CodeAnneeUniv) => Dal_AnneeUniversitaireNomOption.SelectModuleNomOption(IdModule, CodeAnneeUniv);
        
    }
}