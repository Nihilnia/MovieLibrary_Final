using Overture.Areas.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Overture.Areas.User.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult RegisterControl(string userName, string passWord)
        {
            var registerResult = DatabaseProcesses.RegisterControl(userName, passWord, (int)Roles.RoleTypez.User);
            var model = DatabaseProcesses.GetUserIntel(userName);

            if (registerResult)
            {
                return View("~/Areas/User/Views/User/Login.cshtml", model);
            }
            else
            {
                ViewBag.ErrorMessage = "This username already taken.";
                return View("~/Areas/User/Views/User/Failed.cshtml");
            }
            
        }

        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult LoginControl(string userName, string passWord)
        {
            var model = DatabaseProcesses.LoginControl(userName, passWord);

            if (model != null)
            {
                HttpCookie cookie = new HttpCookie("fukinUser", userName);
                Response.Cookies.Add(cookie);
                return View("~/Areas/User/Views/User/Index.cshtml", model);
            }
            else
            {
                ViewBag.ErrorMessage = "Somethings went wrong.";
                return View("~/Areas/User/Views/User/Failed.cshtml");
            }
        }


        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Dashboard()
        {
            var model = DatabaseProcesses.GetUserMovsAndCats(Request.Cookies["fukinUser"].Value);
            return View(model);
        }


        public ActionResult Profile()
        {
            var model = DatabaseProcesses.GetUserIntel(Request.Cookies["fukinUser"].Value);
            return View("~/Areas/User/Views/User/Profile.cshtml", model);
        }

        [HttpPost]
        public ActionResult ProfileControl(string Username, string firstName, string lastName, string EMail, string passWord, HttpPostedFileBase file)
        {
            var pFileName = "";

            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);

                if (extension == ".jpg" || extension == ".png" || extension == ".jpeg")
                {

                    //var aaa = "abc";
                    //var res = aaa.GetHashCode();

                    var fileName = Path.GetFileName(file.FileName);

                    var userIntels = DatabaseProcesses.GetUserIntel(Username);

                    var randNumber = DatabaseProcesses.RandomText();


                    pFileName = randNumber + userIntels.UserName.GetHashCode().ToString() + fileName;

                    if (pFileName == userIntels.ProfilePic)
                    {
                        randNumber = DatabaseProcesses.RandomText();
                        pFileName = randNumber + userIntels.UserName.GetHashCode().ToString() + fileName;
                    }
                    
                    var path = Path.Combine(Server.MapPath("~/Content/UserPictures"), pFileName);

                    file.SaveAs(path);

                    DatabaseProcesses.UpdateUser(Username, passWord, firstName, lastName, EMail, pFileName);
                }
                else
                {
                    ViewBag.ErrorMessage = "The selected picture is not png/jpg";
                    return View("~/Areas/User/Views/User/Failed.cshtml");
                }

            }
            //else
            //{
            //    var currentSettings = DatabaseProcesses.GetUser(Username);
            //    DatabaseProcesses.UpdateUser(Username, passWord, firstName, lastName, EMail, currentSettings.ProfilePic);
            //}

            var model = DatabaseProcesses.GetUserIntel(Username);


            return View("~/Areas/User/Views/User/Profile.cshtml", model);

        }


        [HttpGet]
        public ActionResult DeleteUser(string userName)
        {
            var result = DatabaseProcesses.DeleteUser(userName);

            if (result)
            {
                return View("~/Areas/User/Views/User/Login.cshtml");
            }
            else
            {
                ViewBag.ErrorMessage = "Somethings went wrong.";
                return View("~/Areas/User/Views/User/Failed.cshtml");
            }
        }


        public ActionResult AddMovie()
        {
            var model = DatabaseProcesses.GetUserIntel(Request.Cookies["fukinUser"].Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult AddMovie(string movieName, string category, string userName)
        {
            var result = DatabaseProcesses.AddMovie(movieName, category, userName);

            if (result)
            {
                var model = DatabaseProcesses.GetUserIntel(userName);
                return View("~/Areas/User/Views/User/Dashboard.cshtml", model);
            }
            else
            {
                return View("~/Areas/User/Views/User/Failed.cshtml");
            }
        }
    }
}