using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ProjectEvalutionSystem.Models;
using ProjectEvalutionSystem.Models.EvalutionIndexDTOs;

namespace ProjectEvalutionSystem.Controllers
{
    public class EvalutionIndexController : Controller
    {
        // GET: Evalution
        public ActionResult Index()
        {
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
                        .Include(x=> x.Student)
                        .Include(x => x.Teacher)
                        .Include(x => x.Assignment)
                        .ToListAsync();
                    if (result.Any())
                    {
                        var ConvertedDTO = result.ConvertAll(EvalutionIndexDTO.EvalutionIndexConverter);
                        return Json(ConvertedDTO,JsonRequestBehavior.AllowGet);
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
                        return Json(EvalutionIndexDTO.EvalutionIndexConverter(await _context.EvalutionIndexes.FirstOrDefaultAsync(x => x.ID == id)),JsonRequestBehavior.AllowGet);
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
                    var dataSet = await _context.StudentTeachers.Include(x => x.Teacher)
                        .Where(x => x.ID == studentID).Select(x=> new {x.Teacher.ID, x.Teacher.FullName }).ToListAsync();


                    return Json(dataSet, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}