using System.Collections.Generic;
using Projet_PFE.Models;
using Projet_PFE.Dal;

namespace Projet_PFE.Bll
{
    public class Bll_ChargeDiverse
    {
        public static void Add(ChargeDiverse ChargDivers) =>  Dal_ChargeDiverse.Add(ChargDivers);
           
        public static void Delete(long Id) => Dal_ChargeDiverse.Delete(Id);
            
        public static void Update(ChargeDiverse ChargDivers) => Dal_ChargeDiverse.Update(ChargDivers);
            
        public static List<ChargeDiverse> SelectAll() => Dal_ChargeDiverse.SelectAll();
        public static List<ChargeDiverse> SelectAll(long TeacherId) => Dal_ChargeDiverse.SelectAll(TeacherId);

        public static ChargeDiverse SelectById(long Id) => Dal_ChargeDiverse.SelectById(Id);

        public static ChargeDiverse SelectTheLast() => Dal_ChargeDiverse.SelectTheLast();

    }
}