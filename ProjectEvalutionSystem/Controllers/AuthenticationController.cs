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
                using (ProjectEvalutionSystemEntities _context = new ProjectEvalutionSystemEntities())
                {
                    switch (input.whoLogin)
                    {
                        case UserRole.SuperAdmin:
                            var result = _context.Admins
                                .FirstOrDefault(x =>
                                    x.EmailAddress == input.EmailAddress && x.Password == input.Password);
                            if (result != null)
                            {
                                return Json(new { success = true, data = result }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { success = false}, JsonRequestBehavior.AllowGet);
                            }
                        case UserRole.Teacher:
                            var result1 = _context.Teachers
                                .FirstOrDefault(x =>
                                    x.EmailAddress == input.EmailAddress && x.Password == input.Password);
                            if (result1 != null)
                            {
                                return Json(new { success = true, data = result1 }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                            }
                        case UserRole.Student:
                            var result2 = _context.Students
                                .FirstOrDefault(x =>
                                    x.EmailAddress == input.EmailAddress && x.Password == input.Password);
                            if (result2 != null)
                            {
                                return Json(new { success = true, data = result2 }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                            }
                        default:
                            return Json(new
                            {
                                success = false}, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}