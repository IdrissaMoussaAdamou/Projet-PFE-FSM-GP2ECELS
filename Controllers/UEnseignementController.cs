using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Projet_PFE.Bll;
using Projet_PFE.Models;
namespace Projet_PFE.Controllers
{
    public class UEnseignementController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult CreateEditUnite(string Code)
        {
            var Codes = Code.Split(",");
            var Data = HomeController.SearchPlanE(Code);
            var IdUnite = Convert.ToInt64(Codes[4]);

            if (IdUnite > 0)
            {
                Data.NewUniteE = Bll_UniteEnseignement.SelectById(IdUnite);
            }
            else
            {
                Data.NewUniteE.Periode = null;
            }
            return PartialView("Create", Data);
        }
        [HttpPost]
        public IActionResult StoreUpdate([Bind] UniteEnseignement NewUniteE)
        {
            var Unite = new UniteEnseignement();
            if (NewUniteE.IdUniteEnseignement > 0)
            {
                Bll_UniteEnseignement.Update(NewUniteE);
                Unite = Bll_UniteEnseignement.SelectById(NewUniteE.IdUniteEnseignement);
            }
            else
            {
                Bll_UniteEnseignement.Add(NewUniteE);
                Unite = Bll_UniteEnseignement.SelectByCode(NewUniteE.Code);
            }

            return Json(Unite);
        }

        [HttpPost]
        public JsonResult Delete(long IdUniteEnseignement)
        {
            if (Bll_UniteEnseignement.IsForeignKeyInTable("Module", IdUniteEnseignement))
            {
                return Json(new { Delete = false, Element = "un Module" });
            }

            Bll_UniteEnseignement.Delete(IdUniteEnseignement);
            return Json(new { Delete = true, Id = IdUniteEnseignement });
        }
        public IActionResult Show(string Id)
        {
            var IdUniteEnseignement = Convert.ToInt64(Id);
            var Model = Bll_UniteEnseignement.SelectById(IdUniteEnseignement);
            return PartialView("Show", Model);
        }

        [AcceptVerbs("Get", "POST")]
        public IActionResult VerifyCode([Bind] UniteEnseignement NewUniteE)
        {
            var Code = NewUniteE.Code;
            var OldCode = NewUniteE.OldCode;
            if (Code == OldCode)
            {
                return Json(true);
            }
            else
            {
                var Course = Bll_UniteEnseignement.SelectByCode(Code);
                if (Course != null)
                {
                    return Json($"Le Code {Code} est déjà utilisé.");
                }
                else
                {
                    return Json(true);
                }
            }
        }
    }
}