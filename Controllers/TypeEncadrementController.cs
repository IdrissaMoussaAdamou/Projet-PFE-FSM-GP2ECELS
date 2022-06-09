using System;
using Microsoft.AspNetCore.Mvc;
using Projet_PFE.Bll;
using Projet_PFE.Models;

namespace Projet_PFE.Controllers
{
    public class TypeEncadrementController : Controller
    {
        [HttpGet]
        public IActionResult CreateEdit(string Id)
        {
            var IdTypeEncadrement = Convert.ToInt64(Id);
            var model = new TypeEncadrement();
            if (IdTypeEncadrement > 0)
                model = Bll_TypeEncadrement.SelectById(IdTypeEncadrement);

            return PartialView("Create", model);
        }

        [HttpPost]
        public JsonResult StoreUpdate(TypeEncadrement TyEncadrement)
        {
            TypeEncadrement model;
            if (TyEncadrement.Id > 0)
            {
                Bll_TypeEncadrement.Update(TyEncadrement);
                model = Bll_TypeEncadrement.SelectById(TyEncadrement.Id);
            }
            else
            {
                Bll_TypeEncadrement.Add(TyEncadrement);
                model = Bll_TypeEncadrement.SelectTheLast();
            }

            return Json(model);
        }
        public JsonResult Delete(long Id)
        {
            if (Bll_TypeEncadrement.IsForeignKeyInTable("ChargeEncadrement", Id))
            {
                return Json(new { Delete = false, Element = "une Charge d' Encadrement" });
            }

            Bll_TypeEncadrement.Delete(Id);
            return Json(new { Delete = true, Id = Id });
        }

        // Types Charges Divers
        [HttpGet]
        public IActionResult CreateEditTypeChargeDiverse(string Id)
        {
            var IdTyChrgDivers = Convert.ToInt64(Id);
            var model = new TypeChargeDiverse();
            if (IdTyChrgDivers > 0)
                model = Bll_TypeChargeDiverse.SelectById(IdTyChrgDivers);

            return PartialView("CreateTypeChargeDiverse", model);
        }

        [HttpPost]
        public JsonResult StoreUpdateTypeChargeDiverse(TypeChargeDiverse TyChrgDivers)
        {
            TypeChargeDiverse model;
            if (TyChrgDivers.Id > 0)
            {
                Bll_TypeChargeDiverse.Update(TyChrgDivers);
                model = TyChrgDivers;
            }
            else
            {
                Bll_TypeChargeDiverse.Add(TyChrgDivers);
                model = Bll_TypeChargeDiverse.SelectTheLast();
            }

            return Json(model);
        }
        public JsonResult DeleteTypeChargeDiverse(long Id)
        {

            if (Bll_TypeChargeDiverse.IsForeignKeyInTable("ChargeDiverse", Id))
            {
                return Json(new { Delete = false, Element = "une Charge Divers" });
            }
            Bll_TypeChargeDiverse.Delete(Id);
            return Json(new { Delete = true, Id = Id });
        }
    }
}
