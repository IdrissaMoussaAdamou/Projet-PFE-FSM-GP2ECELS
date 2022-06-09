using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projet_PFE.Bll;
using Projet_PFE.Models;

namespace Projet_PFE.Controllers
{
    public class BatimentController : Controller
    {
        [HttpGet]
        public IActionResult Index() => View(Bll_Batiment.SelectAll());

        public IActionResult CreateEditBatiment(string Code)
        {
            var model = new Batiment();
            if (!string.IsNullOrEmpty(Code))
                model = Bll_Batiment.SelectById(Code);
            return PartialView("CreateEdit", model);
        }

        [HttpPost]
        public IActionResult StoreUpdate(Batiment B)
        {
            object[] obj = new object[2];
            var model = new Batiment();

            obj[0] = Bll_Batiment.Add(B);
            //Bll_Salle.Add(S);
            //model = Bll_Salle.SelectById(S.Code);
            obj[1] = Bll_Batiment.SelectById(B.Code);

            return Json(obj);
        }


        [HttpGet]
        public JsonResult Delete(string Code)
        {

            var Batiment = Bll_Batiment.SelectById(Code);

            if (Bll_Batiment.IsForeignKeyInTable("Salle", Code))
            {
                return Json(new { Delete = false, Element = "une Salle" });
            }

            if (Batiment == null)
            {
                return Json(new { Delete = false, Code = Code });
            }
            Bll_Batiment.Delete(Code);
            return Json(new { Delete = true, Code = Code });
        }

        public IActionResult Show(string Code) => PartialView("Show", Bll_Batiment.SelectById(Code));

    }
}
