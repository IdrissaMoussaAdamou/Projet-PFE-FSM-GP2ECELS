using System;
using Microsoft.AspNetCore.Mvc;
using Projet_PFE.Bll;
using Projet_PFE.Models;
namespace Projet_PFE.Controllers
{
    public class ParcoursController : Controller
    {

        public IActionResult Index() => View();
        public IActionResult CreateEditParcours(string Codes)
        {
            var IdFiliere = Convert.ToInt64(Codes.Split(",")[0]);
            var Id = Convert.ToInt64(Codes.Split(",")[1]);
            var Model = new Parcours();
            if (Id == -1)
            {
                Model.IdFiliere = IdFiliere;
            }
            else
            {
                Model = Bll_Parcours.SelectById(Id);
            }

            return PartialView("Create", Model);
        }

        [HttpPost]
        public IActionResult StoreUpdate(Parcours Pars)
        {
            Parcours model = new Parcours();
            if (Pars.Id > 0)
            {
                Bll_Parcours.Update(Pars);
                model = Bll_Parcours.SelectById(Pars.Id);
            }
            else
            {
                Bll_Parcours.Add(Pars);
                model = Bll_Parcours.SelectTheLast();
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult Delete(long Id)
        {
            if (Bll_Parcours.IsForeignKeyInTable("UniteEnseignement", Id))
            {
                return Json(new { Delete = false, Element = "une Unité D'Enseignement" });
            }

            if (Bll_Parcours.IsForeignKeyInTable("Module", Id))
            {
                return Json(new { Delete = false, Element = "un Module" });
            }

            if (Bll_Parcours.IsForeignKeyInTable("AnneeUniversitaireNiveauParcoursPeriode", Id))
            {
                return Json(new { Delete = false, Element = "un Niveau-Parcours Par Période" });
            }

            Bll_Parcours.Delete(Id);
            return Json(new { Delete = true, Id = Id });
        }

    }
}