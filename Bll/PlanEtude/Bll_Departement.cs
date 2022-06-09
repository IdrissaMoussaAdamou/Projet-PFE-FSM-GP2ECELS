using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projet_PFE.Models;
using Projet_PFE.Dal;

namespace Projet_PFE.Bll
{
    public class Bll_Departement
    {
        public static void Add(Departement Dept) =>  Dal_Departement.Add(Dept);
           
        public static void Delete(string Code) => Dal_Departement.Delete(Code);
            
        public static void Update(string OldCode, Departement Dept) => Dal_Departement.Update(OldCode,Dept);
            
        public static List<Departement> SelectAll() => Dal_Departement.SelectAll();

        public static Departement SelectByCode(string Code) => Dal_Departement.SelectByCode(Code);

        public static Departement SelectByIntituleFr(string Code) => Dal_Departement.SelectByIntituleFr(Code);
        public static bool IsForeignKeyInTable(
            string TableName,
            string Code) => Dal_Departement.IsForeignKeyInTable(TableName, Code);            
    }
}