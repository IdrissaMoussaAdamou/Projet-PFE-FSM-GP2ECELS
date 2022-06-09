using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projet_PFE.Dal;
using Projet_PFE.Models;

namespace Projet_PFE.Bll
{
    public class Bll_SessionExamen
    {
        public static string Add(SessionExamen SessionExamen)
        {
            return Dal_SessionExamen.Insert(SessionExamen);
        }

        //public static String Update(
        //   long OldCode,
        //   SessionJour SessionJour) => Dal_SessionJour.Update(SessionJour, OldCode);

        public static void Delete(long ids, long idj, string sec, long nbc) => Dal_SessionExamen.Delete(ids, idj, sec, nbc);

        public static List<SessionExamen> SelectAll(long id) => Dal_SessionExamen.SelectAll(id);
        public static List<SessionExamen> SelectAll(long id,List<long> idSessionJour, List<string> niveaux) => Dal_SessionExamen.Select(id,idSessionJour, niveaux);
        public static List<SessionExamen> SelectAll(long idSession, long idSessionJour, long idSessionseance, string niveau) => Dal_SessionExamen.Select(idSession, idSessionJour, idSessionseance, niveau);

        public static SessionExamen SelectById(long Id) => Dal_SessionExamen.SelectByCode(Id);
    }
}
