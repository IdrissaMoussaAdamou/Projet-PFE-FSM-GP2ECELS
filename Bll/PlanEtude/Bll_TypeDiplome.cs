using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projet_PFE.Models;
using Projet_PFE.Dal;

namespace Projet_PFE.Bll
{
    public class Bll_TypeDiplome
    {
        public static void Add(TypeDiplome TypeD) =>  Dal_TypeDiplome.Add(TypeD);
        
        public static void Delete(string Code) => Dal_TypeDiplome.Delete(Code);

        public static void Update(
            string OldCode,
            TypeDiplome TypeD) => Dal_TypeDiplome.Update(OldCode,TypeD);  
        
        public static List<TypeDiplome> SelectAll() => Dal_TypeDiplome.SelectAll();     
      
        public static TypeDiplome SelectByCode(string Code) => Dal_TypeDiplome.SelectByCode(Code);
       
        public static TypeDiplome SelectByIntituleFr(string IntituleFr) => Dal_TypeDiplome.SelectByIntituleFr(IntituleFr);
   
        public static bool IsForeignKeyInTable(
            string TableName,
            string Code) => Dal_TypeDiplome.IsForeignKeyInTable(TableName, Code);
    }
}






