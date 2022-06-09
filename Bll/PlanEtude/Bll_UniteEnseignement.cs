using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projet_PFE.Models;
using Projet_PFE.Dal;

namespace Projet_PFE.Bll
{
    public class Bll_UniteEnseignement
    {
        public static void Add(UniteEnseignement Unite) => Dal_UniteEnseignement.Add(Unite);
        public static void Delete(long IdUniteEnseignement) => Dal_UniteEnseignement.Delete(IdUniteEnseignement);
        public static void Update(UniteEnseignement Unite) => Dal_UniteEnseignement.Update(Unite);
        public static UniteEnseignement SelectByCode(string Code) => Dal_UniteEnseignement.SelectByCode(Code);
        public static UniteEnseignement SelectById(long IdUnite) => Dal_UniteEnseignement.SelectById(IdUnite);
        public static List<UniteEnseignement> SelectParcoursUnites(
            long IdNiveau,
            long IdParcours,
            int Periode) => Dal_UniteEnseignement.SelectParcoursUnites(IdNiveau, IdParcours, Periode);
        public static bool IsForeignKeyInTable(
            string TableName,
            long Id) => Dal_UniteEnseignement.IsForeignKeyInTable(TableName, Id);
        
    }
    
}