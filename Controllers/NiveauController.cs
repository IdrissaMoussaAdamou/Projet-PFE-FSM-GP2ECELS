using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Projet_PFE.Bll;
using Projet_PFE.Models;

namespace Projet_PFE.Controllers
{
    public class NiveauController : Controller
    {

        public IActionResult CreateEditNiveau(string Codes)
        {
            var IdFiliere = Convert.ToInt64(Codes.Split(",")[0]);
            var IdNiveau = Convert.ToInt64(Codes.Split(",")[1]);
            var Model = new Niveau();
            if (IdNiveau > 0)
            {
                Model = Bll_Niveau.SelectById(IdNiveau);
            }
            else
            {
                Model.IdFiliere = IdFiliere;
            }

            return PartialView("Create", Model);
        }

        [HttpPost]
        public IActionResult StoreUpdate(Niveau Niv)
        {
            Niveau model = new Niveau();
            if (Niv.Id > 0)
            {
                Bll_Niveau.Update(Niv);
                model = Bll_Niveau.SelectById(Niv.Id);
            }
            else
            {
                Bll_Niveau.Add(Niv);
                model = Bll_Niveau.SelectTheLast();
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult Delete(long Id)
        {
            if (Bll_Niveau.IsForeignKeyInTable("UniteEnseignement", Id))
            {
                return Json(new { Delete = false, Element = "une Unité D'Enseignement" });
            }

            if (Bll_Niveau.IsForeignKeyInTable("Module", Id))
            {
                return Json(new { Delete = false, Element = "un Module" });
            }

            if (Bll_Niveau.IsForeignKeyInTable("AnneeUniversitaireNiveauParcoursPeriode", Id))
            {
                return Json(new { Delete = false, Element = "un Niveau-Parcours Par Période" });
            }

            if (Bll_Niveau.IsForeignKeyInTable("ChargeParModule", Id))
            {
                return Json(new { Delete = false, Element = "une Charge D'Enseignement Par Module" });
            }

            Bll_Niveau.Delete(Id);
            return Json(new { Delete = true, Id = Id });
        }
    }
}