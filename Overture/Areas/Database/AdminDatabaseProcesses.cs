using Microsoft.EntityFrameworkCore;
using Overture.Areas.Admin.Data;
using Overture.Areas.User.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Overture.Areas.Database
{
    public static class AdminDatabaseProcesses
    {
        public static User GetAdmin(string daMFAdmin)
        {
            var aooof = new User();

            using (var DB = new OvertureContext())
            {
                var findAdmin = DB.Users.Where(f => f.UserName == daMFAdmin).FirstOrDefault();

                if (findAdmin != null)
                {
                    aooof = findAdmin;
                }
            }

            return aooof;
        }

        public static bool Register(string daAdminName, string daAdminPass, int daRole)
        {
            bool result = false;

            using (var DB = new OvertureContext())
            {
                var cryptedPass = Crypt.Gloria(daAdminPass);

                var newAdmin = new User()
                {
                    UserName = daAdminName,
                    Password = cryptedPass,
                    Role = daRole
                };

                DB.Users.Add(newAdmin);
                DB.SaveChanges();

                result = true;
            }

            return result;
        }


        public static bool LoginControl(string daAdminName, string daPassWord)
        {
            var result = false;

            using (var DB = new OvertureContext())
            {

                var findAdmin = DB.Users.Where(f => f.UserName == daAdminName).FirstOrDefault();

                if (findAdmin != null)
                {
                    var cryptedPass = Crypt.Gloria(daPassWord);
                    //var crypt_2 = Crypt.Gloria("666");

                    if (findAdmin.Password == cryptedPass)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        public static bool AddMovie(string userName, string movieName, string category)
        {
            var result = false;

            using (var DB = new OvertureContext())
            {
                var findUser = DB.Users.Where(f => f.UserName == userName).FirstOrDefault();

                if (findUser != null)
                {
                    var newMovie = new Movie() { Name = movieName };
                    var newCategory = new Category() { Name = category };

                    newMovie.Categories = new List<Category> { newCategory };
                    newMovie.UserID = findUser.ID;


                    DB.Movies.Add(newMovie);                    
                    DB.SaveChanges();

                    result = true;
                }
            }

            return result;
        }

        public static bool Update(Movie oldMovInfos, AdminManageModel adminMM)
        {
            bool result = false;

            using (var DB = new OvertureContext())
            {
                var findMovie = DB.Movies.Find(oldMovInfos.ID);

                if (findMovie != null)
                {
                    findMovie.Name = adminMM.MovieName;

                    var getCategory = DB.Categories.Where(c => c.MovieID == findMovie.ID).FirstOrDefault();

                    getCategory.Name = adminMM.CategoryName;                    

                    DB.SaveChanges();

                    result = true;
                }
            }

            return result;
        }

        public static string Delete(int movieID)
        {
            string result = null;

            using (var DB = new OvertureContext())
            {
                var findMovie = DB.Movies.Find(movieID);

                if (findMovie != null)
                {
                    DB.Movies.Remove(findMovie);

                    var getCats = DB.Categories.Where(f => f.MovieID == findMovie.ID).ToList();
                    DB.Categories.RemoveRange(getCats);

                    DB.SaveChanges();

                    result = findMovie.Name;
                }
            }

            return result;
        }


        //FOR ADMIN LAYOUT
        public static AdminModel GetEverything(string adminName)
        {
            var mfEverything = new AdminModel();

            using (var DB = new OvertureContext())
            {
                var findAdmin = DB.Users.Where(f => f.UserName == adminName).FirstOrDefault();

                if (findAdmin != null)
                {
                    mfEverything.ID = findAdmin.ID;
                    mfEverything.UserName = findAdmin.UserName;
                    mfEverything.ProfilePic = findAdmin.ProfilePic;

                    var allMovies = DB.Movies.Include(c => c.Categories).ToList();

                    foreach (var movies in allMovies)
                    {
                        var movieID = movies.ID;
                        var movieName = movies.Name;

                        foreach (var cat in movies.Categories)
                        {
                            var categoryID = cat.ID;
                            var categoryName = cat.Name;

                            var daGridModel = new GridModel()
                            {
                                MovieID = movieID,
                                MovieName = movieName,
                                CategoryID = categoryID,
                                CategoryName = categoryName
                            };

                            mfEverything.GridModel.Add(daGridModel);
                        }
                    }
                }
            }

            return mfEverything;
        }

        public static List<AdminGridModel> GetDatabase(string adminName)
        {
            var resultModel = new List<AdminGridModel>();

            using (var DB = new OvertureContext())
            {
                var findAdmin = DB.Users.Where(f => f.UserName == adminName).FirstOrDefault();

                if (findAdmin != null)
                {
                    var allUsers = DB.Users.Include(f => f.Movies).ToList();

                    foreach (var user in allUsers)
                    {
                        var tempUser = user;
                        var moviesOfUser = DB.Movies.Where(f => f.UserID == tempUser.ID).Include(c => c.Categories).ToList();
                        foreach (var userMovies in tempUser.Movies)
                        {
                            var tempMovie = userMovies;

                            //here

                            foreach (var categories in tempMovie.Categories)
                            {
                                var tempCats = new List<Category>() {
                                    new Category() { MovieID = tempMovie.ID, Name = categories.Name }
                                };

                                resultModel.Add(new AdminGridModel()
                                {
                                    UserID = tempUser.ID,
                                    UserName = tempUser.UserName,
                                    MovieID = tempMovie.ID,
                                    MovieName = tempMovie.Name,
                                    CategoryID = categories.ID,
                                    CategoryName = categories.Name,
                                });
                            }
                        }
                    }
                }
            }

            return resultModel;
        }


        //FOR JQGRID
        public static List<GridModel> GetAdminIntel()
        {
            var daGridModel = new List<GridModel>();

            using (var DB = new OvertureContext())
            {
                var allMovies = DB.Movies.Where(f => f.ID > 0).Include(c => c.Categories).ToList();

                foreach (var movie in allMovies)
                {
                    var daMovieID = movie.ID;
                    var daMovieName = movie.Name;

                    foreach (var cat in movie.Categories)
                    {
                        var daCategoryID = cat.ID;
                        var daCategoryName = cat.Name;

                        daGridModel.Add(new GridModel() { MovieID = daMovieID, MovieName = daMovieName, CategoryID = daCategoryID, CategoryName = daCategoryName});
                    }
                }
                
            }

            return daGridModel;
        }
    }
}