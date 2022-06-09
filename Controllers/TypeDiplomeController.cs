
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Projet_PFE.Bll;
using Projet_PFE.Models;
namespace Projet_PFE.Controllers
{
    public class TypeDiplomeController : Controller
    {
        public IActionResult Index() => View(Bll_TypeDiplome.SelectAll());
        public IActionResult CreateEditTypeDiplome(string Code)
        {
            var Model = new TypeDiplome();
            if (!string.IsNullOrEmpty(Code))
            {
                Model = Bll_TypeDiplome.SelectByCode(Code);
            }
            return PartialView("Create", Model);
        }

        [HttpPost]
        public IActionResult StoreUpdate(string pCode, TypeDiplome TypeD)
        {
            if (string.IsNullOrEmpty(pCode))
            {
                Bll_TypeDiplome.Add(TypeD);
            }
            else
            {
                Bll_TypeDiplome.Update(pCode, TypeD);
            }

            return Json(Bll_TypeDiplome.SelectByCode(TypeD.Code));

        }

        [HttpPost]
        public JsonResult Delete(string pCode)
        {
            if (Bll_TypeDiplome.IsForeignKeyInTable("Filiere", pCode))
            {
                return Json(new { Delete = false, Element = "une Filière" });
            }

            Bll_TypeDiplome.Delete(pCode);
            return Json(new { Delete = true, Code = pCode });
        }

        [AcceptVerbs("Get", "POST")]
        public IActionResult VerifyCode([Bind] string Code, string pCode)
        {
            if (pCode == Code)
            {
                return Json(true);
            }

            var TypeD = Bll_TypeDiplome.SelectByCode(Code);
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