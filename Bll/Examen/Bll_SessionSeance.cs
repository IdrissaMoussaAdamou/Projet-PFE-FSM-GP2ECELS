using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projet_PFE.Dal;
using Projet_PFE.Models;

namespace Projet_PFE.Bll
{
    public class Bll_SessionSeance
    {
        public static string Add(SessionSeance SessionSeance)
        {
            return Dal_SessionSeance.Insert(SessionSeance);
        }

        public static String Update(
           long OldCode,
           SessionSeance SessionSeance) => Dal_SessionSeance.Update(SessionSeance, OldCode);

        public static void Delete(long id) => Dal_SessionSeance.Delete(id);

        public static List<SessionSeance> SelectAll(long id) => Dal_SessionSeance.SelectAll(id);

        public static SessionSeance SelectById(long Id) => Dal_SessionSeance.SelectByCode(Id);
    }
}
