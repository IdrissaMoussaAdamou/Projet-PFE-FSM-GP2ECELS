using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Projet_PFE.Bll;
using Projet_PFE.Models;
namespace Projet_PFE.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        [Route("login")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult LogIn() => View(new LogInModel());

        [HttpPost]
        [Route("login")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogInModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var User = Bll_User.Authentificate(model.Email, model.Password);
            if (User == null)
            {
                ModelState.AddModelError("EMessage", "Les Données sont Invalides");
                return View(model);
            }

            var UserClaims = new List<Claim>(){
                new Claim(ClaimTypes.NameIdentifier, User.CIN),
                new Claim(ClaimTypes.Name, string.Concat(User.Nom, " ", User.Prenom)),
                new Claim(ClaimTypes.Email, User.Email)
            };

            // Assign role to user
            if (User.Profil == "Enseignant")
            {
                UserClaims.Add(new Claim("Enseignant", User.Profil));
            }


            var UserIdentity = new ClaimsIdentity(UserClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            var UserPrincipal = new ClaimsPrincipal(UserIdentity);

            await HttpContext.SignInAsync(UserPrincipal);

            if (User.Profil == "Enseignant")
            {
                return RedirectToAction("DetailsCharge", "ChargeEnseignement", new { Id = User.Id });
            }
            return RedirectToAction("Index", "Home");
        }

        [Route("logout")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(LogIn));
        }

        [HttpGet]
        [Route("register")]
        [Authorize]
        public IActionResult Register(string IdUser)
        {
            var Id = Convert.ToInt64(IdUser);
            if (Id > 0)
            {
                return PartialView("Register", Bll_User.SelectById(Id));
            }

            return PartialView("Register", new RegisterModel());
        }

        [HttpPost]
        [Route("register")]
        [Authorize]
        public IActionResult Register(RegisterModel User)
        {

            if (User.Id > 0)
            {
                Bll_User.Update(User);
                User = Bll_User.SelectById(User.Id);
            }
            else
            {
                Bll_User.Add(User);
                User = Bll_User.SelectTheLast();
            }

            User.Password = "";

            return Json(User);
        }

        [HttpGet]
        [Route("registerteacher")]
        [AllowAnonymous]
        public IActionResult RegisterTeacher() => View(new RegisterTeacherModel());

        [HttpPost]
        [Route("registerteacher")]
        //[ValidateAntiForgeryToken]
        [AllowAnonymous]
        public IActionResult RegisterTeacher(RegisterTeacherModel Teacher)
        {
            if (!ModelState.IsValid)
            {
                return View(Teacher);
            }
            Teacher.Email = Teacher.Email.Trim(' ');
            Teacher.CIN = Teacher.CIN.Trim(' ');

            var User = Bll_User.SelectByCIN(Teacher.CIN);
            if (User != null)
            {
                ModelState.AddModelError("EMessage", $"Le CIN {Teacher.CIN} appartient à un compte existant");
                return View(Teacher);
            }

            var AnneeUniv = Bll_AnneeUniversitaire.SelectTNArchivedAnneeUniv();
            var AnneeUnivE = Bll_AnneeUniversitaireEnseignant.SelectByAUCIN(AnneeUniv.Code, Teacher.CIN);

            User = new RegisterModel(AnneeUnivE, Teacher.Email, Teacher.Password);
            // Add teacher
            Bll_User.Add(User);

            return RedirectToAction("LogIn");
        }

        [HttpGet]
        [Route("updateaccount")]
        [Authorize]
        public IActionResult UpdateAccount()
        {
            var CIN = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return PartialView("Edit", Bll_User.SelectByCIN(CIN));
        }

        [HttpPost]
        [Route("updateaccount")]
        [Authorize]
        public IActionResult UpdateAccount(RegisterModel UserAccount)
        {
            Bll_User.Update(UserAccount);
            UserAccount = Bll_User.SelectById(UserAccount.Id);
            return Json(new
            {
                UserName = string.Concat(UserAccount.Nom, " ", UserAccount.Prenom),
                UserEmail = UserAccount.Email,
            });
        }

        [HttpPost]
        [Route("deleteaccount")]
        [Authorize]
        public JsonResult DeleteAccount(long Id)
        {
            Bll_User.Delete(Id);
            return Json(Id);
        }

        [HttpGet]
        [Route("changepassword")]
        [Authorize]
        public IActionResult ChangePassword(string Id)
        {
            var model = new ChangePassword();
            model.Iduser = Convert.ToInt64(Id);

            return PartialView(model);
        }

        [HttpPost]
        [Route("changepassword")]
        [Authorize]
        public JsonResult ChangePassword(ChangePassword model)
        {
            if (model.NewPassword == model.ConfirmNewPassword)
            {
                var UserAccount = new RegisterModel();
                if (model.Iduser > 0)
                {
                    UserAccount.Id = model.Iduser;
                }
                else
                {
                    UserAccount.Id = Bll_User.SelectByCIN(User.FindFirst(ClaimTypes.NameIdentifier)?.Value).Id;
                }

                UserAccount.Password = model.NewPassword;
                Bll_User.Update(UserAccount);

                return Json(true);
            }
            return Json(false);
        }

        [AcceptVerbs("Get", "POST")]
        [Route("verifyCIN")]
        [AllowAnonymous]
        public IActionResult VerifyCIN(string CIN)
        {
            CIN = CIN.Trim(' ');

            var AnneeUniv = Bll_AnneeUniversitaire.SelectTNArchivedAnneeUniv();
            if (AnneeUniv == null)
            {
                return Json($"Les données de la nouvelle année ne sont encore pas saisies");
            }
            else
            {
                var Enseignant = Bll_AnneeUniversitaireEnseignant.SelectByAUCIN(AnneeUniv.Code, CIN);
                if (Enseignant == null)
                {
                    return Json($"Le CIN {CIN} ne figure pas dans la liste des Enseignants de l'année en Cours");
                }

                return Json(true);
            }

        }

        [AcceptVerbs("Get", "POST")]
        [Route("verifycinunicity")]
        [Authorize]
        public IActionResult VerifyCINUnicity(string CIN, string OldCIN)
        {
            CIN = CIN.Trim(' ');
            if (OldCIN == CIN)
            {
                return Json(true);
            }

            var User = Bll_User.SelectByCIN(CIN);
            if (User != null)
            {
                return Json($"Le CIN {CIN} appartient à un compte existant.");
            }
            else
            {
                return Json(true);
            }

        }

        [AcceptVerbs("Get", "POST")]
        [Route("checkemailunicity")]
        [AllowAnonymous]
        public IActionResult CheckEmailUnicity(string Email, string OldEmail)
        {
            Email = Email.Trim(' ');
            if (OldEmail == Email)
            {
                return Json(true);
            }

            var User = Bll_User.SelectByEmail(Email);
            if (User != null)
            {
                return Json($" L'adresse E-mail {Email} appartient à un compte existant.");
            }
            else
            {
                return Json(true);
            }

        }


    }
}