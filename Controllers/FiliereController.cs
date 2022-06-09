using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Projet_PFE.Bll;
using Projet_PFE.Models;
using System.Web;
using Microsoft.Extensions.Logging;

namespace Projet_PFE.Controllers
{
    public class FiliereController : Controller
    {

        private readonly ILogger<FiliereController> _logger;
        public FiliereController(ILogger<FiliereController> logger)
        {
            _logger = logger;
        }

        [Route("filiere/index")]

        public IActionResult Index()
        {
            try
            {
               return  View(Bll_Filiere.SelectAll());
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, ex.Message, null, ex.StackTrace);
                return StatusCode(500);
            }

        }

        public IActionResult CreateEdit(string Id)
        {
            var IdFiliere = Convert.ToInt64(Id);
            var model = new FiliereViewModel();

            model.Departements = Bll_Departement.SelectAll();
            if (model.Departements.Count == 0)
                model.Departements = new List<Departement>();

            model.TypeDiplomes = Bll_TypeDiplome.SelectAll();
            if (model.TypeDiplomes.Count == 0)
                model.TypeDiplomes = new List<TypeDiplome>();

            model.Periodes = Bll_TypePeriode.SelectAll();
            if (model.Periodes.Count == 0)
                model.Periodes = new List<TypePeriode>();

            model.Flre = new Filiere();

            if (IdFiliere != -1)
                model.Flre = Bll_Filiere.SelectById(IdFiliere);
            return PartialView("Create", model);
        }

        [HttpPost]
        public IActionResult StoreUpdate([Bind] Filiere Flre)
        {
            if (Flre.Id > 0)
            {
                Bll_Filiere.Update(Flre);

                return RedirectToAction(nameof(Show), new { Id = Flre.Id });
            }
            else
            {
                Bll_Filiere.Add(Flre);
            }

            return Json(Bll_Filiere.SelectTheLast());
        }
        [HttpGet]
        public IActionResult Show(long Id)
        {
            var Data = new FiliereNiveauParcoursViewModel();
            Data.Flre = Bll_Filiere.SelectById(Id);
            Data.Departements = Bll_Departement.SelectAll();
            Data.TypeDiplomes = Bll_TypeDiplome.SelectAll();
            Data.Parcours = Bll_Parcours.SelectFiliereParcours(Id);
            Data.Niveaux = Bll_Niveau.SelectFiliereNiveaux(Id);
            Data.Periodes = Bll_TypePeriode.SelectAll();

            return View(Data);
        }

        [HttpPost]
        public JsonResult Delete(string IdFiliere)
        {
            var Id = Convert.ToInt64(IdFiliere);
            if (Bll_Filiere.IsForeignKeyInTable("Parcours", Id))
            {
                return Json(new { Delete = false, Element = "un Parcours" });
            }

            if (Bll_Filiere.IsForeignKeyInTable("Niveau", Id))
            {
                return Json(new { Delete = false, Element = "un Niveau" });
            }

            if (Bll_Filiere.IsForeignKeyInTable("AnneeUniversitaireNiveauParcoursPeriode", Id))
            {
                return Json(new { Delete = false, Element = "un Niveau-Parcours Par Période" });
            }

            if (Bll_Filiere.IsForeignKeyInTable("AnneeUniversitaireFilieres", Id))
            {
                return Json(new { Delete = false, Element = "une Filière de l'année en cours" });
            }

            if (Bll_Filiere.IsForeignKeyInTable("ChargeParModule", Id))
            {
                return Json(new { Delete = false, Element = "une Charge Enseignement Par Module" });
            }

            Bll_Filiere.Delete(Id);

            return Json(new { Delete = true, Id = Id });
        }

    }
}