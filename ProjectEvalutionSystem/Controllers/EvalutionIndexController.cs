using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ProjectEvalutionSystem.Helper;
using ProjectEvalutionSystem.Models;
using ProjectEvalutionSystem.Models.EvalutionIndexDTOs;

namespace ProjectEvalutionSystem.Controllers
{
    public class EvalutionIndexController : Controller
    {
        // GET: Evalution
        public ActionResult Index()
        {
            if (Session["UserRole"] == null)
            {
                Session["ErrorException"] = "Please Login First";
                return RedirectToAction("Exception", "ErrorHandling");
            }
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAll()
        {
            try
            {
                using (ProjectEvalutionSystemEntities _context = new ProjectEvalutionSystemEntities())
                {
                    var result = await _context.EvalutionIndexes
                        .Include(x => x.Teacher)
                        .Include(x => x.Student)
                        .Include(x => x.Assignment)
                        .ToListAsync();
                    if (result.Any())
                    {
                        var ConvertedDTO = result.ConvertAll(EvalutionIndexDTO.EvalutionIndexConverter);
                        return Json(ConvertedDTO, JsonRequestBehavior.AllowGet);
                    }

                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<JsonResult> Get(int id)
        {
            try
            {
                using (ProjectEvalutionSystemEntities _context = new ProjectEvalutionSystemEntities())
                {
                    if (id > 0)
                    {
                        return Json(EvalutionIndexDTO.EvalutionIndexConverter(await _context.EvalutionIndexes.FirstOrDefaultAsync(x => x.ID == id)), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetDropdownDataStudent()
        {
            try
            {
                using (ProjectEvalutionSystemEntities _context = new ProjectEvalutionSystemEntities())
                {
                    var dataSet = await _context.Students.Select(x => new { x.ID, x.FullName }).ToListAsync();
                    return Json(dataSet, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetDropdownDataForStudent(int studentID)
        {
            try
            {
                using (ProjectEvalutionSystemEntities _context = new ProjectEvalutionSystemEntities())
                {
                    var _teachers = await _context.StudentTeachers.Include(x => x.Teacher)
                        .Where(x => x.Student.ID == studentID).Select(x => new { x.Teacher.ID, x.Teacher.FullName }).Distinct().ToListAsync();

                    var _assignments = await _context.Assignments.Where(x => x.StudentID == studentID).Select(x => new { x.ID, x.Name }).Distinct().ToListAsync();

                    var dataSet = new
                    {
                        teachers = _teachers,
                        assignments = _assignments
                    };

                    return Json(dataSet, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpGet]
        public async Task<JsonResult> StartEvalution(int studentid, int teacherid, int assignmentid)
        {
            try
            {
                using (ProjectEvalutionSystemEntities _context = new ProjectEvalutionSystemEntities())
                {
                    var Assignment = await _context.Assignments.Include(x => x.Student).Include(x => x.Teacher).Where(x => x.ID == assignmentid).FirstOrDefaultAsync();
                    if (Assignment != null)
                    {
                        //Checking Teacher
                        if (Assignment.TeacherID != teacherid)
                        {
                            return Json(new { status = false, data = "Teacher does not match" }, JsonRequestBehavior.AllowGet);
                        }
                        //Checking Student
                        if (Assignment.StudentID != studentid)
                        {
                            return Json(new { status = false, data = "Student does not match" }, JsonRequestBehavior.AllowGet);
                        }
                        DirectoryInfo dir = new DirectoryInfo(Path.Combine(Server.MapPath("~/App_Data/"), Assignment.Path));
                        if (dir.FullName != null)
                        {
                            SeliniumExecution.StartProcess(dir.FullName);
                        }
                        return Json(new { message = "ok" }, JsonRequestBehavior.AllowGet);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {

                throw;
            }
}
    }
}