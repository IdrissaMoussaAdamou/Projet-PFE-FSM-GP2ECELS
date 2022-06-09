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
    [Route("chargeenseignement")]
    public class ChargeEnseignementController : Controller
    {
        [HttpGet]
        [Route("index")]
        [Authorize]
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

        [HttpGet]
        [Route("consult/{Code}")]
        [Authorize]
        public IActionResult Consult(string Code)
        {
            List<string> ListDept = new List<string>();

            var l = Bll_Departement.SelectAll();
            if (l != null)
            {
                foreach (var Dept in l)
                {
                    ListDept.Add(Dept.IntituleFr);
                }
            }

            ViewBag.Annee = Code;
            ViewBag.ListDepartement = ListDept;

            var ListEnseignants = Bll_AnneeUniversitaireEnseignant.SelectAll(Code);
            var UserAccount = Bll_User.SelectByCIN(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (UserAccount.Profil == "ChefDepartement" || UserAccount.Profil == "AgentADDepartement")
            {
                return View(ListEnseignants.Where(Teacher => UserAccount.Affiliation.Contains(Teacher.IntituleFrDepartement)).ToList());
            }

            return View(ListEnseignants);
        }

        [HttpGet]
        [Route("details/{Code}")]
        [Authorize]
        public IActionResult Details(string Code)
        {
            List<string> ListDept = new List<string>();

            var l = Bll_Departement.SelectAll();
            if (l != null)
            {
                foreach (var Dept in l)
                {
                    ListDept.Add(Dept.IntituleFr);
                }
            }

            ViewBag.Annee = Code;
            ViewBag.ListDepartement = ListDept;

            return View(Bll_AnneeUniversitaireEnseignant.SelectAll(Code));
        }

        [HttpGet]
        [Route("parametres")]
        [Authorize]
        public IActionResult Parametres()
        {
            var ListTypeEncadrement = Bll_TypeEncadrement.SelectAll();
            ViewBag.ListTypeChargeDiverse = Bll_TypeChargeDiverse.SelectAll();

            return View(ListTypeEncadrement);
        }

        [HttpGet]
        [Route("archiverChargeEnseignement")]
        [Authorize]
        public IActionResult ArchiverChargeEnseignement(string Code)
        {
            Bll_AnneeUniversitaire.ArchiverChargeEnseignement(Code);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("selectDepartementsNames")]
        [Authorize]
        public JsonResult SelectDepartementsNames()
        {
            List<string> ListDept = null;

            var l = Bll_Departement.SelectAll();
            if (l != null)
            {
                ListDept = new List<string>();
                foreach (var Dept in l)
                {
                    ListDept.Add(Dept.IntituleFr);
                }
            }

            return Json(ListDept);
        }

        [HttpPost]
        [Route("filterenseignant")]
        [Authorize]
        public JsonResult FilterEnseignant(string FieldName, string FieldValue, string CodeAnneeUniv)
        {
            var ListEnseignants = Bll_AnneeUniversitaireEnseignant.SelectAll(FieldName, FieldValue, CodeAnneeUniv);
            if (ListEnseignants == null)
            {
                return Json(null);
            }

            var UserAccount = Bll_User.SelectByCIN(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (UserAccount.Profil == "ChefDepartement" || UserAccount.Profil == "AgentADDepartement")
            {
                return Json(ListEnseignants.Where(Teacher => UserAccount.Affiliation.Contains(Teacher.IntituleFrDepartement)).ToList());
            }

            return Json(ListEnseignants);

        }

        [HttpGet]
        [Route("importEnseignants")]
        [Authorize]
        public IActionResult ImportEnseignants(string Code)
        {
            var FieldName = Code.Split(",")[0];
            var FieldValue = Code.Split(",")[1];
            var CodeAnneeUniv = Code.Split(",")[2];

            var ListImportEnseignants = new List<ImportEnseignant>();

            var ListEnseignants = Bll_Enseignant.SelectAll(FieldName, FieldValue, CodeAnneeUniv);
            if (ListEnseignants != null)
            {
                foreach (var Teacher in ListEnseignants)
                    ListImportEnseignants.Add(new ImportEnseignant(Teacher));
            }


            var UserAccount = Bll_User.SelectByCIN(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (UserAccount.Profil == "ChefDepartement" || UserAccount.Profil == "AgentADDepartement")
            {
                return PartialView("ImportModalBody", ListImportEnseignants.Where(Teacher => UserAccount.Affiliation.Contains(Teacher.IntituleFrDepartement)).ToList());
            }
            return PartialView("ImportModalBody", ListImportEnseignants);
        }

        [HttpPost]
        [Route("storeImportEnseignants")]
        [Authorize]
        public JsonResult StoreImportEnseignants(string AnneeUniv, List<ImportEnseignant> ListImportEnseignants)
        {
            var ListEnseignants = new List<string>();

            foreach (var Teacher in ListImportEnseignants)
            {
                if (Teacher.Selected)
                {
                    Bll_AnneeUniversitaireEnseignant.Add(new AnneeUniversitaireEnseignant(Teacher, AnneeUniv));
                    ListEnseignants.Add(Teacher.CIN);
                }
            }

            return Json(Bll_AnneeUniversitaireEnseignant.SelectAll(ListEnseignants, AnneeUniv));
        }

        [HttpGet]
        [Route("createAnneeUniversitaireEnseignant")]
        [Authorize]
        public IActionResult CreateAnneeUniversitaireEnseignant()
        {
            var ListDepts = Bll_Departement.SelectAll();
            if (ListDepts.Count == 0)
                ListDepts = new List<Departement>();
            ViewBag.ListDepartement = ListDepts;
            return PartialView("../AnneeUniversitaireEnseignant/Create", new AnneeUniversitaireEnseignant());
        }

        [HttpPost]
        [Route("storeAnneeUniversitaireEnseignant")]
        [Authorize]
        public JsonResult StoreAnneeUniversitaireEnseignant(string codeAnneeUniv, AnneeUniversitaireEnseignant AnneeUnivE)
        {
            AnneeUnivE.CodeAnneeUniv = codeAnneeUniv;
            Bll_AnneeUniversitaireEnseignant.Add(AnneeUnivE);

            return Json(Bll_AnneeUniversitaireEnseignant.SelectTheLast());
        }

        [HttpGet]
        [Route("editAnneeUniversitaireEnseignant")]
        [Authorize]
        public IActionResult EditAnneeUniversitaireEnseignant(string Id)
        {
            var IdAnneeUnivE = Convert.ToInt64(Id);
            var ListDepts = Bll_Departement.SelectAll();
            if (ListDepts.Count == 0)
                ListDepts = new List<Departement>();
            ViewBag.ListDepartement = ListDepts;

            return PartialView("../AnneeUniversitaireEnseignant/Edit", Bll_AnneeUniversitaireEnseignant.SelectById(IdAnneeUnivE));
        }

        [HttpPost]
        [Route("updateAnneeUniversitaireEnseignant")]
        [Authorize]
        public JsonResult UpdateAnneeUniversitaireEnseignant(string codeAnneeUniv, AnneeUniversitaireEnseignant AnneeUnivE)
        {
            AnneeUnivE.CodeAnneeUniv = codeAnneeUniv;
            Bll_AnneeUniversitaireEnseignant.Update(AnneeUnivE);

            return Json(Bll_AnneeUniversitaireEnseignant.SelectById(AnneeUnivE.Id));
        }

        [HttpPost]
        [Route("anneeUniversitaireEnseignantInfos")]
        [Authorize]
        public JsonResult AnneeUniversitaireEnseignantInfos(long Id)
        {
            var Data = new
            {
                Teacher = Bll_AnneeUniversitaireEnseignant.SelectById(Id),
                TeacherChrgEnParModule = Bll_ChargeParModule.SelectAll(Id),
                TeacherChrgEncadrement = Bll_ChargeEncadrement.SelectAll(Id),
                TeacherChrgDivers = Bll_ChargeDiverse.SelectAll(Id),
                TeacherCharge = Bll_ChargeEnseignementParPeriode.SelectAll(Id)
            };

            return Json(Data);
        }

        [HttpPost]
        [Route("deleteAnneeUniversitaireEnseignant")]
        [Authorize]
        public JsonResult DeleteAnneeUniversitaireEnseignant(long Id)
        {

            if (Bll_AnneeUniversitaireEnseignant.IsForeignKeyInTable("ChargeParModule", Id))
            {
                return Json(new { Delete = false, Element = "une Charge Enseignement Par Module" });
            }

            if (Bll_AnneeUniversitaireEnseignant.IsForeignKeyInTable("ChargeEnseignementParPeriode", Id))
            {
                return Json(new { Delete = false, Element = "une Charge Enseignement Par Periode" });
            }

            if (Bll_AnneeUniversitaireEnseignant.IsForeignKeyInTable("ChargeDiverse", Id))
            {
                return Json(new { Delete = false, Element = "une Charge Divers" });
            }

            if (Bll_AnneeUniversitaireEnseignant.IsForeignKeyInTable("ChargeEncadrement", Id))
            {
                return Json(new { Delete = false, Element = "une Charge Encadrement" });
            }

            if (Bll_AnneeUniversitaireEnseignant.IsForeignKeyInTable("ChargeEnseignementTotaleAnnuelle ", Id))
            {
                return Json(new { Delete = false, Element = "une Charge Enseignement RecapAnnuel " });
            }
            Bll_AnneeUniversitaireEnseignant.Delete(Id);
            return Json(new { Delete = true, Id = Id });
        }

        [AcceptVerbs("Get", "POST")]
        [Route("verifyCIN")]
        public IActionResult VerifyCIN(string CIN)
        {

            var Enseignant = Bll_Enseignant.SelectByCIN(CIN);
            if (Enseignant == null)
            {
                return Json($"Le Numéro CIN {CIN} n'appartient à aucun Enseignant");
            }
            else
            {
                var AnneeUniv = Bll_AnneeUniversitaire.SelectTNArchivedAnneeUniv();
                var AnneeUnivE = Bll_AnneeUniversitaireEnseignant.SelectByAUCIN(AnneeUniv.Code, CIN);

                if (AnneeUnivE == null)
                    return Json(true);
                return Json($"Le Numéro CIN {CIN} appartient à un Enseignant De L'année en Cours");
            }
        }

        // ChargeParModule Actions

        [HttpGet]
        [Route("createEditChargeEParModule")]
        [Authorize]
        public IActionResult CreateEditChargeEParModule(string Id)
        {

            var TeacherId = Convert.ToInt64(Id.Split(",")[0]);
            var CIN = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            dynamic Result = ChargeEnseignementController.DeptValidation(TeacherId, CIN);
            //empêche la modification de la  charge lorsque qu'elle verouillée ou validée au niveau départal la saisie est vérouillée
            if (Bll_User.SelectByCIN(User.FindFirst(ClaimTypes.NameIdentifier).Value).Profil == "Enseignant" || Result.Edit == false)
            {
                if (!ChargeEnseignementController.EtatSaisieStatus(TeacherId))
                {
                    return PartialView("../ChargeParModule/Create", null);
                }
            }

            var IdChargEnParModule = Convert.ToInt64(Id.Split(",")[1]);
            ViewBag.ListFilieres = Bll_Filiere.SelectAll(TeacherId);
            var model = new ChargeParModule();
            if (IdChargEnParModule > 0)
            {
                model = Bll_ChargeParModule.SelectById(IdChargEnParModule);
                ViewBag.ListNiveaux = Bll_Niveau.SelectFiliereNiveaux(model.IdFiliere);
                ViewBag.Id = model.Id;

                // Module
                var Course = Bll_Module.SelectById(model.IdModule);
                if (Course.Nature == "Optionnelle")
                {
                    var Option = Bll_AnneeUniversitaireNomOption.SelectModuleOption(model.IdModule, model.CodeAnneeUniv);
                    if (Option != null)
                        Course.IntituleFr = Option.Intitule;
                }

                ViewBag.Course = Course;
            }
            else
            {
                ViewBag.Id = 0;
            }
            return PartialView("../ChargeParModule/Create", model);
        }

        [HttpPost]
        [Route("getNiveauxByFiliere")]
        [Authorize]
        public JsonResult GetNiveauxByFiliere(long IdFiliere) => Json(new
        {
            Filiere = Bll_Filiere.SelectById(IdFiliere),
            ListNiveaux = Bll_Niveau.SelectFiliereNiveaux(IdFiliere)
        });

        [HttpPost]
        [Route("getModulesByNiveauPeriode")]
        [Authorize]
        public JsonResult GetModulesByNiveauPeriode(long IdNiveau, string CodeAnneeUniv)
        {

            var ListCourses = Bll_Module.SelectAll(IdNiveau);
            if (ListCourses == null)
                return Json(null);
            else
            {
                var Courses = new List<Module>(ListCourses.Count);
                foreach (var CourseItem in ListCourses)
                {
                    if (CourseItem.Nature == "Optionnelle")
                    {
                        var Option = Bll_AnneeUniversitaireNomOption.SelectModuleOption(CourseItem.IdModule, CodeAnneeUniv);
                        if (Option != null)
                            CourseItem.IntituleFr = Option.Intitule;
                    }
                    Courses.Add(CourseItem);
                }

                return Json(Courses);
            }

        }

        [HttpPost]
        [Route("storeUpdateChargeParModule")]
        [Authorize]
        public JsonResult StoreUpdateChargeParModule(string codeAnneeUniv, ChargeParModule ChrgEParModule)
        {
            if (ChrgEParModule.Id > 0)
            {
                Bll_ChargeParModule.Update(ChrgEParModule);
                return Json(Bll_ChargeParModule.SelectById(ChrgEParModule.Id));
            }
            else
            {
                ChrgEParModule.CodeAnneeUniv = codeAnneeUniv;
                Bll_ChargeParModule.Add(ChrgEParModule);

                return Json(Bll_ChargeParModule.SelectTheLast());
            }
        }

        [HttpPost]
        [Route("deleteChargeParModule")]
        [Authorize]
        public JsonResult DeleteChargeParModule(long Id, long IdTeacher)
        {
            var CIN = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            dynamic Result = ChargeEnseignementController.DeptValidation(IdTeacher, CIN);

            if (Bll_User.SelectByCIN(User.FindFirst(ClaimTypes.NameIdentifier).Value).Profil == "Enseignant" || Result.Edit == false)
            {
                if (!ChargeEnseignementController.EtatSaisieStatus(IdTeacher))
                {
                    return Json(new { Delete = false });
                }

            }

            Bll_ChargeParModule.Delete(Id);
            return Json(new { Delete = true, Id = Id });
        }

        // ChargeParModule Actions

        [HttpGet]
        [Route("createEditChargeEncadrement")]
        [Authorize]
        public IActionResult CreateEditChargeEncadrement(string Ids)
        {
            var TeacherId = Convert.ToInt64(Ids.Split(",")[0]);
            var CIN = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            dynamic Result = ChargeEnseignementController.DeptValidation(TeacherId, CIN);

            //empêche la modification de la  charge lorsque qu'elle verouillée ou validée au niveau départal la saisie est vérouillée
            if (Bll_User.SelectByCIN(User.FindFirst(ClaimTypes.NameIdentifier).Value).Profil == "Enseignant" || Result.Edit == false)
            {
                if (!ChargeEnseignementController.EtatSaisieStatus(TeacherId))
                {
                    return PartialView("../ChargeEncadrement/CreateEdit", null);
                }
            }

            var IdChrgEncadrement = Convert.ToInt64(Ids.Split(",")[1]);
            ViewBag.ListTypeEncadrement = Bll_TypeEncadrement.SelectAll();

            if (IdChrgEncadrement > 0)
            {
                return PartialView("../ChargeEncadrement/CreateEdit", Bll_ChargeEncadrement.SelectById(IdChrgEncadrement));
            }

            return PartialView("../ChargeEncadrement/CreateEdit", new ChargeEncadrement());

        }

        [HttpPost]
        [Route("storeUpdateChargeEncadrement")]
        [Authorize]
        public JsonResult StoreUpdateChargeEncadrement(string codeAnnee, ChargeEncadrement ChrgEncadrement)
        {
            ChrgEncadrement.CodeAnneeUniv = codeAnnee;

            if (ChrgEncadrement.Id > 0)
            {
                Bll_ChargeEncadrement.Update(ChrgEncadrement);
                return Json(Bll_ChargeEncadrement.SelectById(ChrgEncadrement.Id));
            }
            else
            {
                Bll_ChargeEncadrement.Add(ChrgEncadrement);
                return Json(Bll_ChargeEncadrement.SelectTheLast());
            }

        }

        [HttpPost]
        [Route("deleteChargeEncadrement")]
        [Authorize]
        public JsonResult DeleteChargeEncadrement(long Id, long IdTeacher)
        {
            var CIN = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            dynamic Result = ChargeEnseignementController.DeptValidation(IdTeacher, CIN);

            if (Bll_User.SelectByCIN(User.FindFirst(ClaimTypes.NameIdentifier).Value).Profil == "Enseignant" || Result.Edit == false)
            {
                if (!ChargeEnseignementController.EtatSaisieStatus(IdTeacher))
                {
                    return Json(new { Delete = false });
                }

            }
            Bll_ChargeEncadrement.Delete(Id);
            return Json(new { Delete = true, Id = Id });
        }

        // ChargeDiverse Actions

        [HttpGet]
        [Route("createEditChargeDiverse")]
        [Authorize]
        public IActionResult CreateEditChargeDiverse(string Ids)
        {
            var TeacherId = Convert.ToInt64(Ids.Split(",")[0]);
            var CIN = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            dynamic Result = ChargeEnseignementController.DeptValidation(TeacherId, CIN);
            //empêche la modification lorsque la chargé est verouillée ou valider au niveau départemental la saisie est vérouillée
            if (Bll_User.SelectByCIN(User.FindFirst(ClaimTypes.NameIdentifier).Value).Profil == "Enseignant" || Result.Edit == false)
            {
                if (!ChargeEnseignementController.EtatSaisieStatus(TeacherId))
                {
                    return PartialView("../ChargeDiverse/CreateEdit", null);
                }
            }

            var IdChargeDiverse = Convert.ToInt64(Ids.Split(",")[1]);
            var model = new ChargeDiverse();
            if (IdChargeDiverse > 0)
            {
                model = Bll_ChargeDiverse.SelectById(IdChargeDiverse);
            }

            ViewBag.ListTypeChrgDivers = Bll_TypeChargeDiverse.SelectAll();
            return PartialView("../ChargeDiverse/CreateEdit", model);
        }

        [HttpPost]
        [Route("storeUpdateChargeDiverse")]
        [Authorize]
        public JsonResult StoreUpdateChargeDiverse(string AnneeUniv, ChargeDiverse ChrgDivers)
        {
            ChrgDivers.CodeAnneeUniv = AnneeUniv;
            if (ChrgDivers.Id > 0)
            {
                Bll_ChargeDiverse.Update(ChrgDivers);
                return Json(Bll_ChargeDiverse.SelectById(ChrgDivers.Id));
            }
            else
            {
                Bll_ChargeDiverse.Add(ChrgDivers);
                return Json(Bll_ChargeDiverse.SelectTheLast());
            }
        }

        [HttpPost]
        [Route("deleteChargeDiverse")]
        [Authorize]
        public JsonResult DeleteChargeDiverse(long Id, long IdTeacher)
        {
            var CIN = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            dynamic Result = ChargeEnseignementController.DeptValidation(IdTeacher, CIN);

            if (Bll_User.SelectByCIN(User.FindFirst(ClaimTypes.NameIdentifier).Value).Profil == "Enseignant" || Result.Edit == false)
            {
                if (!ChargeEnseignementController.EtatSaisieStatus(IdTeacher))
                {
                    return Json(new { Delete = false });
                }

            }

            Bll_ChargeDiverse.Delete(Id);
            return Json(new { Delete = true, Id = Id });
        }

        [HttpGet]
        [Route("changeMode")]
        public IActionResult ChangeMode(string mode)
        {
            if (Convert.ToInt32(mode) == 1)
            {
                return PartialView("OngletMode");
            }
            else
            {
                return PartialView("AccordionMode");
            }
        }

        [HttpGet]
        [Route("detailscharge/{Id}")]
        [Authorize(Policy = "Teacher")]
        public IActionResult DetailsCharge(long Id)
        {
            List<AnneeUniversitaire> OrderedListAnneeUniversitaires = new List<AnneeUniversitaire>();
            var ListAnneeUniversitaires = Bll_AnneeUniversitaire.SelectAll();
            if (ListAnneeUniversitaires.Count > 0)
            {
                for (int i = ListAnneeUniversitaires.Count - 1; i >= 0; i--)
                    OrderedListAnneeUniversitaires.Add(ListAnneeUniversitaires[i]);
            }


            return View("DetailParEnseignant", OrderedListAnneeUniversitaires);
        }

        [HttpPost]
        [Route("anneeUniversitaireteacherinfos")]
        [Authorize(Policy = "Teacher")]
        public JsonResult AnneeUniversitaireTeacherInfos(string CodeAnneeUniv)
        {
            var CIN = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var Teacher = Bll_AnneeUniversitaireEnseignant.SelectByAUCIN(CodeAnneeUniv, CIN);

            var Data = new
            {
                Teacher = Teacher,
                TeacherChrgEnParModule = Bll_ChargeParModule.SelectAll(Teacher.Id),
                TeacherChrgEncadrement = Bll_ChargeEncadrement.SelectAll(Teacher.Id),
                TeacherChrgDivers = Bll_ChargeDiverse.SelectAll(Teacher.Id),
                TeacherCharge = Bll_ChargeEnseignementParPeriode.SelectAll(Teacher.Id)
            };

            return Json(Data);
        }

        // WorkFlow

        [HttpPost]
        [Route("lockcharge")]
        public JsonResult LockCharge(long IdTeacher)
        {

            var CIN = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            dynamic Result = ChargeEnseignementController.checkedLockOrUnLockPermission(IdTeacher, "lock", CIN);

            if (!Result.authorized)
            {
                return Json(new
                {
                    Title = Result.Title,
                    Message = Result.Message,
                });
            }

            // Vérouiller la charge
            Bll_AnneeUniversitaireEnseignant.LockOrUnLoCkCharge(IdTeacher, "lock");
            return Json(new
            {
                Title = "Processus De Validation De Charge",
                Message = $"La Charge  D\'Enseignement de {Result.Name} est verrouillée",
                teacher = Bll_AnneeUniversitaireEnseignant.SelectById(IdTeacher)
            });
        }

        [HttpPost]
        [Route("unLockCharge")]
        [Authorize]
        public JsonResult unLockCharge(long IdTeacher)
        {

            var CIN = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            dynamic Result = ChargeEnseignementController.checkedLockOrUnLockPermission(IdTeacher, "unlock", CIN);

            if (!Result.authorized)
            {
                return Json(new
                {
                    Title = Result.Title,
                    Message = Result.Message
                });
            }

            // déverrouiller la charge
            Bll_AnneeUniversitaireEnseignant.LockOrUnLoCkCharge(IdTeacher, "unlock");
            return Json(new
            {
                Title = "Processus De Validation De Charge",
                Message = $"La Charge  D\'Enseignement de {Result.Name} est déverrouillée",
                teacher = Bll_AnneeUniversitaireEnseignant.SelectById(IdTeacher)
            });
        }

        // vérifier l'authorisation pour le vérouillage et déverrouillage
        private static object checkedLockOrUnLockPermission(long IdTeacher, string Keyword, string CIN)
        {
            var Teacher = Bll_AnneeUniversitaireEnseignant.SelectById(IdTeacher);
            if (Teacher == null)
            {
                return new
                {
                    authorized = false,
                    Title = "Processus De Validation De Charge",
                    Message = "La Charge  D\'Enseignement  n'existe pas \nContactez l'administrateur si nécessaire"
                };
            }

            // vérifier le Profil de l'utilisateur
            //la charge ne peut être verrouillée/déverrouillée que par l'agent administratif ou l'agent de département de l'enseignant
            var UserAccount = Bll_User.SelectByCIN(CIN);
            if (UserAccount.Profil == "DirecteurEtude" || UserAccount.Profil == "SuperAdministrateur" || UserAccount.Profil == "ChefDepartement")
            {
                return new
                {
                    authorized = false,
                    Title = "Opération Non Permise",
                    Message = $"Vous n'êtes pas habilité à faire cette action",
                    Name = ""
                };
            }
            if (Teacher.EtatSaisie == "Verifiee")
            {
                return new
                {
                    authorized = false,
                    Title = "Processus De Validation De Charge",
                    Message = "La Charge  D\'Enseignement  a été  marquée comme vérifiée",
                    Name = string.Concat(Teacher.Nom, " ", Teacher.Prenom)
                };
            }

            // vérifier l'état de saisie de la charge 
            if (Keyword == "lock")
            {
                if (Teacher.EtatSaisie == "Verrouillee")
                {
                    return new
                    {
                        authorized = false,
                        Title = "Processus De Validation De Charge",
                        Message = "La Charge  D\'Enseignement  a été  verrouillée par une action antérieure",
                        Name = string.Concat(Teacher.Nom, " ", Teacher.Prenom)
                    };
                }
            }
            else
            {
                if (Teacher.EtatSaisie == "En Cours")
                {
                    return new
                    {
                        authorized = false,
                        Title = "Processus De Validation De Charge",
                        Message = "La Charge  D\'Enseignement  est  déjà en Cours de Saisie",
                        Name = string.Concat(Teacher.Nom, " ", Teacher.Prenom)
                    };
                }

                if (Teacher.ValidationChargeDepartement == "Validee")
                {
                    return new
                    {
                        authorized = false,
                        Title = "Processus De Validation De Charge",
                        Message = "La Charge  D\'Enseignement  est  validée au niveau du département\n impossible de la déverrouillée",
                        Name = string.Concat(Teacher.Nom, " ", Teacher.Prenom)
                    };
                }
            }

            return new { authorized = true, Name = string.Concat(Teacher.Nom, " ", Teacher.Prenom) };
        }

        [HttpPost]
        [Route("chargeverified")]
        [Authorize]
        public JsonResult ChargeVerified(long IdTeacher)
        {
            var CIN = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            dynamic Result = ChargeEnseignementController.checkedPermissionChargeVerified(IdTeacher, CIN);

            if (!Result.authorized)
            {
                return Json(new
                {
                    Title = Result.Title,
                    Message = Result.Message,
                });
            }

            // marquée la charge commevérifiée
            Bll_AnneeUniversitaireEnseignant.ChargeVerified(IdTeacher);

            // Calculer La charge
            var Teacher = Bll_AnneeUniversitaireEnseignant.SelectById(IdTeacher);
            Bll_ChargeEnseignementParPeriode.ManageTeacherChargeParPeriode(Teacher);

            return Json(new
            {
                Title = "Processus De Validation De Charge",
                Message = $"La Charge  D\'Enseignement de {Result.Name} est marquée comme vérifiée",
                Teacher = Teacher,
                TeacherCharge = Bll_ChargeEnseignementParPeriode.SelectAll(IdTeacher)
            });

        }

        private static object checkedPermissionChargeVerified(long IdTeacher, string CIN)
        {
            var Teacher = Bll_AnneeUniversitaireEnseignant.SelectById(IdTeacher);
            if (Teacher == null)
            {
                return new
                {
                    authorized = false,
                    Title = "Processus De Validation De Charge",
                    Message = "La Charge  D\'Enseignement  n'existe pas \nContactez l'administrateur si nécessaire"
                };
            }

            // vérifier le Profil de l'utilisateur
            //la charge ne peut être marquée comme vérifiée que par l'agent administratif ou l'agent de département de l'enseignant
            var UserAccount = Bll_User.SelectByCIN(CIN);
            if (UserAccount.Profil == "DirecteurEtude" || UserAccount.Profil == "SuperAdministrateur" || UserAccount.Profil == "ChefDepartement")
            {
                return new
                {
                    authorized = false,
                    Title = "Opération Non Permise",
                    Message = $"Vous n'êtes pas habilité à faire cette action",
                    Name = ""
                };
            }

            if (Teacher.EtatSaisie == "En Cours")
            {
                return new
                {
                    authorized = false,
                    Title = "Processus De Validation De Charge",
                    Message = "La Charge  D\'Enseignement  est  déjà en Cours de Saisie Veuillez la verrouilée",
                    Name = string.Concat(Teacher.Nom, " ", Teacher.Prenom)
                };
            }


            if (Teacher.EtatSaisie == "Verifiee")
            {
                return new
                {
                    authorized = false,
                    Title = "Processus De Validation De Charge",
                    Message = "La Charge  D\'Enseignement  a été  marquée comme vérifiée par une action antérieure",
                    Name = string.Concat(Teacher.Nom, " ", Teacher.Prenom)
                };
            }

            return new { authorized = true, Name = string.Concat(Teacher.Nom, " ", Teacher.Prenom) };
        }

        [HttpPost]
        [Route("checkEtatsaisie")]
        [Authorize(Policy = "Teacher")]
        public JsonResult CheckEtatSaisie(long Id)
        {
            var Teacher = Bll_AnneeUniversitaireEnseignant.SelectById(Id);

            var Result = Teacher.EtatSaisie == "En Cours" ? true : false;
            return Json(Result);
        }

        [HttpPost]
        [Route("checkdeptvalidation")]
        [Authorize]
        public JsonResult checkDeptValidation(long Id)
        {
            var CIN = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            dynamic Result = ChargeEnseignementController.DeptValidation(Id, CIN);

            if (!Result.Authorized)
            {
                return Json(new { Edit = false, Title = "Opération non permise", Message = "Vous n'êtes pas habilité à faire cette action" });
            }
            if (!Result.Edit)
            {
                return Json(new { Edit = false, Title = "Modification Impossible", Message = "la charge est marquée comme vérifiée, impossible de la modifier" });
            }

            return Json(new { Edit = true });

        }

        private static bool EtatSaisieStatus(long IdTeacher)
        {
            var Teacher = Bll_AnneeUniversitaireEnseignant.SelectById(IdTeacher);
            return (Teacher.EtatSaisie == "En Cours" ? true : false);
        }

        private static object DeptValidation(long IdTeacher, string CIN)
        {

            var User = Bll_User.SelectByCIN(CIN);
            var Teacher = Bll_AnneeUniversitaireEnseignant.SelectById(IdTeacher);
            if (User.Profil == "DirecteurEtude" || User.Profil == "SuperAdministrateur" || User.Profil == "ChefDepartement")
            {
                return new { Authorized = false, Edit = false };
            }

            return new { Authorized = true, Edit = Teacher.EtatSaisie == "Verifiee" ? false : true };
        }

        [HttpPost]
        [Route("validatecharge")]
        [Authorize]
        public JsonResult ValidateCharge(long IdTeacher)
        {
            var CIN = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            dynamic Result = ChargeEnseignementController.checkedValidateOrInvaldate(IdTeacher, "validate", CIN);

            if (!Result.authorized)
            {
                return Json(new
                {
                    Title = Result.Title,
                    Message = Result.Message,
                });
            }


            var UserAccount = Bll_User.SelectByCIN(CIN);
            if (UserAccount.Profil == "ChefDepartement")
            {
                // Valider la charge
                Bll_AnneeUniversitaireEnseignant.ValidateOrUnValidate(IdTeacher, "Departement", "validate");
            }
            else
            {
                Bll_AnneeUniversitaireEnseignant.ValidateOrUnValidate(IdTeacher, "Administration", "validate");
            }

            return Json(new
            {
                Title = "Processus De Validation De Charge",
                Message = $"La Charge  D\'Enseignement de {Result.Name} est validée",
                Teacher = Bll_AnneeUniversitaireEnseignant.SelectById(IdTeacher)

            });
        }


        [HttpPost]
        [Route("invalidatecharge")]
        [Authorize]
        public JsonResult InValidateCharge(long IdTeacher)
        {
            var CIN = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            dynamic Result = ChargeEnseignementController.checkedValidateOrInvaldate(IdTeacher, "invalidate", CIN);

            if (!Result.authorized)
            {
                return Json(new
                {
                    Title = Result.Title,
                    Message = Result.Message
                });
            }


            var UserAccount = Bll_User.SelectByCIN(CIN);
            if (UserAccount.Profil == "ChefDepartement")
            {
                // invalider la charge
                Bll_AnneeUniversitaireEnseignant.ValidateOrUnValidate(IdTeacher, "Departement", "invalidate");
            }
            else
            {
                Bll_AnneeUniversitaireEnseignant.ValidateOrUnValidate(IdTeacher, "Administration", "invalidate");
            }

            return Json(new
            {
                Title = "Processus De Validation De Charge",
                Message = Result.Message,
                Teacher = Bll_AnneeUniversitaireEnseignant.SelectById(IdTeacher)
            });


        }


        // vérifier l'authorisation pour le vérouillage et déverrouillage
        private static object checkedValidateOrInvaldate(long IdTeacher, string Keyword, string CIN)
        {
            var Teacher = Bll_AnneeUniversitaireEnseignant.SelectById(IdTeacher);
            if (Teacher == null)
            {
                return new
                {
                    authorized = false,
                    Title = "Processus De Validation De Charge",
                    Message = "La Charge  D\'Enseignement  n'existe pas \nContactez l'administrateur si nécessaire",
                    Name = ""
                };
            }

            // vérifier le Profil de l'utilisateur
            var UserAccount = Bll_User.SelectByCIN(CIN);

            // Chef de département
            if (UserAccount.Profil == "ChefDepartement")
            {

                // vérifier l'opération valider/invalider
                if (Keyword == "validate")
                {
                    // vérifier que la charge est vérrouilléé
                    if (Teacher.EtatSaisie == "En Cours" || Teacher.EtatSaisie == "Verrouillee")
                    {
                        return new
                        {
                            authorized = false,
                            Title = "Processus De Validation De Charge",
                            Message = $"la charge  d\'enseignement n'est pas encore vérifiée",
                            Name = string.Concat(Teacher.Nom, " ", Teacher.Prenom)
                        };
                    }

                    if (Teacher.ValidationChargeDepartement == "Validee")
                    {
                        return new
                        {
                            authorized = false,
                            Title = "Processus De Validation De Charge",
                            Message = $"la charge  d\'enseignement a été validée par une action antérieur",
                            Name = string.Concat(Teacher.Nom, " ", Teacher.Prenom)
                        };
                    }

                    return new { authorized = true, Name = string.Concat(Teacher.Nom, " ", Teacher.Prenom) };
                }
                else
                {
                    // vérifier l'état de saisie de la charge
                    if (Teacher.EtatSaisie == "Verifiee" && Teacher.ValidationChargeDepartement == "Non Validee")
                    {
                        return new
                        {
                            authorized = true,
                            Title = "Processus De Validation De Charge",
                            Message = $"la charge  d\'enseignement est déverouillée pour les agents"
                        };
                    }
                    // vérifier l'état de validation la charge
                    if (Teacher.ValidationChargeDepartement == "Non Validee")
                    {
                        return new
                        {
                            authorized = false,
                            Title = "Processus De Validation De Charge",
                            Message = $"la charge  d\'enseignement se trouve dans un état  invalidé",
                            Name = string.Concat(Teacher.Nom, " ", Teacher.Prenom)
                        };
                    }

                    // vérifier l'etat de validation la charge au niveau administrative 
                    if (Teacher.ValidationChargeAdministration == "Validee")
                    {
                        return new
                        {
                            authorized = false,
                            Title = "Opération Non Permise",
                            Message = $"la charge  d\'enseignement est validée au niveau Administrative Veuillez Contacter l'administration si nécessaire",
                            Name = string.Concat(Teacher.Nom, " ", Teacher.Prenom)
                        };
                    }

                    return new { authorized = true, Message = $"La Charge  D\'Enseignement de {string.Concat(Teacher.Nom, " ", Teacher.Prenom)} est invalidée" };
                }


            }

            // Directeur d'Etude
            if (UserAccount.Profil == "DirecteurEtude")
            {
                if (Keyword == "validate")
                {
                    if (Teacher.ValidationChargeDepartement == "Non Validee")
                    {
                        return new
                        {
                            authorized = false,
                            Title = "Processus De Validation De Charge",
                            Message = $"la charge d\'enseignement n\'est pas validée par le Directeur du département { Teacher.IntituleFrDepartement }",
                            Name = ""
                        };

                    }
                    if (Teacher.ValidationChargeAdministration == "Validee")
                    {
                        return new
                        {
                            authorized = false,
                            Title = "Processus De Validation De Charge",
                            Message = $"la charge d\'enseignement a été  validée par une action antérieure",
                            Name = string.Concat(Teacher.Nom, " ", Teacher.Prenom)
                        };

                    }
                    // Vérifier l'état de charge de l'année
                    /*if(Bll_AnneeUniversitaire.SelectByCode(Teacher.CodeAnneeUniv).EtatCharges == "Cloturee")
                    {
                        return new {
                        authorized = false,
                        Title = "Opération Non Permise",
                        Message = "L'année est cloturée pour toute opération de modification de charge d'enseignement" };

                    }*/

                    return new { authorized = true, Name = string.Concat(Teacher.Nom, " ", Teacher.Prenom) };

                }
                else
                {
                    if (Teacher.ValidationChargeDepartement == "Non Validee")
                    {
                        return new
                        {
                            authorized = false,
                            Title = "Processus De Validation De Charge",
                            Message = $"la charge  d\'enseignement se trouve dans un état  invalidé",
                            Name = string.Concat(Teacher.Nom, " ", Teacher.Prenom)
                        };

                    }
                    if (Teacher.ValidationChargeAdministration == "Non Validee")
                    {
                        return new
                        {
                            authorized = false,
                            Title = "Processus De Validation De Charge",
                            Message = $"la charge d\'enseignement se trouve dans un état invalide",
                            Name = string.Concat(Teacher.Nom, " ", Teacher.Prenom)
                        };

                    }

                    /*if(Bll_AnneeUniversitaire.SelectByCode(Teacher.CodeAnneeUniv).EtatCharges == "Cloturee")
                    {
                        return new {
                        authorized = false,
                        Title = "Opération Non Permise",
                        Message = "L'année est cloturée pour toute opération de modification de charge d'enseignement" };

                    }*/
                    return new { authorized = true, Message = $"la charge de {string.Concat(Teacher.Nom, " ", Teacher.Prenom)} est invalidée" };

                }
            }


            return new
            {
                authorized = false,
                Title = "Opération Non Permise",
                Message = "Vous n\'êtes pas habilité à faire cette action",
                Name = ""
            };

        }

        [HttpPost]
        [Route("lockcharges")]
        [Authorize]
        public JsonResult Lockcharges(string StrIds)
        {
            var Ids = JsonConvert.DeserializeObject<List<int>>(StrIds);
            if (Ids.Count <= 0)
            {
                return Json(null);
            }

            dynamic Result;
            var CIN = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Result = ChargeEnseignementController.MaxLockOrUnlockcharges(CIN, Ids, "lock");
            if (!Result.authorized)
            {
                return Json(Result);
            }

            //Vérouillé les charges
            foreach (var IdTeacher in Result.LockChargeIds)
            {
                Bll_AnneeUniversitaireEnseignant.LockOrUnLoCkCharge(IdTeacher, "lock");
            }
            // l'ensemble des à a été vérouillé
            return Json(new
            {
                Title = "Processus De Validation",
                Message = "Les charges sélectionnées ont été vérouillées, Veuillez actualiser la page pour voir les modifications",
            });
        }

        [HttpPost]
        [Route("unlockcharges")]
        [Authorize]
        public JsonResult UnLockcharges(string StrIds)
        {
            var Ids = JsonConvert.DeserializeObject<List<int>>(StrIds);
            if (Ids.Count <= 0)
            {
                return Json(null);
            }

            dynamic Result;
            var CIN = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Result = ChargeEnseignementController.MaxLockOrUnlockcharges(CIN, Ids, "unlock");
            if (!Result.authorized)
            {
                return Json(Result);
            }

            //Vérouillé les charges
            foreach (var IdTeacher in Result.LockChargeIds)
            {
                Bll_AnneeUniversitaireEnseignant.LockOrUnLoCkCharge(IdTeacher, "unlock");
            }
            // l'ensemble des à a été vérouillé
            return Json(new
            {
                Title = "Processus De Validation",
                Message = "Les charges sélectionnées ont été déverrouillées, Veuillez actualiser la page pour voir les modifications",
            });
        }
        private static object MaxLockOrUnlockcharges(string CIN, List<int> Ids, string Keyword)
        {
            dynamic Result;
            var LockChargeIds = new List<int>();

            foreach (var IdTeacher in Ids)
            {
                Result = ChargeEnseignementController.checkedLockOrUnLockPermission(IdTeacher, Keyword, CIN);
                //arrêter le vérouillage si une charge ne peut pas être vérouiller/déverrouilller
                if (!Result.authorized)
                {
                    return new
                    {
                        authorized = false,
                        Title = Result.Title,
                        Message = string.Concat(Result.Message, ". ", Result.Name)
                    };
                }
                else
                {
                    LockChargeIds.Add(IdTeacher);
                }
            }

            return new
            {
                authorized = true,
                LockChargeIds = LockChargeIds
            };
        }


        [HttpPost]
        [Route("validatecharges")]
        [Authorize]
        public JsonResult ValidateCharges(string StrIds)
        {
            var Ids = JsonConvert.DeserializeObject<List<int>>(StrIds);
            if (Ids.Count <= 0)
            {
                return Json(null);
            }

            dynamic Result;
            var CIN = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Result = ChargeEnseignementController.MaxvalidateOrUnUnvalidateCharges(CIN, Ids, "validate");
            if (!Result.authorized)
            {
                return Json(Result);
            }

            var UserAccount = Bll_User.SelectByCIN(CIN);
            //Vérouillé les charges
            foreach (var IdTeacher in Result.LockChargeIds)
            {
                if (UserAccount.Profil == "ChefDepartement")
                {
                    // Valider la charge
                    Bll_AnneeUniversitaireEnseignant.ValidateOrUnValidate(IdTeacher, "Departement", "validate");
                }
                else
                {
                    Bll_AnneeUniversitaireEnseignant.ValidateOrUnValidate(IdTeacher, "Administration", "validate");
                }

            }
            // l'ensemble des à a été vérouillé
            return Json(new
            {
                Title = "Processus De Validation",
                Message = "Les charges sélectionnées ont été Validées, Veuillez actualiser la page pour voir les modifications",
            });

        }

        [HttpPost]
        [Route("unvalidatecharges")]
        [Authorize]
        public JsonResult UnValidateCharges(string StrIds)
        {
            var Ids = JsonConvert.DeserializeObject<List<int>>(StrIds);
            if (Ids.Count <= 0)
            {
                return Json(null);
            }

            dynamic Result;
            var CIN = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Result = ChargeEnseignementController.MaxvalidateOrUnUnvalidateCharges(CIN, Ids, "unvalidate");
            if (!Result.authorized)
            {
                return Json(Result);
            }

            var UserAccount = Bll_User.SelectByCIN(CIN);
            //Vérouillé les charges
            foreach (var IdTeacher in Result.LockChargeIds)
            {
                if (UserAccount.Profil == "ChefDepartement")
                {
                    // Valider la charge
                    Bll_AnneeUniversitaireEnseignant.ValidateOrUnValidate(IdTeacher, "Departement", "unvalidate");
                }
                else
                {
                    Bll_AnneeUniversitaireEnseignant.ValidateOrUnValidate(IdTeacher, "Administration", "unvalidate");
                }

            }
            // l'ensemble des à a été vérouillé
            return Json(new
            {
                Title = "Processus De Validation",
                Message = "Les charges sélectionnées ont été inValidées, Veuillez actualiser la page pour voir les modifications",
            });

        }

        private static object MaxvalidateOrUnUnvalidateCharges(string CIN, List<int> Ids, string Keyword)
        {
            dynamic Result;
            var LockChargeIds = new List<int>();

            foreach (var IdTeacher in Ids)
            {
                Result = ChargeEnseignementController.checkedValidateOrInvaldate(IdTeacher, Keyword, CIN);
                //arrêter la validation si une charge ne peut pas être vérouiller/déverrouilller
                if (!Result.authorized)
                {
                    return new
                    {
                        authorized = false,
                        Title = Result.Title,
                        Message = string.Concat(Result.Message, ". ", Result.Name)
                    };
                }
                else
                {
                    LockChargeIds.Add(IdTeacher);
                }
            }

            return new
            {
                authorized = true,
                LockChargeIds = LockChargeIds
            };
        }

    }
}
