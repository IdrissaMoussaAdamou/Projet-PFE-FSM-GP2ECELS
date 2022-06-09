using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projet_PFE.Models;
using Projet_PFE.Dal;

namespace Projet_PFE.Bll
{
    public class Bll_TypePeriode
    {
        public static void Add(TypePeriode Dept) => Dal_TypePeriode.Add(Dept);
        
        public static void Delete(string Code) =>  Dal_TypePeriode.Delete(Code);
       
        public static void Update(string OldCode, TypePeriode PerD) => Dal_TypePeriode.Update(OldCode, PerD);
      
        public static List<TypePeriode> SelectAll() => Dal_TypePeriode.SelectAll();

        public static TypePeriode SelectByCode(string Code) => Dal_TypePeriode.SelectByCode(Code);
       
        public static TypePeriode SelectByIntituleFr(string IntituleFr) => Dal_TypePeriode.SelectByCode(IntituleFr);
        public static bool IsForeignKeyInTable(
            string TableName,
            string Code) => Dal_TypePeriode.IsForeignKeyInTable(TableName, Code);
    }
    
}