using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectEvalutionSystem.Models;
using ProjectEvalutionSystem.Models.Auth;

namespace ProjectEvalutionSystem.Controllers
{
    public class AuthenticationController : Controller
    {
        private ProjectEvalutionSystemEntities db = new ProjectEvalutionSystemEntities();
        // GET: Authentication
        public ActionResult GetIn()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(LoginDTO input)
        {
            try
            {
                switch (input.whoLogin)
                {
                    case UserRole.SuperAdmin:
                        var result = db.Admins
                            .FirstOrDefault(x =>
                                x.EmailAddress == input.EmailAddress && x.Password == input.Password);
                        if (result != null)
                        {
                            ViewData["CurrentLoginInfo"] = result;
                            return Json(new { success = true, data = result }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                        }
                    case UserRole.Teacher:
                        var result1 = db.Teachers
                            .FirstOrDefault(x =>
                                x.EmailAddress == input.EmailAddress && x.Password == input.Password);
                        if (result1 != null)
                        {
                            ViewData["CurrentLoginInfo"] = result1;
                            return Json(new { success = true, data = result1 }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                        }
                    case UserRole.Student:
                        var result2 = db.Students
                            .FirstOrDefault(x =>
                                x.EmailAddress == input.EmailAddress && x.Password == input.Password);
                        if (result2 != null)
                        {
                            ViewData["CurrentLoginInfo"] = result2;
                            return Json(new { success = true, data = result2 }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                        }
                    default:
                        return Json(new
                        {
                            success = false
                        }, JsonRequestBehavior.AllowGet);
                }
                db.Dispose();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }

}