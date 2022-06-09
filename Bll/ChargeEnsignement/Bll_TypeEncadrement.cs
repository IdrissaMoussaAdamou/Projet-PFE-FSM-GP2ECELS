using System.Collections.Generic;
using Projet_PFE.Models;
using Projet_PFE.Dal;

namespace Projet_PFE.Bll
{
    public class Bll_TypeEncadrement
    {
        public static void Add(TypeEncadrement TyEncadrement) =>  Dal_TypeEncadrement.Add(TyEncadrement);
           
        public static void Delete(long Id) => Dal_TypeEncadrement.Delete(Id);
            
        public static void Update(TypeEncadrement TyEncadrement) => Dal_TypeEncadrement.Update(TyEncadrement);
            
        public static List<TypeEncadrement> SelectAll() => Dal_TypeEncadrement.SelectAll();

        public static TypeEncadrement SelectById(long Id) => Dal_TypeEncadrement.SelectById(Id);
        public static TypeEncadrement SelectTheLast() => Dal_TypeEncadrement.SelectTheLast();
        public static bool IsForeignKeyInTable(
            string TableName,
            long Id) => Dal_TypeEncadrement.IsForeignKeyInTable(TableName, Id);
    }
}