using System;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Projet_PFE.Bll;
using Projet_PFE.Models;

namespace Projet_PFE.Controllers
{
    public class SessionController : Controller
    {
        [HttpGet]
        public IActionResult Index() => View(Bll_Session.SelectAll());

        public IActionResult Index2() => View(Bll_Session.SelectAll());

        public IActionResult CreateEditSession(long Code)
        {
            var model = new Session();
            if (Code != 0)
                model = Bll_Session.SelectById(Code);

            return PartialView("CreateEdit", model);
        }

        [HttpPost]
        public IActionResult StoreUpdate(long OldCode, Session S)
        {
            object[] obj = new object[2];
            var model = new Salle();
            if (OldCode == 0)
            {
                obj[0] = Bll_Session.Add(S);


                obj[1] = Bll_Session.SelectById(S.Id);
            }
            else
            {
                obj[0] = Bll_Session.Update(OldCode, S);

                obj[1] = Bll_Session.SelectById(S.Id);
            }

            return Json(obj);
        }

        [HttpGet]
        public JsonResult Delete(long id)
        {
            //var Session = Bll_Session.SelectById(id);

            //if (Session == null)
            //{
            //    return Json(new { Delete = false, Code = id });
            //}
            Bll_Session.Delete(id);
            return Json(new { Delete = true, Code = id });
        }
        [HttpGet]
        [Route("show/{id}")]
        [Authorize]
        public IActionResult show2(long id)
        {
            var Data = new UneSession();
            Data.ListJournée = Bll_SessionJour.SelectAll(id);
            Data.ListSalles = Bll_SessionSalle.SelectAll(id);
            Data.ListSeance = Bll_SessionSeance.SelectAll(id);
            Data.ListSurveillants = Bll_SessionSurveillant.SelectAll(id);
            Data.ListSection = Bll_SessionSection.SelectAll(id);
            Data.Session = Bll_Session.SelectById(id);
            return View(Data);
        }

        [HttpGet]
        [Route("show3/{id}")]
        [Authorize]
        public IActionResult show3(long id)
        {
            var Data = new UneSession();
            Data.ListJournée = Bll_SessionJour.SelectAll(id);
            Data.ListSalles = Bll_SessionSalle.SelectAll(id);
            Data.ListSeance = Bll_SessionSeance.SelectAll(id);
            Data.ListSurveillants = Bll_SessionSurveillant.SelectAll(id);
            Data.ListSection = Bll_SessionSection.SelectAll(id);
            Data.Session = Bll_Session.SelectById(id);
            return View(Data);
        }

        public IActionResult CreateEditSessionJ(long id, long ids)
        {
            var model = new SessionJour();
            if (id != 0)
                model = Bll_SessionJour.SelectById(id);
            else
                model.IdSession = ids;

            return PartialView("../SessionJour/Create", model);
        }

        [HttpPost]
        public IActionResult StoreUpdateJ(long OldCode, SessionJour S)
        {
            object[] obj = new object[2];
            var model = new Salle();
            if (OldCode == 0)
            {
                obj[0] = Bll_SessionJour.Add(S);


                obj[1] = Bll_SessionJour.SelectById(S.Id);
            }
            else
            {
                obj[0] = Bll_SessionJour.Update(OldCode, S);

                obj[1] = Bll_SessionJour.SelectById(S.Id);
            }

            return Json(obj);
        }

        public IActionResult showJ(long id) => PartialView("../SessionJour/show", Bll_SessionJour.SelectById(id));

        public JsonResult DeleteJ(long id)
        {
            var Sessionj = Bll_SessionJour.SelectById(id);

            if (Sessionj == null)
            {
                return Json(new { Delete = false, Code = id });
            }
            Bll_SessionJour.Delete(id);
            return Json(new { Delete = true, Code = id });
        }

        [HttpGet]
        public IActionResult CreateEditSessionSalle(long ids)
        {
            ViewBag.ids = ids;
            ViewBag.ListBatiment = Bll_Batiment.SelectAll();
            return PartialView("../SessionSalle/Create", Bll_Salle.SelectAll(ids));
        }

        public IActionResult ImporterSalle(string[] tab, long ids)
        {
           string[] obj = new string[20];
            int i = 0;
            foreach (string code in tab)
            {
                SessionSalle ss = new SessionSalle();
                Salle s = Bll_Salle.SelectById(code);
                ss.IdSession = ids;
                ss.CodeSalle = code;
                ss.CapaciteEnseignement = s.CapaciteEnseignement;
                ss.CapaciteExamen = s.CapaciteExamen;
                ss.NbSurveillants = s.NbSurveillants;
                ss.Etat = s.Etat;
                obj[i]= Bll_SessionSalle.Add(ss);
                i++;
            }
            return Json(obj);
        }

        public IActionResult ShowSalle(string code) => PartialView("../SessionSalle/Show", Bll_Salle.SelectById(code));

        public JsonResult DeleteS(long id)
        {
            var SessionS = Bll_SessionSalle.SelectById(id);

            if (SessionS == null)
            {
                return Json(new { Delete = false, Code = id });
            }
            Bll_SessionSalle.Delete(id);
            return Json(new { Delete = true, Code = id });
        }

        public IActionResult CreateEditSessionSc(long id, long ids)
        {
            var model = new SessionSeance();
            if (id != 0)
                model = Bll_SessionSeance.SelectById(id);
            else
                model.IdSession = ids;

            return PartialView("../SessionSeance/Create", model);
        }

        [HttpPost]
        public IActionResult StoreUpdateSc(long OldCode, SessionSeance S)
        {
            object[] obj = new object[2];

            if (OldCode == 0)
            {
                obj[0] = Bll_SessionSeance.Add(S);


                obj[1] = Bll_SessionSeance.SelectById(S.Id);
            }
            else
            {
                obj[0] = Bll_SessionSeance.Update(OldCode, S);

                obj[1] = Bll_SessionSeance.SelectById(S.Id);
            }

            return Json(obj);
        }

        public IActionResult showSc(long id) => PartialView("../SessionSeance/show", Bll_SessionSeance.SelectById(id));

        public JsonResult DeleteSc(long id)
        {
            var SessionSc = Bll_SessionSeance.SelectById(id);

            if (SessionSc == null)
            {
                return Json(new { Delete = false, Code = id });
            }
            Bll_SessionSeance.Delete(id);
            return Json(new { Delete = true, Code = id });
        }

        [HttpGet]
        public IActionResult CreateEditSessionSurveillant(long ids)
        {
            ViewBag.ids = ids;
            ViewBag.ListDepartement=Bll_Departement.SelectAll();
            return PartialView("../SessionSurveillant/Create", Bll_Enseignant.SelectAll(ids));
        }

        public IActionResult ImporterSurveillant(string[] tab, long ids)
        {
            string[] obj = new string[20];
            int i = 0;
            foreach (string CIN in tab)
            {
                SessionSurveillant ss = new SessionSurveillant();
                Enseignant s = Bll_Enseignant.SelectByCIN(CIN);
                ss.IdSession = ids;
                ss.CIN = CIN;
                ss.IdEnseignant = s.Id;
                ss.Nom = s.Nom;
                ss.Prenom = s.Prenom;
                ss.Grade = s.Grade;
                ss.Statut = s.Statut;
                ss.CodeDepartement = s.CodeDepartement;
                ss.SituationAdministrative = s.SituationAdministrative;
                ss.Email1 = s.Email1;
                ss.Email2 = s.Email2;
                ss.Telephone1 = s.Telephone1;
                ss.Telephone2 = s.Telephone2;
                obj[i] = Bll_SessionSurveillant.Add(ss);
                i++;
            }
            return Json(obj);
        }

        public IActionResult ShowSurveillant(string Cin) => PartialView("../SessionSurveillant/Show", Bll_Enseignant.SelectByCIN(Cin));

        public JsonResult DeleteSurveillant(long id)
        {
            var SessionS = Bll_SessionSurveillant.SelectById(id);

            if (SessionS == null)
            {
                return Json(new { Delete = false, Code = id });
            }
            Bll_SessionSurveillant.Delete(id);
            return Json(new { Delete = true, Code = id });
        }

        public IActionResult CreateEditSessionSection(long ids, string date)
        {
            ViewBag.ids = ids;
            return PartialView("../SessionSection/Create", Bll_SessionSection.SelectAll(ids,date));
        }

        public IActionResult ImporterSection(long[] tab, long ids)
        {
            string[] obj = new string[20];
            int i = 0;
            foreach (long id in tab)
            {
                SessionSection ss = new SessionSection();
                AnneeUniversitaireNiveauParcoursPeriode s = Bll_AnneeUniversitaireNiveauParcoursPeriode.SelectById(id);
                Filiere f = Bll_Filiere.SelectById(s.IdFiliere);
                Parcours p = Bll_Parcours.SelectById(s.IdParcours);
                ss.CodeFiliere = f.Code;
                ss.IntituleFiliere = f.IntituleFr;
                ss.IntituleFiliereAbrg = f.IntituleAbrg;
                ss.TypeDiplome = f.CodeTypeDiplome;
                ss.CodeParcours = p.Code;
                ss.IntituleParcours = p.IntituleFr;
                ss.IntituleParcoursAbrg = p.IntituleAbrg;
                ss.IdSession = ids;
                ss.NbEtudiants = s.NbEtudiants;
                ss.Niveau = Bll_Niveau.SelectById(s.IdNiveau).IntituleAbrg;
                ss.Niveaudyplo = Bll_Niveau.SelectById(s.IdNiveau).IntituleFr;
                ss.Periode = s.Periode;

                obj[i] = Bll_SessionSection.Add(ss);
                i++;
            }
            return Json(obj);
        }

        public IActionResult ShowSection(long code)
        {
            string td = Bll_SessionSection.SelectById(code).TypeDiplome;
            ViewBag.typediplo =Bll_TypeDiplome.SelectByCode(td).IntituleFr;
            return PartialView("../SessionSection/Show", Bll_SessionSection.SelectById(code));
        }

        public JsonResult DeleteSection(long id)
        {
            var SessionS = Bll_SessionSection.SelectById(id);

            if (SessionS == null)
            {
                return Json(new { Delete = false, Code = id });
            }
            Bll_SessionSection.Delete(id);
            return Json(new { Delete = true, Code = id });
        }

        public JsonResult SelectBatiment()
        {
            List<Batiment> ListDept = Bll_Batiment.SelectAll();
            return Json(ListDept);
        }

        public JsonResult SelectDepartementt()
        {
            List<Departement> ListDept = Bll_Departement.SelectAll();
            return Json(ListDept);
        }

        public JsonResult FilterSalle(string FieldName, string FieldValue, long ids)
        {
            var ListSalle = Bll_Salle.SelectAll(FieldName, FieldValue, ids);
         
            return Json(ListSalle);

        }

        public JsonResult FilterSurveillant(string FieldName, string FieldValue, long ids)
        {
            var ListSurv = Bll_Enseignant.SelectAll(FieldName, FieldValue, ids);

            return Json(ListSurv);

        }

    }
}
