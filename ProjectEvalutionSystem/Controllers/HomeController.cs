using Newtonsoft.Json;
using ProjectEvalutionSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ProjectEvalutionSystem.Models.Auth;
using Assignment = ProjectEvalutionSystem.Models.Assignment;

namespace ProjectEvalutionSystem.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            if (Session["UserRole"] == null)
            {
                Session["ErrorException"] = "Please Login First";
                return RedirectToAction("Exception", "ErrorHandling");
            }
            return View(await GetAllAssignmentDetails());
        }

        [HttpGet]
        public async Task<JsonResult> GetStats()
        {
            try
            {
                using (PESCF _context = new PESCF())
                {
                    int _students = 0, _teachers = 0, _assignments = 0, _evalutionIndexes = 0, _courses = 0;
                    var sessionID = (int)Session["CurrentLoginId"];
                    switch ((UserRole)Session["UserRole"])
                    {
                        case UserRole.Teacher:
                            _students = await _context.Students.Where(x=> x.TeacherID == sessionID).CountAsync();
                            _assignments = await _context.Assignments
                                .Include(x => x.Cours)
                                .Where(x=> x.Cours.TeacherID == sessionID)
                                .CountAsync();
                            _evalutionIndexes = await _context.EvalutionIndexes
                                .Include(a => a.Assignment)
                                .Include(x => x.Assignment.Cours)
                                .Where(x => x.Assignment.Cours.TeacherID == sessionID).CountAsync();
                            _courses = await _context.Courses.Where(x=> x.TeacherID == sessionID).CountAsync();
                            break;

                        case UserRole.SuperAdmin:
                            _students = await _context.Students.CountAsync();
                            _teachers = await _context.Teachers.CountAsync();
                            _assignments = await _context.Assignments.CountAsync();
                            _evalutionIndexes = await _context.EvalutionIndexes.CountAsync();
                            _courses = await _context.Courses.CountAsync();
                            break;
                    }
                    
                    var dataSet = new
                    {
                        students = _students,
                        teachers = _teachers,
                        assignments = _assignments,
                        evalutionIndexes = _evalutionIndexes,
                        courses = _courses
                    };
                    return Json(dataSet, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<Assignment>> GetAllAssignmentDetails()
        {
            try
            {
                using (PESCF _context = new PESCF())
                {
                    IQueryable<Assignment> assignments = null;
                    var sessionID = (int)Session["CurrentLoginId"];
                    switch ((UserRole)Session["UserRole"])
                    {
                        case UserRole.Teacher:
                            assignments = _context.Assignments.Include(a => a.Cours).Where(x => x.Cours.TeacherID == sessionID);
                            break;

                        case UserRole.SuperAdmin:
                            assignments = _context.Assignments.Include(a => a.Cours);
                            break;
                        case UserRole.Student:
                            assignments = _context.Assignments.Include(a => a.Cours).Where(x=> x.StudentID == sessionID);
                            break;
                    }

                    return await assignments.ToListAsync();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> ChangeSettings(ChangeSettingsDTO input)
        {
            try
            {
                using (PESCF _context = new PESCF())
                {
                    switch ((UserRole)input.user_role)
                    {
                        case UserRole.Student:
                            var userDetails0 = await _context.Students.Where(x => x.ID == input.id)
                                .FirstOrDefaultAsync();
                            if (userDetails0 != null)
                            {
                                userDetails0.Password = input.new_password;
                                userDetails0.FullName = input.fullname;
                                userDetails0.EmailAddress = input.email_address;
                                _context.Entry(userDetails0).State = EntityState.Modified;
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                return Json(new
                                {
                                    message = "Student not found",
                                    code = 404,
                                    success = false,
                                }, JsonRequestBehavior.AllowGet);
                            }
                            break;
                        case UserRole.Teacher:
                            var userDetails1 = await _context.Teachers.Where(x => x.ID == input.id)
                                .FirstOrDefaultAsync();
                            if (userDetails1 != null)
                            {
                                userDetails1.Password = input.new_password;
                                userDetails1.FullName = input.fullname;
                                userDetails1.EmailAddress = input.email_address;
                                _context.Entry(userDetails1).State = EntityState.Modified;
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                return Json(new
                                {
                                    message = "Teacher not found",
                                    code = 404,
                                    success = false,
                                }, JsonRequestBehavior.AllowGet);
                            }
                            break;
                        case UserRole.SuperAdmin:
                            var userDetails2 = await _context.Admins.Where(x => x.ID == input.id)
                                .FirstOrDefaultAsync();
                            if (userDetails2 != null)
                            {
                                userDetails2.FullName = input.fullname;
                                userDetails2.EmailAddress = input.email_address;
                                userDetails2.Password = input.new_password;
                                _context.Entry(userDetails2).State = EntityState.Modified;
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                return Json(new
                                {
                                    message = "Admin not found",
                                    code = 404,
                                    success = false,
                                }, JsonRequestBehavior.AllowGet);
                            }
                            break;
                    }

                    return Json(new
                    {
                        message = "Password has been changed!",
                        code = 200,
                        success = true,
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetUserSettings()
        {
            try
            {
                using (PESCF _context = new PESCF())
                {
                    var currentLoginId = (int) Session["CurrentLoginId"];
                    switch ((UserRole)Session["UserRole"])
                    {
                        case UserRole.Student:
                            var userDetails0 = await _context.Students.Where(x => x.ID == currentLoginId)
                                .FirstOrDefaultAsync();
                            Session["CurrentLoginName"] = userDetails0.FullName;
                            Session["CurrentLoginEmail"] = userDetails0.EmailAddress;
                            return Json(new
                            {
                                emailaddress = userDetails0.EmailAddress,
                                fullname = userDetails0.FullName
                            }, JsonRequestBehavior.AllowGet);
                        case UserRole.Teacher:
                            var userDetails1 = await _context.Teachers.Where(x => x.ID == currentLoginId)
                                .FirstOrDefaultAsync();
                            Session["CurrentLoginName"] = userDetails1.FullName;
                            Session["CurrentLoginEmail"] = userDetails1.EmailAddress;
                            return Json(new
                            {
                                emailaddress = userDetails1.EmailAddress,
                                fullname = userDetails1.FullName
                            }, JsonRequestBehavior.AllowGet);
                        case UserRole.SuperAdmin:
                            var userDetails2 = await _context.Admins.Where(x => x.ID == currentLoginId)
                                .FirstOrDefaultAsync();
                            Session["CurrentLoginName"] = userDetails2.FullName;
                            Session["CurrentLoginEmail"] = userDetails2.EmailAddress;
                            return Json(new
                            {
                                emailaddress = userDetails2.EmailAddress,
                                fullname = userDetails2.FullName
                            }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new
                    {
                        emailaddress = "",
                        fullname = ""
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}