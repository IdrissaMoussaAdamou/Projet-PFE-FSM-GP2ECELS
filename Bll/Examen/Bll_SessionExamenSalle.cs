using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projet_PFE.Dal;
using Projet_PFE.Models;

namespace Projet_PFE.Bll
{
    public class Bll_SessionExamenSalle
    {
        public static string Add(SessionExamenSalle SessionExamenSalle)
        {
            return Dal_SessionExamenSalle.Insert(SessionExamenSalle);
        }

        //public static String Update(
        //   long OldCode,
        //   SessionJour SessionJour) => Dal_SessionJour.Update(SessionJour, OldCode);

        public static void Delete(long ids, long idE, long idS) => Dal_SessionExamenSalle.Delete(ids, idE, idS);

        public static List<SessionExamenSalle> SelectAll(long id) => Dal_SessionExamenSalle.SelectAll(id);
        public static List<SessionExamenSalle> SelectAll(long idSession, List<long> idSessionExam, List<long> idSessionSalle) => Dal_SessionExamenSalle.Select(idSession, idSessionExam, idSessionSalle);
        public static List<SessionExamenSalle> SelectAll(long idSession, long idSessionExam, List<long> idSessionSalle) => Dal_SessionExamenSalle.Select(idSession, idSessionExam, idSessionSalle);

        public static SessionExamenSalle SelectById(long Id) => Dal_SessionExamenSalle.SelectByCode(Id);
    }
}
