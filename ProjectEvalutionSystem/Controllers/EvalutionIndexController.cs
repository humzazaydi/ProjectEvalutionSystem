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
    }
}