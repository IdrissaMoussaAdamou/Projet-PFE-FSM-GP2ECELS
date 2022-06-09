using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Projet_PFE.Bll;
using Projet_PFE.Models;

namespace Projet_PFE.Controllers
{
    public class ModuleController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult CreateEditModule(string Code)
        {
            var Codes = Code.Split(",");
            var Data = HomeController.SearchPlanE(Code);
            var IdModule = Convert.ToInt64(Codes[4]);

            if (IdModule > 0)
            {
                Data.NewModule = Bll_Module.SelectById(IdModule);
            }
            else
            {
                Data.NewModule.Periode = null;
            }
            return PartialView("Create", Data);
        }

        public IActionResult Show(string Id)
        {
            var IdModule = Convert.ToInt64(Id);
            var Model = Bll_Module.SelectById(IdModule);
            return PartialView("Show", Model);
        }
        [HttpPost]
        public IActionResult StoreUpdate([Bind] Module NewModule)
        {
            var Olcode = NewModule.OldCode;
            if (NewModule.IdModule > 0)
            {
                Bll_Module.Update(Olcode, NewModule);
            }
            else
            {
                Bll_Module.Add(NewModule);
            }

            return Json(Bll_Module.SelectByCode(NewModule.Code));
        }
        public JsonResult Delete(long Id)
        {
            if (Bll_Module.IsForeignKeyInTable("AnneeUniversitaireNomOption", Id))
            {
                return Json(new { Delete = false, Element = "un Module Optionnel" });
            }

            if (Bll_Module.IsForeignKeyInTable("ChargeParModule", Id))
            {
                return Json(new { Delete = false, Element = "une Charge D'Enseignement Par Module" });
            }

            Bll_Module.Delete(Id);
            return Json(new { Delete = true, Id = Id });
        }

        [AcceptVerbs("Get", "POST")]
        public IActionResult VerifyCode([Bind] Module NewModule)
        {
            var Code = NewModule.Code;
            var OldCode = NewModule.OldCode;
            if (Code == OldCode)
            {
                return Json(true);
            }
            else
            {
                var Course = Bll_Module.SelectByCode(Code);
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