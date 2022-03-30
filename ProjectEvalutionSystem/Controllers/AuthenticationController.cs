using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ProjectEvalutionSystem.Models;
using ProjectEvalutionSystem.Models.Auth;
using RestSharp;
using HttpCookie = System.Web.HttpCookie;

namespace ProjectEvalutionSystem.Controllers
{
    public class AuthenticationController : Controller
    {
        private ProjectEvalutionSystemEntities db = new ProjectEvalutionSystemEntities();
        // GET: Authentication
        public ActionResult GetIn()
        {
            if (Session["CurrentLoginName"] == null)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
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
                            Session["CurrentLoginId"] = result.ID;
                            Session["CurrentLoginName"] = result.FullName;
                            Session["CurrentLoginEmail"] = result.EmailAddress;
                            Session["UserRole"] = (int)result.UserRole;

                            return Json(new { success = true, data = new LoginSessionDTO
                            {
                                emailaddress = result.EmailAddress,
                                fullname = result.FullName,
                                user_role = (UserRole)result.UserRole
                            }
                            }, JsonRequestBehavior.AllowGet);
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
                            Session["CurrentLoginId"] = result1.ID;
                            Session["CurrentLoginName"] = result1.FullName;
                            Session["CurrentLoginEmail"] = result1.EmailAddress;
                            Session["UserRole"] = (UserRole)result1.UserRole;
                            return Json(new { success = true, data = new LoginSessionDTO
                            {
                                emailaddress = result1.EmailAddress,
                                fullname = result1.FullName,
                                user_role = (UserRole)result1.UserRole
                            }
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                        }
                    case UserRole.Student:
                        var result2 = db.Students.AsNoTracking()
                            .FirstOrDefault(x =>
                                x.EmailAddress == input.EmailAddress && x.Password == input.Password);
                        if (result2 != null)
                        {
                            Session["CurrentLoginId"] = result2.ID;
                            Session["CurrentLoginName"] =  result2.FullName;
                            Session["CurrentLoginEmail"] = result2.EmailAddress;
                            Session["UserRole"] = (UserRole)result2.UserRole;
                            return Json(new { success = true, data = new LoginSessionDTO
                            {
                                emailaddress = result2.EmailAddress,
                                fullname = result2.FullName,
                                user_role = (UserRole)result2.UserRole
                            }
                            }, JsonRequestBehavior.AllowGet);
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
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public ActionResult Logout()
        {
            Session.RemoveAll();
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("GetIn");
        }
    }

}