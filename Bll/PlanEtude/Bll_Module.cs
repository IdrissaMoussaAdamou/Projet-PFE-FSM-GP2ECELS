using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projet_PFE.Models;
using Projet_PFE.Dal;

namespace Projet_PFE.Bll
{
    public class Bll_Module
    {
        public static void Add(Module Course) => Dal_Module.Add(Course);
        
        public static void Delete(long IdModule) => Dal_Module.Delete(IdModule);
            
        public static void Update(string OldCode, Module Course) => Dal_Module.Update(OldCode, Course);
        public static List<Module> SelectAll(long IdNiveau) => Dal_Module.SelectAll(IdNiveau);

        public static List<Module> SelectParcourscourses(
            long IdNiveau,
            long IdParcours,
            int Periode) => Dal_Module.SelectParcourscourses(IdNiveau, IdParcours, Periode);
        
        public static Module SelectByCode(string Code) => Dal_Module.SelectByCode(Code);
        public static Module SelectById(long IdModule) => Dal_Module.SelectById(IdModule);
        public static bool IsForeignKeyInTable(
            string TableName,
            long Id) => Dal_Module.IsForeignKeyInTable(TableName, Id);
    }
    
}