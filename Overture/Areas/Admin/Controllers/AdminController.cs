using Overture.Areas.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Overture.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin/Admin
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginControl(string userName, string passWord)
        {

            var result = AdminDatabaseProcesses.LoginControl(userName, passWord);

            if (result)
            {
                HttpCookie mfCookie = new HttpCookie("fukinAdmin", userName);
                Response.Cookies.Add(mfCookie);

                var model = AdminDatabaseProcesses.GetAdmin(userName);
                return View("~/Areas/Admin/Views/Admin/Index.cshtml", model);
            }
            else
            {
                ViewBag.ErrorMessage = "Check your login info mf.";
                return View("~/Areas/Admin/Views/Admin/Failed.cshtml");
            }
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Profile()
        {
            var model = AdminDatabaseProcesses.GetAdmin(Request.Cookies["fukinAdmin"].Value);
            return View(model);
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
                    ViewBag.ErrorMessage = "The selected picture is not png/jpg/jpeg";
                    return View("~/Areas/Admin/Views/Admin/Failed.cshtml");
                }

            }
            //else
            //{
            //    var currentSettings = DatabaseProcesses.GetUser(Username);
            //    DatabaseProcesses.UpdateUser(Username, passWord, firstName, lastName, EMail, currentSettings.ProfilePic);
            //}

            var model = DatabaseProcesses.GetUserIntel(Username);


            return View("~/Areas/Admin/Views/Admin/Profile.cshtml", model);

        }



        public ActionResult Register()
        {
            return View();
        }

        public ActionResult RegisterControl(string userName, string passWord)
        {

            var result = AdminDatabaseProcesses.Register(userName, passWord, (int)Roles.RoleTypez.Admin);

            if (result)
            {
                var model = AdminDatabaseProcesses.GetAdmin(userName);
                return View("~/Areas/Admin/Views/Admin/Login.cshtml", model);
            }
            else
            {
                ViewBag.ErrorMessage = "Somethings went wrong.";
                return View("~/Areas/Admin/Views/Admin/Failed.cshtml");
            }
        }


        public ActionResult Dashboard()
        {
            var adminName = Request.Cookies["fukinAdmin"].Value;
            var model = AdminDatabaseProcesses.GetEverything(adminName);
            return View(model);
        }


        public ActionResult AddMovie()
        {
            var model = AdminDatabaseProcesses.GetAdmin(Request.Cookies["fukinAdmin"].Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult AddMovie(string movieName, string category)
        {
            var adminName = Request.Cookies["fukinAdmin"].Value;
            var result = AdminDatabaseProcesses.AddMovie(adminName, movieName, category);

            if (result != null)
            {
                var model = AdminDatabaseProcesses.GetEverything(adminName);
                return View("~/Areas/Admin/Views/Admin/Dashboard.cshtml", model);
            }
            else
            {
                ViewBag.ErrorMessage = "Somethings went wrong mf.";
                return View("~/Areas/Admin/Views/Admin/Failed.cshtml");
            }
        }


        public ActionResult DeleteProfile()
        {
            return View();
        }
    }
}