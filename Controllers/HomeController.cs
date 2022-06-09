using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Projet_PFE.Bll;
using Projet_PFE.Models;

namespace Projet_PFE.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        [Route("home")]
        [AllowAnonymous]
        public IActionResult Login() => RedirectToAction("LogIn", "Auth");

        [Route("/home/index")]
        [Authorize]
        public IActionResult Index() => View();

        [Route("home/settings")]
        [HttpGet]
        [Authorize]
        public IActionResult Settings()
        {
            var CIN = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var UserAccount = Bll_User.SelectByCIN(CIN);
            UserAccount.Password = "";
            ViewBag.UserAccount = UserAccount;

            if (UserAccount.Profil == "SuperAdministrateur")
            {
                var UserAccounts = Bll_User.SelectAll();
                var SuperUser = UserAccounts.Find(account => account.Profil == "SuperAdministrateur");
                UserAccounts.Remove(SuperUser);
                return View("../Settings/Settings", UserAccounts);
            }
            return View("../Settings/Settings", null);
        }

        [HttpPost]
        [Route("home/affiliations")]
        [Authorize]
        public JsonResult Affiliations(string Profil)
        {
            List<string> ListAffiliations = new List<string>();
            if (Profil == "AgentADDepartement" || Profil == "ChefDepartement")
            {
                var ListDepartements = Bll_Departement.SelectAll();
                if (ListDepartements.Count > 0)
                {
                    foreach (var DeptItem in ListDepartements)
                    {
                        ListAffiliations.Add(string.Concat("Dept", DeptItem.IntituleFr));
                    }
                }
            }

            if (Profil == "AgentAdministratif" || Profil == "DirecteurEtude")
            {
                ListAffiliations.Add("Administration");
            }

            return Json(ListAffiliations);
        }
        public IActionResult IndexPlanEtude() => View();
        public IActionResult IndexLocaux() => View();

        public IActionResult Parametre() => View();

        [HttpGet]
        public IActionResult PlanEtude()
        {
            var Data = new PlanEtude();

            Data.ListFilieres = Bll_Filiere.SelectAll();
            Data.ListNiveaux = Bll_Niveau.SelectFiliereNiveaux(Data.ListFilieres[0].Id);
            Data.ListParcours = Bll_Parcours.SelectFiliereParcours(Data.ListFilieres[0].Id);

            Data.ListModules = new List<Module>();
            Data.ListUnites = new List<UniteEnseignement>();

            return View(Data);
        }
        [HttpPost]
        public JsonResult PlanEtude(long IdFiliere)
        {
            var Data = new PlanEtude();
            Data.ListNiveaux = Bll_Niveau.SelectFiliereNiveaux(IdFiliere);
            Data.ListParcours = Bll_Parcours.SelectFiliereParcours(IdFiliere);

            return Json(Data);
        }

        public static void ConstructPlanEtude(PlanEtude Data)
        {
            Data.ListFilieres = Bll_Filiere.SelectAll();
            Data.ListNiveaux = Bll_Niveau.SelectFiliereNiveaux(Data.IdFiliere);
            Data.ListParcours = Bll_Parcours.SelectFiliereParcours(Data.IdFiliere);

            Data.NewUniteE = new UniteEnseignement();
            Data.NewModule = new Module();
            Data.ListUnites = Bll_UniteEnseignement.SelectParcoursUnites(Data.IdNiveau, Data.IdParcours, Data.Periode);
            if (Data.ListUnites.Count == 0)
                Data.ListUnites = new List<UniteEnseignement>();

            Data.ListModules = Bll_Module.SelectParcourscourses(Data.IdNiveau, Data.IdParcours, Data.Periode);
            if (Data.ListModules.Count == 0)
                Data.ListModules = new List<Module>();
        }

        public IActionResult SearchPlanEtude(PlanEtude Data)
        {
            ConstructPlanEtude(Data);
            return View("PlanEtude", Data);
        }
        public IActionResult SearchPlan(string Code)
        {
            string[] Codes = Code.Split(',');
            var Data = new PlanEtude();

            Data.IdFiliere = Convert.ToInt64(Code[0]);
            Data.IdNiveau = Convert.ToInt64(Code[1]);
            Data.IdParcours = Convert.ToInt64(Code[2]);
            Data.Periode = Convert.ToInt32(Code[3]);
            ConstructPlanEtude(Data);

            return View("PlanEtude", Data);
        }

        public static PlanEtude SearchPlanE(string Code)
        {
            string[] Codes = Code.Split(',');
            var Data = new PlanEtude();

            Data.IdFiliere = Convert.ToInt64(Codes[0]);
            Data.IdNiveau = Convert.ToInt64(Codes[1]);
            Data.IdParcours = Convert.ToInt64(Codes[2]);
            Data.Periode = Convert.ToInt32(Codes[3]);

            ConstructPlanEtude(Data);

            return Data;
        }
    }
}