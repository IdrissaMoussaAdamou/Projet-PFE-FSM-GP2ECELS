using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projet_PFE.Dal;
using Projet_PFE.Models;

namespace Projet_PFE.Bll
{
    public class Bll_SessionSurveillant
    {
        public static string Add(SessionSurveillant SessionSurveillant)
        {
            return Dal_SessionSurveillant.Insert(SessionSurveillant);
        }

        public static String Update(
           string OldCode,
           SessionSurveillant SessionSurveillant) => Dal_SessionSurveillant.Update(SessionSurveillant, OldCode);

        public static void Delete(long id) => Dal_SessionSurveillant.Delete(id);

        public static List<SessionSurveillant> SelectAll(long id) => Dal_SessionSurveillant.SelectAll(id);

        public static SessionSurveillant SelectById(long Id) => Dal_SessionSurveillant.SelectByCode(Id);
    }
}
