using System.Collections.Generic;
using Projet_PFE.Models;
using Projet_PFE.Dal;

namespace Projet_PFE.Bll
{
    public class Bll_TypeChargeDiverse
    {
        public static void Add(TypeChargeDiverse TypeChrgDivers) =>  Dal_TypeChargeDiverse.Add(TypeChrgDivers);
           
        public static void Delete(long Id) => Dal_TypeChargeDiverse.Delete(Id);
            
        public static void Update(TypeChargeDiverse TypeChrgDivers) => Dal_TypeChargeDiverse.Update(TypeChrgDivers);
            
        public static List<TypeChargeDiverse> SelectAll() => Dal_TypeChargeDiverse.SelectAll();

        public static TypeChargeDiverse SelectById(long Id) => Dal_TypeChargeDiverse.SelectById(Id);
        public static TypeChargeDiverse SelectTheLast() => Dal_TypeChargeDiverse.SelectTheLast();
        public static bool IsForeignKeyInTable(
            string TableName,
            long Id) => Dal_TypeChargeDiverse.IsForeignKeyInTable(TableName, Id);
    }
}