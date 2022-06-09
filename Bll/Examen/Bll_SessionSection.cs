using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projet_PFE.Dal;
using Projet_PFE.Models;

namespace Projet_PFE.Bll
{
    public class Bll_SessionSection
    {
        public static string Add(SessionSection SessionSection)
        {
            return Dal_SessionSection.Insert(SessionSection);
        }

        //public static String Update(
        //   long OldCode,
        //   SessionJour SessionJour) => Dal_SessionJour.Update(SessionJour, OldCode);

        public static void Delete(long id) => Dal_SessionSection.Delete(id);

        public static List<SessionSection> SelectAll(long id) => Dal_SessionSection.SelectAll(id);
        public static List<SessionSection> SelectByNiveau(long id, string niveau) => Dal_SessionSection.SelectByNiveau(id, niveau);

        public static List<UneSection> SelectAll(long ids, string date) => Dal_SessionSection.SelectAll(ids,date);

        public static SessionSection SelectById(long Id) => Dal_SessionSection.SelectByCode(Id);
    }
}
