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
    public class SessionExamenController : Controller
    {
        public IActionResult Index(long ids, string niveau)
        {
            ViewBag.ids = ids;
            ViewBag.niveau = niveau;
            var ls = Bll_SessionSection.SelectByNiveau(ids, niveau);
            ViewBag.listsection = ls;
            List<SessionJour> L = Bll_SessionJour.SelectAll(ids);
            L.Sort((a, b) => a.Jour.CompareTo(b.Jour));

            // - 
            var idSesssionJour = L.Select(x => x.Id)?.ToList();
            var niveaux = ls.Select(x => x.Niveau)?.ToList();
            var cells = Bll_SessionExamen.SelectAll(ids, idSesssionJour, niveaux);
            // - 

            var _niveaux = ls.Select(x => x.Niveau)?.ToList();
            var _jours = L.Select(x => x.Jour.ToLongDateString())?.ToList();
            var data = new List<List<CellVM>> { };
            for (int i = 0; i < ls.Count; i++)
            {
                var _line = new List<CellVM>();
                for (int j = 0; j < L.Count; j++)
                {
                    _line.Add(new CellVM { Id=-1, Id2 = -1, SessionId = ids, JourId = L[j].Id, JourName = L[j].Jour.ToLongDateString(), SectionId = ls[i].Id, SectionNiveau = ls[i].Niveau });
                }
                data.Add(_line);
            }

            // -
            foreach (var cellItem in cells)
            {
                {
                    var jour = L.Find(x => x.Id == cellItem.IdSessionJour);
                    var section = ls.Find(x => x.Niveau.Trim().ToLower() == cellItem.Niveau.Trim().ToLower());

                    if (jour != null && section != null)
                    {
                        var jourIdx = _jours.FindIndex(x => x == jour.Jour.ToLongDateString());
                        var sectionIdx = _niveaux.FindIndex(x => x.Trim().ToLower() == section.Niveau.Trim().ToLower());

                        // - trouver son numcell 2
                        var numcell2 = cells.Find(x => x.IdSession == ids && x.IdSessionJour == cellItem.IdSessionJour && x.Niveau == cellItem.Niveau && x.numcell != cellItem.numcell);
                        if (data[sectionIdx][jourIdx].Id == -1)
                        {
                            if (cellItem.numcell == 1)
                            {
                                data[sectionIdx][jourIdx] = new CellVM
                                {
                                    Id=cellItem.Id,
                                    JourId = jour.Id,
                                    JourName = jour.Jour.ToLongDateString(),
                                    SectionId = section.Id,
                                    SectionNiveau = section.Niveau,
                                    SessionId = ids,
                                    Intitule = cellItem.Intitule,
                                    HD = cellItem.HeureDebut,
                                    HF = cellItem.HeureFin,
                                    Intitule2 = numcell2 != null ? numcell2.Intitule : "",
                                    HD2 = numcell2 != null ? numcell2.HeureDebut : "",
                                    HF2 = numcell2 != null ? numcell2.HeureFin : "",
                                    Id2 = numcell2 != null ? numcell2.Id : -1
                                };
                            }
                            else
                            {
                                data[sectionIdx][jourIdx] = new CellVM
                                {
                                    Id2 = cellItem.Id,
                                    JourId = jour.Id,
                                    JourName = jour.Jour.ToLongDateString(),
                                    SectionId = section.Id,
                                    SectionNiveau = section.Niveau,
                                    SessionId = ids,
                                    Intitule2 = cellItem.Intitule,
                                    HD2 = cellItem.HeureDebut,
                                    HF2 = cellItem.HeureFin,
                                    Intitule = numcell2 != null ? numcell2.Intitule : "",
                                    HD = numcell2 != null ? numcell2.HeureDebut : "",
                                    HF = numcell2 != null ? numcell2.HeureFin : "",
                                     Id = numcell2 != null ? numcell2.Id : -1
                                };
                            }
                        }                    
                    }

                }
            }

            // -
            return View(new SessionJourVM
            {
                Niveaux = ls.Select(x => x.Niveau)?.ToList(),
                Jours = L.Select(x=> x.Jour.ToLongDateString())?.ToList(),
                Cells = data
            }) ;
        }

        public IActionResult CreateEditSessionExamen(long ids, long idjour, long idsection, int numcell)
        {
            var model = new SessionExamen();
            var section = Bll_SessionSection.SelectById(idsection);
            var niveau = Bll_Niveau.SelectByIntituleAbr(section.Niveau);
            model.IdSession = ids;
            model.numcell = numcell;
            model.IdSessionJour = idjour;
            model.CodeFiliere = section.CodeFiliere;
            model.CodeParcours = section.CodeParcours;
            model.Niveau = section.Niveau;
            model.NbEtudiants = section.NbEtudiants;
            model.Periode = section.Periode;
            ViewBag.listeseance = Bll_SessionSeance.SelectAll(ids);
            ViewBag.listmodule = Bll_Module.SelectAll(niveau.Id);

            return PartialView("../SessionExamen/Create", model);
        }

        [HttpPost]
        public IActionResult StoreUpdateSE(long IdMod, long IdSean, SessionExamen S)
        {
            object[] obj = new object[2];
            var Module = Bll_Module.SelectById(IdMod);
            var seance = Bll_SessionSeance.SelectById(IdSean);
            S.IdSessionSeance = IdSean;
            S.HeureDebut = seance.HeureDebut;
            S.HeureFin = seance.HeureFin;
            S.CodeModule = Module.Code;
            S.Intitule = Module.IntituleFr;
            S.Nature = Module.Nature;
           
                obj[0] = Bll_SessionExamen.Add(S);


                obj[1] = Bll_SessionExamen.SelectById(S.Id);
           
             return Json(obj);
        }

        [HttpGet]
        public JsonResult DeleteCalCell(long ids, long idj, long idsec, long nbc)
        {
            //var Session = Bll_Session.SelectById(id);

            //if (Session == null)
            //{
            //    return Json(new { Delete = false, Code = id });
            //}
            Bll_SessionExamen.Delete(ids,idj, Bll_SessionSection.SelectById(idsec).Niveau, nbc);
            return Json(new { Delete = true, Code = ids });
        }

        public IActionResult Imprimer(long ids, string niveau)
        {
            ViewBag.ids = ids;
            var ls = Bll_SessionSection.SelectByNiveau(ids, niveau);
            ViewBag.listsection = ls;
            List<SessionJour> L = Bll_SessionJour.SelectAll(ids);
            L.Sort((a, b) => a.Jour.CompareTo(b.Jour));

            // - 
            var idSesssionJour = L.Select(x => x.Id)?.ToList();
            var niveaux = ls.Select(x => x.Niveau)?.ToList();
            var cells = Bll_SessionExamen.SelectAll(ids, idSesssionJour, niveaux);
            // - 

            var _niveaux = ls.Select(x => x.Niveau)?.ToList();
            var _jours = L.Select(x => x.Jour.ToLongDateString())?.ToList();
            var data = new List<List<CellVM>> { };
            for (int i = 0; i < ls.Count; i++)
            {
                var _line = new List<CellVM>();
                for (int j = 0; j < L.Count; j++)
                {
                    _line.Add(new CellVM { Id = -1, Id2 = -1, SessionId = ids, JourId = L[j].Id, JourName = L[j].Jour.ToLongDateString(), SectionId = ls[i].Id, SectionNiveau = ls[i].Niveau });
                }
                data.Add(_line);
            }

            // -
            foreach (var cellItem in cells)
            {
                {
                    var jour = L.Find(x => x.Id == cellItem.IdSessionJour);
                    var section = ls.Find(x => x.Niveau.Trim().ToLower() == cellItem.Niveau.Trim().ToLower());

                    if (jour != null && section != null)
                    {
                        var jourIdx = _jours.FindIndex(x => x == jour.Jour.ToLongDateString());
                        var sectionIdx = _niveaux.FindIndex(x => x.Trim().ToLower() == section.Niveau.Trim().ToLower());

                        // - trouver son numcell 2
                        var numcell2 = cells.Find(x => x.IdSession == ids && x.IdSessionJour == cellItem.IdSessionJour && x.Niveau == cellItem.Niveau && x.numcell != cellItem.numcell);
                        if (data[sectionIdx][jourIdx].Id == -1)
                        {
                            if (cellItem.numcell == 1)
                            {
                                data[sectionIdx][jourIdx] = new CellVM
                                {
                                    Id = cellItem.Id,
                                    JourId = jour.Id,
                                    JourName = jour.Jour.ToLongDateString(),
                                    SectionId = section.Id,
                                    SectionNiveau = section.Niveau,
                                    SessionId = ids,
                                    Intitule = cellItem.Intitule,
                                    HD = cellItem.HeureDebut,
                                    HF = cellItem.HeureFin,
                                    Intitule2 = numcell2 != null ? numcell2.Intitule : "",
                                    HD2 = numcell2 != null ? numcell2.HeureDebut : "",
                                    HF2 = numcell2 != null ? numcell2.HeureFin : "",
                                    Id2 = numcell2 != null ? numcell2.Id : -1
                                };
                            }
                            else
                            {
                                data[sectionIdx][jourIdx] = new CellVM
                                {
                                    Id2 = cellItem.Id,
                                    JourId = jour.Id,
                                    JourName = jour.Jour.ToLongDateString(),
                                    SectionId = section.Id,
                                    SectionNiveau = section.Niveau,
                                    SessionId = ids,
                                    Intitule2 = cellItem.Intitule,
                                    HD2 = cellItem.HeureDebut,
                                    HF2 = cellItem.HeureFin,
                                    Intitule = numcell2 != null ? numcell2.Intitule : "",
                                    HD = numcell2 != null ? numcell2.HeureDebut : "",
                                    HF = numcell2 != null ? numcell2.HeureFin : "",
                                    Id = numcell2 != null ? numcell2.Id : -1
                                };
                            }
                        }
                    }

                }
            }

            // -
            return View(new SessionJourVM
            {
                Niveaux = ls.Select(x => x.Niveau)?.ToList(),
                Jours = L.Select(x => x.Jour.ToLongDateString())?.ToList(),
                Cells = data
            });
        }
    }
}
