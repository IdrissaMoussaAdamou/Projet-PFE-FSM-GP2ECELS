using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projet_PFE.Dal;
using Projet_PFE.Models;

namespace Projet_PFE.Bll
{
    public class Bll_Salle
    {
        public static String Add(Salle Salle) {
            return Dal_Salle.Insert(Salle);
        }

        public static void Delete(string code) => Dal_Salle.Delete(code);

        public static String Update(
            string OldCode,
            Salle salle) => Dal_Salle.Update(salle,OldCode);

        public static List<Salle> SelectAll() => Dal_Salle.SelectAll();

        public static List<Salle> SelectAll(long id) => Dal_Salle.SelectAll(id);

        public static List<Salle> SelectAll(
            string FieldName,
            string FielValue,
            long id) => Dal_Salle.SelectAll(FieldName, FielValue,id);

        public static Salle SelectById(string code) => Dal_Salle.SelectByCode(code);
    }
}
