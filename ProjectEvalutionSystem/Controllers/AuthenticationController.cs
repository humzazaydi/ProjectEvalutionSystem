using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
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
                            Session["CurrentLoginName"] = result.FullName;
                            Session["CurrentLoginEmail"] = result.EmailAddress;
                            Session["UserRole"] = (int)result.UserRole;
                            Session["CopyLeaksSession"] = LoginToCopyLeaks();
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
                            Session["CurrentLoginName"] = result1.FullName;
                            Session["CurrentLoginEmail"] = result1.EmailAddress;
                            Session["UserRole"] = (int)result1.UserRole;
                            Session["CopyLeaksSession"] = LoginToCopyLeaks();
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
                        var result2 = db.Students
                            .FirstOrDefault(x =>
                                x.EmailAddress == input.EmailAddress && x.Password == input.Password);
                        if (result2 != null)
                        {
                            Session["CurrentLoginName"] =  result2.FullName;
                            Session["CurrentLoginEmail"] = result2.EmailAddress;
                            Session["UserRole"] = (int)result2.UserRole;
                            Session["CopyLeaksSession"] = LoginToCopyLeaks();
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

        public async Task LoginToCopyLeaks()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    //CopyLeaks Authentication
                    var param = new
                    {
                        Email = "syedmohammadhumzazaydi@gmail.com",
                        Key = "f9b9e9fb-b2d4-4471-a04c-8ff8b63d4ce6"
                    };
                    var myContent = JsonConvert.SerializeObject(param);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = await client.PostAsync(AppConstants.GetCopyLeaksEndPoint() + "api/CopyleaksDemo/login", byteContent);

                    Session["CopyLeaksInfo"] = response.Content;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }

}