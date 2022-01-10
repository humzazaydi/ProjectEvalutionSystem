using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using ProjectEvalutionSystem.Models;
using ProjectEvalutionSystem.Models.AssignmentDTOs;

namespace ProjectEvalutionSystem.Controllers
{
    public class AssignController : Controller
    {
        private ProjectEvalutionSystemEntities db = new ProjectEvalutionSystemEntities();
        // GET: Assignments
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetAsync()
        {
            try
            {
                var assignments = await db.Assignments.Where(x => x.IsDeleted == false).ToListAsync();
                return Json(assignments, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(exception.InnerException, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("GetAsync/{assignmentId}")]
        public async Task<JsonResult> GetAsync(int assignmentId)
        {
            try
            {
                var assignments = await db.Assignments.Where(x => x.ID == assignmentId && x.IsDeleted == false).FirstOrDefaultAsync();
                return Json(assignments, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(exception.InnerException, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> PostAsync([Form] AssignmentsDTO input)
        {
            try
            {
                if (input.Files != null)
                {
                    //Get all files from Request object  
                    HttpPostedFileBase file = input.Files;
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    // Get the complete folder path and store the file inside it.  
                    fname = Path.Combine(Server.MapPath("~/App_Data/"), fname);
                    file.SaveAs(fname);
                    var assignmentTobeAdd = new Assignment
                    {
                        Name = input.Name,
                        Description = input.Description,
                        Path = file.FileName,
                        IsDeleted = false,
                        CreationTimeStamp = DateTime.Now
                    };

                    db.Assignments.Add(assignmentTobeAdd);
                    await db.SaveChangesAsync();

                    return Json(input.ID, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Please upload a document", JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception exception)
            {
                return Json(exception.InnerException, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPut]
        public async Task<JsonResult> PutAsync(AssignmentsDTO input)
        {
            try
            {
                var assignment = await db.Assignments.Where(x => x.ID == input.ID).FirstOrDefaultAsync();
                if (assignment != null)
                {
                    assignment.Name = input.Name;
                    assignment.Description = input.Description;

                    db.Entry(assignment).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    return Json(assignment.ID, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exception)
            {
                return Json(exception.InnerException, JsonRequestBehavior.AllowGet);
            }
        }
    }
}