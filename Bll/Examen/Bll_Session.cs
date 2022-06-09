using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projet_PFE.Dal;
using Projet_PFE.Models;

namespace Projet_PFE.Bll
{
    public class Bll_Session
    {
        public static string Add(Session Session)
        {
            return Dal_Session.Insert(Session);
        }

        public static String Update(
           long OldCode,
           Session Session) => Dal_Session.Update(Session,OldCode);

        public static void Delete(long id) => Dal_Session.Delete(id);

        public static List<Session> SelectAll() => Dal_Session.SelectAll();

        public static Session SelectById(long Id) => Dal_Session.SelectByCode(Id);
    }
}
