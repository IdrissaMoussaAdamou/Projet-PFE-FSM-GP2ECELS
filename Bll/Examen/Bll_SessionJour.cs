using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projet_PFE.Dal;
using Projet_PFE.Models;

namespace Projet_PFE.Bll
{
    public class Bll_SessionJour
    {
        public static string Add(SessionJour SessionJour)
        {
            return Dal_SessionJour.Insert(SessionJour);
        }

        public static String Update(
           long OldCode,
           SessionJour SessionJour) => Dal_SessionJour.Update(SessionJour, OldCode);

        public static void Delete(long id) => Dal_SessionJour.Delete(id);

        public static List<SessionJour> SelectAll(long id) => Dal_SessionJour.SelectAll(id);

        public static SessionJour SelectById(long Id) => Dal_SessionJour.SelectByCode(Id);

    }
}
