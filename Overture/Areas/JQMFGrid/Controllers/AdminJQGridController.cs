using Overture.Areas.Admin.Data;
using Overture.Areas.Database;
using Overture.Areas.User.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Overture.Areas.Admin.Controllers
{
    public class AdminJQGridController : Controller
    {
        // GET: User/UserJQGrid
        // GET: JQGrid
        public JsonResult AdminGetDashboard()
        {
            var adminName = Request.Cookies["fukinAdmin"].Value;
            var model = AdminDatabaseProcesses.GetDatabase(adminName);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public string AdminAddMovie(string UserName, string MovieName, string CategoryName)
        {
            var userName = Request.Cookies["fukinAdmin"].Value;
            var result = AdminDatabaseProcesses.AddMovie(UserName, MovieName, CategoryName);

            if (result)
            {
                return $"Movie: {MovieName} added to your movie/s.";
            }
            else {
                return $"Movie: {MovieName} Culdn't added because there is no user such as {userName}.";
            }

            
        }

        public string AdminEditMovie(Movie oldMovInfos, AdminManageModel adminMM)
        {
            var userName = Request.Cookies["fukinAdmin"].Value;
            var result = AdminDatabaseProcesses.Update(oldMovInfos, adminMM); //LMAO

            if (result)
            {
                return $"{oldMovInfos.Name} edited as {adminMM.MovieName}.";
            }
            else{
                return $"{oldMovInfos.Name} Couldn' t edited.";
            }

        }

        public string AdminDeleteMovie(string ID)
        {
            var userName = Request.Cookies["fukinAdmin"].Value;
            var result = AdminDatabaseProcesses.Delete(Convert.ToInt32(ID));
            return $"Movie Deleted from your movie/s.";
        }
        
        public ActionResult AdminDeleteMovie2(string ID)
        {
            var userName = Request.Cookies["fukinAdmin"].Value;
            var result = AdminDatabaseProcesses.Delete(Convert.ToInt32(ID));
            var model = AdminDatabaseProcesses.GetEverything(userName);
            return View("~/Areas/Admin/Views/Admin/Dashboard.cshtml", model);
        }
    }
}