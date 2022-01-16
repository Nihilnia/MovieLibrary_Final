using Overture.Areas.Database;
using Overture.Areas.User.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Overture.Areas.User.Controllers
{
    public class UserJQGridController : Controller
    {
        // GET: User/UserJQGrid
        // GET: JQGrid
        public JsonResult GetDashboard()
        {
            var userName = Request.Cookies["fukinUser"].Value;
            var model = GridProcesses.GETMFGET(userName);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public string AddMovie(string MovieName, string CategoryName)
        {
            var userName = Request.Cookies["fukinUser"].Value;
            GridProcesses.AddMovie(userName, MovieName, CategoryName);
            return $"Added {MovieName} to your movie/s.";
        }

        public string DeleteMovie(Movie IncominMovie)
        {
            var userName = Request.Cookies["fukinUser"].Value;
            GridProcesses.DeleteMovie(IncominMovie);
            return $"Deleted from your movie/s.";
        }

        public string EditMovie(Movie IncominMovie, GridModel IncominModel)
        {
            var userName = Request.Cookies["fukinUser"].Value;
            var temp = IncominMovie.Name;
            GridProcesses.UpdateMovie(IncominMovie, IncominModel); //LMAO
            return $"{temp} edited.";
        }
    }
}