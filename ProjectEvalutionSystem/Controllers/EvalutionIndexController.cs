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
    }
}