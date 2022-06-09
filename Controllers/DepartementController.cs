using System;
using Microsoft.AspNetCore.Mvc;
using Projet_PFE.Bll;
using Projet_PFE.Models;


namespace Projet_PFE.Controllers
{

    public class DepartementController : Controller
    {

        public IActionResult Index() => View(Bll_Departement.SelectAll());

        public ActionResult CreateEditDepartement(string Code)
        {
            var model = new Departement();
            if (!string.IsNullOrEmpty(Code))
            {
                model = Bll_Departement.SelectByCode(Code);
            }
            return PartialView("Create", model);
        }

        [HttpPost]
        public IActionResult StoreUpdate(string pCode, Departement Dept)
        {
            if (string.IsNullOrEmpty(pCode))
            {
                Bll_Departement.Add(Dept);
            }
            else
            {
                Bll_Departement.Update(pCode, Dept);
            }

            return Json(Bll_Departement.SelectByCode(Dept.Code));
        }

        [HttpPost]
        public JsonResult Delete(string pCode)
        {
            if (Bll_Departement.IsForeignKeyInTable("Filiere", pCode))
            {
                return Json(new { Delete = false, Element = "une Filière" });
            }

            if (Bll_Departement.IsForeignKeyInTable("Enseignant", pCode))
            {
                return Json(new { Delete = false, Element = "un Enseignant" });
            }

            if (Bll_Departement.IsForeignKeyInTable("AnneeUniversitaireEnseignant", pCode))
            {
                return Json(new { Delete = false, Element = "un Enseignant de l'année en cours" });
            }

            Bll_Departement.Delete(pCode);
            return Json(new { Delete = true, Code = pCode });
        }

        [AcceptVerbs("Get", "POST")]
        public IActionResult VerifyCode([Bind] string Code, string pCode)
        {
            if (pCode == Code)
            {
                return Json(true);
            }

            var Dept = Bll_Departement.SelectByCode(Code);
            if (Dept != null)
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