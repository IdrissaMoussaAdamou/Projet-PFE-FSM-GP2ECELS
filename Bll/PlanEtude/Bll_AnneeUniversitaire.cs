using System;
using System.Collections.Generic;
using Projet_PFE.Models;
using Projet_PFE.Dal;

namespace Projet_PFE.Bll
{
    public class Bll_AnneeUniversitaire
    {
        public static void Add(AnneeUniversitaire AnneeUniv) => Dal_AnneeUniversitaire.Add(AnneeUniv);
            
        public static void Delete(string Code) => Dal_AnneeUniversitaire.Delete(Code);
            
        public static void Update(string OldCode, AnneeUniversitaire AnneeUniv) => Dal_AnneeUniversitaire.Update(OldCode,AnneeUniv);

        public static List<AnneeUniversitaire> SelectAll() => Dal_AnneeUniversitaire.SelectAll();     

        public static AnneeUniversitaire SelectByCode(string Code) => Dal_AnneeUniversitaire.SelectByCode(Code);

        public static void Archive(string Code) => Dal_AnneeUniversitaire.ArchiveAnneeUniversitaire(Code); 
        public static AnneeUniversitaire SelectTNArchivedAnneeUniv() => Dal_AnneeUniversitaire.SelectTNArchivedAnneeUniv();
        
        public static void ArchiverChargeEnseignement(string Code) => Dal_AnneeUniversitaire.ArchiverChargeEnseignement(Code); 
    }
}