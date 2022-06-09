using System;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Projet_PFE.Bll;
using Projet_PFE.Models;

namespace Projet_PFE.Controllers
{
    public class SessionExamenSalleController : Controller
    {
        public IActionResult Index(long ids,long idSessionJour, long idSessionseance, string niveau)
        {
            long idj=idSessionJour;
            long idse=idSessionseance;
            ViewBag.ids = ids;
            ViewBag.séance = Bll_SessionSeance.SelectAll(ids);
            List<SessionJour> L = Bll_SessionJour.SelectAll(ids);
            L.Sort((a, b) => a.Jour.CompareTo(b.Jour));
            if (idSessionJour == -1 && idSessionseance == -1)
            {
                idj = L.FirstOrDefault().Id;
                idse = Bll_SessionSeance.SelectAll(ids).FirstOrDefault().Id;
            }
            ViewBag.jour = L;
            var model = new SessExaSalle();
            var LSE= Bll_SessionExamen.SelectAll(ids,idj,idse,niveau);
            var LSS= Bll_SessionSalle.SelectAll(ids);
            var a = LSE.Select(x => x.Id)?.ToList();
            var b = LSS.Select(x => x.Id)?.ToList();
            var BLSE = new List<BonLSE>();

            foreach (var SE in LSE)
            {
                BonLSE k = new BonLSE();
                k.SE = SE;
                int? nb = 0;
                var lses = Bll_SessionExamenSalle.SelectAll(ids, SE.Id, b);
                foreach(var ses in lses)
                {
                    nb = nb + Bll_SessionSalle.SelectById(ses.IdSessionSalle).CapaciteExamen;
                }
                k.nbc = nb;
                BLSE.Add(k);
            }


            model.LSE = BLSE;
            model.LSS = LSS;
            model.LSES = Bll_SessionExamenSalle.SelectAll(ids, a, b);
            return View(model);
        }

        [HttpPost]
        public IActionResult StoreUpdateSES(long IdSe, long IdSalle, long ids)
        {
            object[] obj = new object[2];
            var modul = new SessionExamenSalle();
            modul.IdSession = ids;
            modul.IdSessionExamen = IdSe;
            modul.IdSessionSalle = IdSalle;
            obj[0] = Bll_SessionExamenSalle.Add(modul);
            return Json(obj);
        }

        [HttpPost]
        public JsonResult DeleteSES(long IdSe, long IdSalle, long ids)
        {
            Bll_SessionExamenSalle.Delete(ids, IdSe, IdSalle);
            return Json(new { Delete = true, Code = ids });
        }
    }
}
