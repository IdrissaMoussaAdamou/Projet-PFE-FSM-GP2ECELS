using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Projet_PFE.Bll;
using Projet_PFE.Models;
namespace Projet_PFE.Controllers
{
    public class TypePeriodeController : Controller
    {
        [HttpGet]
        public IActionResult Index() => View(Bll_TypePeriode.SelectAll());

        public IActionResult CreateEditTypePeriode(string Code)
        {
            var Model = new TypePeriode();
            if (!String.IsNullOrEmpty(Code))
            {
                Model = Bll_TypePeriode.SelectByCode(Code);
            }

            return PartialView("Create", Model);
        }

        [HttpPost]
        public IActionResult StoreUpdate(string pCode, TypePeriode PerD)
        {
            if (String.IsNullOrEmpty(pCode))
            {
                Bll_TypePeriode.Add(PerD);
            }
            else
            {
                Bll_TypePeriode.Update(pCode, PerD);
            }
            return Json(Bll_TypePeriode.SelectByCode(PerD.Code));
        }

        [HttpPost]
        public JsonResult Delete(string Code)
        {
            if (Bll_TypePeriode.IsForeignKeyInTable("Filiere", Code))
            {
                return Json(new { Delete = false, Element = "une Filière" });
            }

            Bll_TypePeriode.Delete(Code);
            return Json(new { Delete = true, Code = Code });

        }


        [AcceptVerbs("Get", "POST")]
        public IActionResult VerifyCode(string Code, string pCode)
        {
            if (pCode == Code)
            {
                return Json(true);
            }

            var TypeD = Bll_TypePeriode.SelectByCode(Code);
            if (TypeD != null)
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