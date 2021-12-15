using Newtonsoft.Json;
using ProjectEvalutionSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProjectEvalutionSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(GetAllAssignmentDetails());
        }

        [HttpGet]
        public async Task<JsonResult> GetStats()
        {
            try
            {
                using (ProjectEvalutionSystemEntities _context = new ProjectEvalutionSystemEntities())
                {
                    var _students = await _context.Students.CountAsync();
                    var _teachers = await _context.Teachers.CountAsync();
                    var _assignments = await _context.Assignments.CountAsync();
                    var dataSet = new
                    {
                        students = _students,
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

        public List<Assignment> GetAllAssignmentDetails()
        {
            try
            {
                using (ProjectEvalutionSystemEntities _context = new ProjectEvalutionSystemEntities())
                {
                    return (from us in _context.Assignments
                            //where us.CreationTimeStamp.Date == DateTime.Now.Date
                            select us).ToList();
                    //return _context.Assignments.Where(x => Convert.ToDateTime(x.CreationTimeStamp).Date == DateTime.Now.Date).ToList();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}