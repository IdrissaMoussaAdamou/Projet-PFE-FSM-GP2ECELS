using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projet_PFE.Dal;
using Projet_PFE.Models;

namespace Projet_PFE.Bll
{
    public class Bll_SessionSalle
    {
        public static string Add(SessionSalle SessionSalle)
        {
            return Dal_SessionSalle.Insert(SessionSalle);
        }

        public static String Update(
           string OldCode,
           SessionSalle SessionSalle) => Dal_SessionSalle.Update(SessionSalle, OldCode);

        public static void Delete(long id) => Dal_SessionSalle.Delete(id);

        public static List<SessionSalle> SelectAll(long id) => Dal_SessionSalle.SelectAll(id);

        public static SessionSalle SelectById(long Id) => Dal_SessionSalle.SelectByCode(Id);
    }
}
