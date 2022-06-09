using System;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Projet_PFE.Bll;
using Projet_PFE.Models;
namespace Projet_PFE.Controllers
{
    [Authorize]
    public class AnneeUniversitaireController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            List<AnneeUniversitaire> OrderedListAnneeUniversitaires = new List<AnneeUniversitaire>();
            var ListAnneeUniversitaires = Bll_AnneeUniversitaire.SelectAll();
            if (ListAnneeUniversitaires.Count > 0)
            {
                for (int i = ListAnneeUniversitaires.Count - 1; i >= 0; i--)
                    OrderedListAnneeUniversitaires.Add(ListAnneeUniversitaires[i]);
            }

            return View(OrderedListAnneeUniversitaires);
        }

        [HttpPost]
        public JsonResult ArchiveAnneeUniversitaire(string CodeAnneeUniv)
        {
            Bll_AnneeUniversitaire.Archive(CodeAnneeUniv);
            return Json(CodeAnneeUniv);
        }

        [HttpPost]
        public JsonResult VerifyAnneeUniversitaireStatus(string CodeAnneeUniv) => Json(Bll_AnneeUniversitaire.SelectByCode(CodeAnneeUniv));
        public ActionResult CreateAnneeUniversitaire(string Code) => PartialView("Create", new AnneeUniversitaire());

        [HttpPost]
        public JsonResult Store(AnneeUniversitaire AnneeUniv)
        {
            var AnneUniv = Bll_AnneeUniversitaire.SelectTNArchivedAnneeUniv();
            if (AnneeUniv != null)
            {
                return Json(new
                {
                    Inserted = false,
                    CodeAnneeUniv = AnneUniv.Code
                });
            }
            AnneeUniv.Code = AnneeUniv.DateDebut.Year.ToString() + '-' + AnneeUniv.DateFin.Year.ToString();
            Bll_AnneeUniversitaire.Add(AnneeUniv);

            return Json(new { Inserted = true, Url = "/AnneeUniversitaire/Index" });
        }

        [HttpGet]
        public IActionResult Consult(string Code)
        {
            var Data = new SearchModelView();
            List<TypeDiplome> ListDiplome = new List<TypeDiplome>();
            ListDiplome = Bll_TypeDiplome.SelectAll();

            if (ListDiplome.Count != 0)
            {
                Data.ListFilieres = Bll_Filiere.GetAll(ListDiplome[0].Code, Code);
                if (Data.ListFilieres.Count != 0)
                {
                    Data.ListNiveaux = Bll_Niveau.GetAll(Data.ListFilieres[0].Id, Code);
                    Data.ListParcours = Bll_Parcours.GetAll(Data.ListFilieres[0].Id, Code);
                }
                else
                {
                    Data.ListFilieres = new List<Filiere>();
                    Data.ListNiveaux = new List<Niveau>();
                    Data.ListParcours = new List<Parcours>();
                }

            }
            else
            {

                Data.ListFilieres = new List<Filiere>();
                Data.ListNiveaux = new List<Niveau>();
                Data.ListParcours = new List<Parcours>();
            }

            ViewBag.ListDiplome = ListDiplome;
            ViewBag.Annee = Bll_AnneeUniversitaire.SelectByCode(Code);

            return View(Data);
        }

        [HttpPost]
        public JsonResult FilterFiliereByTypeDiplome(string CodeTypeDiplome, string CodeAnneeUniv)
        {
            var Data = new SearchModelView();
            Data.ListFilieres = Bll_Filiere.GetAll(CodeTypeDiplome, CodeAnneeUniv);

            if (Data.ListFilieres.Count > 0)
            {
                Data.ListNiveaux = Bll_Niveau.GetAll(Data.ListFilieres[0].Id, CodeAnneeUniv);
                Data.ListParcours = Bll_Parcours.GetAll(Data.ListFilieres[0].Id, CodeAnneeUniv);
            }
            else
            {
                Data.ListFilieres = new List<Filiere>();
                Data.ListNiveaux = new List<Niveau>();
                Data.ListParcours = new List<Parcours>();
            }

            return Json(Data);
        }

        [HttpPost]
        public JsonResult ConsultSearch(long IdFiliere)
        {
            var Data = new PlanEtude();
            Data.ListNiveaux = Bll_Niveau.SelectFiliereNiveaux(IdFiliere);
            Data.ListParcours = Bll_Parcours.SelectFiliereParcours(IdFiliere);

            return Json(Data);
        }

        public IActionResult CreateNomOption(string Code)
        {
            var Data = new SearchModelView();
            Data.NewNiveauParcours = new AnneeUniversitaireNiveauParcoursPeriode();
            Data.ListFilieres = Bll_Filiere.SelectAll();
            Data.ListNiveaux = Bll_Niveau.SelectFiliereNiveaux(Data.ListFilieres[0].Id);
            Data.ListParcours = Bll_Parcours.SelectFiliereParcours(Data.ListFilieres[0].Id);
            Data.CodeAnneuniv = Code;

            //Les Périodes sont Indépendantes de la Filières
            Data.ListTypePeriodes = Bll_TypePeriode.SelectAll();

            return PartialView("CreateNomOptionHeader", Data);
        }

        public IActionResult CreateNomOptionBody(string Codes)
        {
            List<NomOption> LesOptions = null;
            List<Module> Modules;
            string[] Code = Codes.Split(',');
            var IdNiveau = Convert.ToInt64(Code[1]);
            var IdParcours = Convert.ToInt64(Code[2]);
            var Periode = Convert.ToInt32(Code[3]);

            if ((Modules = Bll_Module.SelectParcourscourses(IdNiveau, IdParcours, Periode)) != null)
            {
                var Courses = Modules.FindAll(
                     delegate (Module m)
                     {
                         return m.Nature == "Optionnelle";
                     }
                 );

                if (Courses != null)
                {
                    LesOptions = new List<NomOption>(Courses.Count);
                    for (int i = 0; i < Courses.Count; i++)
                    {
                        var Option = new NomOption();
                        Option.IdModule = Courses[i].IdModule;
                        Option.ModuleIntuleFr = Courses[i].IntituleFr;
                        LesOptions.Add(Option);
                    }
                }
            }

            return PartialView("CreateNomOptionBody", LesOptions);
        }

        public IActionResult CreateEditOptions(string Codes)
        {
            List<NomOption> LesOptions = new List<NomOption>();
            List<Module> Modules;
            string[] Code = Codes.Split(',');

            var IdNiveau = Convert.ToInt64(Code[1]);
            var IdParcours = Convert.ToInt64(Code[2]);
            var Periode = Convert.ToInt32(Code[3]);

            if ((Modules = Bll_Module.SelectParcourscourses(IdNiveau, IdParcours, Periode)).Count != 0)
            {
                var Courses = Modules.FindAll(
                    delegate (Module m)
                    {
                        return m.Nature == "Optionnelle";
                    }
                );

                if (Courses != null)
                {
                    // Vérifier si les modules optionnels sont enregistrés ou non
                    if ((LesOptions = Bll_AnneeUniversitaireNomOption.SelectAll(IdNiveau, IdParcours, Periode, Code[4])).Count == 0)
                    {
                        LesOptions = new List<NomOption>(Courses.Count);
                        for (int i = 0; i < Courses.Count; i++)
                        {
                            var Option = new NomOption();
                            Option.IdModule = Courses[i].IdModule;
                            Option.ModuleIntuleFr = Courses[i].IntituleFr;
                            Option.CodeAnneeUniv = Code[4];
                            LesOptions.Add(Option);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < Courses.Count; i++)
                            LesOptions[i].ModuleIntuleFr = Courses[i].IntituleFr;
                    }
                }
            }

            return PartialView("CreateNomOptionBody", LesOptions);
        }

        [HttpPost]
        public JsonResult StoreUpdateNomOption(List<NomOption> LesOptions)
        {
            if (LesOptions[0].Id == 0)
            {
                foreach (var OptionItem in LesOptions)
                {
                    var Option = new NomOption();
                    Option.IdModule = OptionItem.IdModule;
                    Option.Intitule = OptionItem.Intitule;
                    Option.IntituleAbrg = OptionItem.IntituleAbrg;
                    Option.CodeAnneeUniv = OptionItem.CodeAnneeUniv;
                    Bll_AnneeUniversitaireNomOption.Add(Option);
                }
            }
            else
            {
                foreach (var OptionItem in LesOptions)
                {
                    var Option = new NomOption();
                    Option.IdModule = OptionItem.IdModule;
                    Option.Intitule = OptionItem.Intitule;
                    Option.IntituleAbrg = OptionItem.IntituleAbrg;
                    Option.CodeAnneeUniv = OptionItem.CodeAnneeUniv;
                    Option.Id = OptionItem.Id;
                    Bll_AnneeUniversitaireNomOption.Update(Option);
                }
            }

            return Json(LesOptions);
        }

        [HttpPost]
        public JsonResult PlanEude(string Codes)
        {
            string[] Code = Codes.Split(',');
            var Data = new PlanEtude();

            var IdFiliere = Convert.ToInt64(Code[0]);
            var IdNiveau = Convert.ToInt64(Code[1]);
            var IdParcours = Convert.ToInt64(Code[2]);
            var Periode = Convert.ToInt32(Code[3]);

            Data.ListFilieres = new List<Filiere>();
            Data.ListFilieres.Add(Bll_Filiere.SelectById(IdFiliere));
            Data.ListUnites = Bll_UniteEnseignement.SelectParcoursUnites(IdNiveau, IdParcours, Periode);
            Data.ListModules = Bll_Module.SelectParcourscourses(IdNiveau, IdParcours, Periode);

            // changer le nom des modules optionnels

            for (int i = 0; i < Data.ListModules.Count; i++)
            {
                if (Data.ListModules[i].Nature == "Optionnelle")
                {
                    var Option = Bll_AnneeUniversitaireNomOption.SelectModuleOption(Data.ListModules[i].IdModule, Code[4]);
                    if (Option != null)
                        Data.ListModules[i].IntituleFr = Option.Intitule;
                    else
                        break;
                }
            }

            return Json(Data);
        }
        [HttpPost]
        public IActionResult StoreNomOption(string CodeAnneeUniv, List<NomOption> LesOptions)
        {
            foreach (var OptionItem in LesOptions)
            {
                var Option = new NomOption();
                Option.IdModule = OptionItem.IdModule;
                Option.CodeAnneeUniv = CodeAnneeUniv;
                Option.Intitule = OptionItem.Intitule;
                Option.IntituleAbrg = OptionItem.IntituleAbrg;
                Bll_AnneeUniversitaireNomOption.Add(Option);
            }

            return Json(true);
        }

        [HttpGet]
        public IActionResult AnneeUniversitaireFilieres(string Code)
        {
            var ListFilieres = Bll_Filiere.SelectAll(Code);
            ViewBag.Annee = Bll_AnneeUniversitaire.SelectByCode(Code);
            return View(ListFilieres);
        }

        public IActionResult CreateAnneeUniversitaireFiliere(string CodeAnneeUniv)
        {
            var ListDiplome = Bll_TypeDiplome.SelectAll();
            List<Filiere> ListFilieres;

            if (ListDiplome != null)
                ListFilieres = Bll_Filiere.SelectAll(ListDiplome[0].Code, CodeAnneeUniv);
            else
                ListFilieres = null;

            ViewBag.ListDiplome = ListDiplome;
            return PartialView("CreateAnneeUniversitaireFiliere", ListFilieres);
        }

        [HttpPost]
        public IActionResult TypeDiplomeFilieres(
            string CodeTypeDiplome,
            string CodeAnneeUniv) => Json(Bll_Filiere.SelectAll(CodeTypeDiplome, CodeAnneeUniv));

        [HttpPost]
        public JsonResult StoreAnneeUniversitaireFiliere(long IdFiliere, string CodeAnneeUniv)
        {
            Bll_AnneeUniversitaireFilieres.Add(IdFiliere, CodeAnneeUniv);

            return Json(Bll_Filiere.SelectById(IdFiliere));
        }

        [HttpPost]
        public JsonResult DeleteAnneeUniversitaireFiliere(long IdFiliere, string CodeAnneeUniv)
        {
            Bll_AnneeUniversitaireFilieres.Delete(IdFiliere, CodeAnneeUniv);
            return Json(IdFiliere);
        }

        public IActionResult AnneeUniversitaireNivParcours(string Annee)
        {
            var CodeAnneeUniv = Annee.Split('?')[0];
            var IdFiliere = Convert.ToInt64((Annee.Split('?')[1]).Split('=')[1]);

            ViewBag.Filiere = Bll_Filiere.SelectById(IdFiliere);
            ViewBag.Annee = Bll_AnneeUniversitaire.SelectByCode(CodeAnneeUniv);

            var ListNivParcours = Bll_AnneeUniversitaireNiveauParcoursPeriode.SelectAll(IdFiliere, CodeAnneeUniv);
            return View(ListNivParcours);
        }

        public IActionResult CreateNiveauParcours(string Code)
        {
            var Codes = Code.Split(',');
            var IdNiveauParcours = Convert.ToInt64(Codes[0]);
            var IdFiliere = Convert.ToInt64(Codes[1]);

            var CodeAnneeUniv = Codes[2];
            var Data = new SearchModelView();
            Data.NewNiveauParcours = new AnneeUniversitaireNiveauParcoursPeriode();

            if (IdNiveauParcours != -1)
                Data.NewNiveauParcours = Bll_AnneeUniversitaireNiveauParcoursPeriode.SelectById(IdNiveauParcours);

            Data.ListNiveaux = Bll_Niveau.SelectFiliereNiveaux(IdFiliere);
            Data.ListParcours = Bll_Parcours.SelectFiliereParcours(IdFiliere);

            return PartialView("CreateNiveauParcours", Data);
        }

        [HttpPost]
        public JsonResult StoreUpdateNiveauParcours(AnneeUniversitaireNiveauParcoursPeriode NewNiveauParcours)
        {
            AnneeUniversitaireNiveauParcoursPeriode AnneeNivPars;
            if (NewNiveauParcours.Id == 0)
            {
                // Vérifier si le NiveauParcours existe déjà pour cette filière pour l'année en cours
                if (Bll_AnneeUniversitaireNiveauParcoursPeriode.CheckAnneeUnivNiveauParcours(NewNiveauParcours.IdFiliere, NewNiveauParcours.IdNiveau, NewNiveauParcours.IdParcours, NewNiveauParcours.CodeAnneeUniv))
                {
                    Bll_AnneeUniversitaireNiveauParcoursPeriode.Add(NewNiveauParcours);
                    AnneeNivPars = Bll_AnneeUniversitaireNiveauParcoursPeriode.SelectByAnneeUnivNiv(
                                NewNiveauParcours.IdFiliere,
                                NewNiveauParcours.IdNiveau,
                                NewNiveauParcours.IdParcours,
                                NewNiveauParcours.CodeAnneeUniv);

                    return Json(new { Add = true, AnneeNivPars = AnneeNivPars });
                }
                else
                {
                    return Json(new { Add = false });
                }
            }
            else
            {
                Bll_AnneeUniversitaireNiveauParcoursPeriode.Update(NewNiveauParcours);

                return Json(new
                {
                    Add = true,
                    AnneeNivPars = Bll_AnneeUniversitaireNiveauParcoursPeriode.SelectById(NewNiveauParcours.Id)
                });
            }
        }

        [HttpPost]
        public JsonResult DeleteNiveauParcours(long Id)
        {
            Bll_AnneeUniversitaireNiveauParcoursPeriode.Delete(Id);

            return Json(true);
        }

        [HttpGet]
        public IActionResult PlanEtudeFiliere(string Filiere)
        {
            var IdFiliere = Convert.ToInt64(Filiere.Split('?')[0]);

            var CodeAnneeUniv = (Filiere.Split('?')[1]).Split('=')[1];

            ViewBag.Filiere = Bll_Filiere.SelectById(IdFiliere);
            ViewBag.Annee = Bll_AnneeUniversitaire.SelectByCode(CodeAnneeUniv);

            var Data = new SearchModelView();
            Data.ListNiveaux = Bll_Niveau.GetAll(IdFiliere, CodeAnneeUniv);
            Data.ListParcours = Bll_Parcours.GetAll(IdFiliere, CodeAnneeUniv);
            Data.ListTypePeriodes = Bll_TypePeriode.SelectAll();

            return View("PlanEtudeFiliere", Data);
        }
    }


}