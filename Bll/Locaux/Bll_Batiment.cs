using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projet_PFE.Dal;
using Projet_PFE.Models;

namespace Projet_PFE.Bll
{
    public class Bll_Batiment
    {
        public static List<Batiment> SelectAll() => Dal_Batiment.SelectAll();
        public static String Add(Batiment B)
        {
            return Dal_Batiment.Insert(B);
        }

        public static void Delete(string code) => Dal_Batiment.Delete(code);

        public static Batiment SelectById(string code) => Dal_Batiment.SelectByCode(code);

        public static bool IsForeignKeyInTable(
            string TableName,
            string Code) => Dal_Batiment.IsForeignKeyInTable(TableName, Code);
    }
}
