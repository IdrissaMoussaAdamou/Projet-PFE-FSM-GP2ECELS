using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Projet_PFE.Bll;
using Projet_PFE.Models;

namespace Projet_PFE.Controllers
{
    public class EnseignantController : Controller
    {
        [HttpGet]
        public IActionResult Index() => View(Bll_Enseignant.SelectAll());
        public IActionResult CreateEditEnseignant(long Id)
        {
            var model = new Enseignant();
            if (Id != -1)
                model = Bll_Enseignant.SelectById(Id);

            ViewBag.ListDepartement = Bll_Departement.SelectAll();

            return PartialView("CreateEdit", model);
        }

        [HttpPost]
        public IActionResult StoreUpdate(string OldCIN, Enseignant E)
        {
            var model = new Enseignant();
            if (E.Id < 0)
            {
                Bll_Enseignant.Add(E);
                model = Bll_Enseignant.SelectByCIN(E.CIN);
            }
            else
            {
                SessionSurveillant ss = new SessionSurveillant();
                ss.CIN = E.CIN;
                ss.IdEnseignant = E.Id;
                ss.Nom = E.Nom;
                ss.Prenom = E.Prenom;
                ss.Grade = E.Grade;
                ss.Statut = E.Statut;
                ss.CodeDepartement = E.CodeDepartement;
                ss.SituationAdministrative = E.SituationAdministrative;
                ss.Email1 = E.Email1;
                ss.Email2 = E.Email2;
                ss.Telephone1 = E.Telephone1;
                ss.Telephone2 = E.Telephone2;
                Bll_SessionSurveillant.Update(OldCIN, ss);
                Bll_Enseignant.Update(OldCIN, E);
                model = Bll_Enseignant.SelectById(E.Id);
            }

            return Json(model);
        }
        [HttpPost]
        public JsonResult Delete(long Id)
        {
            var Enseignant = Bll_Enseignant.SelectById(Id);

            if (Bll_Enseignant.HasRowInTableInAnneeUnivEnseignant(Enseignant.CIN))
            {
                return Json(new { Delete = false, element = "Année Universitaire" });
            }

            Bll_Enseignant.Delete(Id);
            return Json(new { Delete = true, Id = Id });
        }

        public IActionResult Show(long Id) => PartialView("Show", Bll_Enseignant.SelectById(Id));

        [AcceptVerbs("Get", "POST")]
        public IActionResult VerifyCIN(string CIN, string OldCIN)
        {
            if (OldCIN == CIN)
            {
                return Json(true);
            }
            var Enseignant = Bll_Enseignant.SelectByCIN(CIN);
            if (Enseignant != null)
            {
                return Json($"Le Numéro CIN {CIN} est déjà utilisé.");
            }
            else
            {
                return Json(true);
            }
        }

    }
}