using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projet_PFE.Bll;
using Projet_PFE.Models;

namespace Projet_PFE.Controllers
{
    public class LocauxController : Controller
    {
        [HttpGet]
        public IActionResult Index() => View(Bll_Salle.SelectAll());

        public IActionResult CreateEditSalle(string Code)
        {
            var model = new Salle();
            if (!string.IsNullOrEmpty(Code))
                model = Bll_Salle.SelectById(Code);

             ViewBag.ListBatiment = Bll_Batiment.SelectAll();

            return PartialView("CreateEdit", model);
        }

        [HttpPost]
        public IActionResult StoreUpdate(string OldCode, Salle S)
        {
            object[] obj = new object[2];
            var model = new Salle();
            if (string.IsNullOrEmpty(OldCode))
            {
                obj[0] = Bll_Salle.Add(S);
                //Bll_Salle.Add(S);
                //model = Bll_Salle.SelectById(S.Code);
                obj[1] = Bll_Salle.SelectById(S.Code);
            }
            else
            {
                SessionSalle ss = new SessionSalle();
                ss.CodeSalle = S.Code;
                ss.CapaciteEnseignement = S.CapaciteEnseignement;
                ss.CapaciteExamen = S.CapaciteExamen;
                ss.NbSurveillants = S.NbSurveillants;
                ss.Etat = S.Etat;
                Bll_SessionSalle.Update(OldCode, ss);
                obj[0] = Bll_Salle.Update(OldCode, S);
                //model = Bll_Salle.SelectById(S.Code);
                obj[1] = Bll_Salle.SelectById(S.Code); 
            }

            return Json(obj);
        }
        [HttpGet]
        public JsonResult Delete(string Code)
        {
            var Salle = Bll_Salle.SelectById(Code);

            if (Salle == null)
            {
                return Json(new { Delete = false, Code = Code });
            }
            Bll_Salle.Delete(Code);
            return Json(new { Delete = true, Code = Code });
        }

        public IActionResult Show(string Code) => PartialView("Show", Bll_Salle.SelectById(Code));

        [AcceptVerbs("Get", "POST")]
        public IActionResult VerifyCIN(string Code, string OldCode)
        {
            if (OldCode == Code)
            {
                return Json(true);
            }
            var Salle = Bll_Salle.SelectById(Code);
            if (Salle != null)
            {
                return Json($"Le Numéro Code {Code} est déjà utilisé.");
            }
            else
            {
                return Json(true);
            }
        }

    }
}
