using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projet_PFE.Models;
using Projet_PFE.Dal;

namespace Projet_PFE.Bll
{
    public class Bll_Niveau
    {
        public static void Add(Niveau Niv) =>  Dal_Niveau.Add(Niv);
           
        public static void Delete(long Id) => Dal_Niveau.Delete(Id);
            
        public static void Update(Niveau Niv) => Dal_Niveau.Update(Niv);

        public static List<Niveau> SelectAll() => Dal_Niveau.SelectAll(); 
        
        public static List<Niveau> SelectAll(
            string CodeFiliere,
            string CodeAnneeUniv) => Dal_Niveau.SelectAll(CodeFiliere, CodeAnneeUniv);
        
        public static List<Niveau> SelectFiliereNiveaux(long IdFiliere) => Dal_Niveau.SelectFiliereNiveaux(IdFiliere); 
        
        public static List<Niveau> GetAll(
            long IdFiliere,
            string CodeAnneeUniv) => Dal_Niveau.GetAll(IdFiliere, CodeAnneeUniv);
        public static Niveau SelectById(long Id) => Dal_Niveau.SelectById(Id);

         public static Niveau SelectTheLast() => Dal_Niveau.SelectTheLast();
       
        public static Niveau SelectByIntituleFr(string IntituleFr) => Dal_Niveau.SelectByIntituleFr(IntituleFr);
        public static Niveau SelectByIntituleAbr(string IntituleFr) => Dal_Niveau.SelectByIntituleAbr(IntituleFr);
        public static bool IsForeignKeyInTable(
            string TableName,
            long Id) => Dal_Niveau.IsForeignKeyInTable(TableName, Id);
    }
}