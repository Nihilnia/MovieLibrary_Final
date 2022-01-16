using Microsoft.EntityFrameworkCore;
using Overture.Areas.User.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Overture.Areas.Database
{
    public static class DatabaseProcesses
    {
        //Basic Info's of F User
        public static User GetUserIntel(string daUserName)
        {
            var theUser = new User();

            using (var DB = new OvertureContext())
            {
                var findUser = DB.Users.Where(f => f.UserName == daUserName).FirstOrDefault();

                if (findUser != null)
                {
                    theUser = findUser;
                }
            }

            return theUser;
        }


        //Register Control
        public static bool RegisterControl(string daUserName, string daPassword, int daRoleType)
        {
            var result = false;

            using (var DB = new OvertureContext())
            {
                var isUnique = DB.Users.Where(f => f.UserName == daUserName).FirstOrDefault();

                if (isUnique == null)
                {
                    var cryptedPassword = Crypt.Gloria(daPassword);

                    var newUser = new User() { UserName = daUserName, Password = cryptedPassword, Role = daRoleType };
                    DB.Users.Add(newUser);
                    DB.SaveChanges();

                    result = true;
                }                
            }

            return result;
        }


        //Login Control
        public static User LoginControl(string daUserName, string daPassword)
        {
            var theUser = new User();

            using (var DB = new OvertureContext())
            {
                var findUser = DB.Users.Where(f => f.UserName == daUserName).FirstOrDefault();

                if (findUser != null)
                {
                    var cryptedPassword = Crypt.Gloria(daPassword);

                    if (findUser.Password == cryptedPassword)
                    {
                        theUser = findUser;
                    }
                }
            }

            return theUser;
        }


        //UPDATE
        public static bool UpdateUser(string daUserName, string passWord, string firstName, string lastName, string eMail, string pFileName)
        {
            var result = false;

            using (var DB = new OvertureContext())
            {
                var findUser = DB.Users.Where(f => f.UserName == daUserName).FirstOrDefault();

                if (findUser != null)
                {
                    findUser.FirstName = firstName;
                    findUser.LastName = lastName;
                    findUser.EMail = eMail;
                    findUser.ProfilePic = pFileName;

                    DB.SaveChanges();

                    result = true;
                }
            }

            return result;
        }


        //DELETE
        public static bool DeleteUser(string daUserName)
        {
            bool result = false;

            using (var DB = new OvertureContext())
            {
                var findUser = DB.Users.Where(f => f.UserName == daUserName).FirstOrDefault();

                if (findUser != null)
                {
                    DB.Users.Remove(findUser);
                    DB.SaveChanges();

                    result = true;
                }
            }

            return result;
        }




        //RANDOM PROFILE PIC NAME
        public static string RandomText()
        {
            var Gloria = 0;

            using (var DB = new OvertureContext())
            {
                var getProfilePicNames = DB.Users.Where(f => f.ID > 0).Select(f => f.ProfilePic).ToList();

                Random random = new Random();
                ;
                for (int f = 0; f < 12; f++)
                {
                    int randomNumb = random.Next(0, 999);
                    Gloria += randomNumb;
                }

                foreach (var f in getProfilePicNames)
                {
                    if (Gloria.ToString() != f)
                    {
                        continue;
                    }
                    else
                    {
                        Gloria++;
                    }
                }

            }

            return Gloria.ToString();
            
        }



        //ADDING NEW MOVIE
        public static bool AddMovie(string newMovName, string newCategory, string daUserName)
        {
            bool result = false;

            using (var DB = new OvertureContext())
            {
                var theUser = DatabaseProcesses.GetUserIntel(daUserName);
                var newCat = new List<Category>()
                {
                    new Category(){ Name = newCategory }
                };
                DB.Movies.Add(new Movie() { Name = newMovName, UserID = theUser.ID, Categories = newCat });
                

                DB.SaveChanges();

                result = true;
            }

            return result;
        }

        public static User GetUserMovsAndCats(string daUserName)
        {
            var theUser = new User();

            using (var DB = new OvertureContext())
            {
                var findUser = DB.Users.Where(f => f.UserName == daUserName)
                    .FirstOrDefault();

                var userMovies = DB.Movies.Where(f => f.UserID == findUser.ID)
                    .Include(c => c.Categories)
                    .ToList();

                findUser.Movies = userMovies;

                

                if (findUser != null)
                {
                    theUser = findUser;
                }
            }

            return theUser;
        }



        //MOVIE SIDE

        //USER MOVIES
        public static List<Movie> GetUserMovies(string daUserName)
        {
            var moviez = new List<Movie>();

            using (var DB = new OvertureContext())
            {
                var findUser = DB.Users.Where(f => f.UserName == daUserName).FirstOrDefault();

                if (findUser != null)
                {
                    moviez = DB.Movies.Where(f => f.UserID == findUser.ID).ToList();
                }
            }

            return moviez;
        }


        //TRY
        //public static List<GridModel> GETMFGET(string daUserName)
        //{
        //    var result = new List<GridModel>();

        //    using (var DB = new OvertureContext())
        //    {
        //        var findUser = DB.Users.Where(f => f.UserName == daUserName).FirstOrDefault();

        //        var userMovies = DB.Movies.Where(f => f.UserID == findUser.ID).Include(c => c.Categories).ToList();

        //        findUser.Movies = userMovies;

        //        foreach (var f in findUser.Movies)
        //        {
        //            var movID = f.ID;
        //            var movName = f.Name;
        //            foreach (var c in f.Categories)
        //            {
        //                var catID = c.ID;
        //                var catName = c.Name;

        //                result.Add(new GridModel()
        //                {
        //                    MovieID = movID,
        //                    MovieName = movName,
        //                    CategoryID = catID,
        //                    CategoryName = catName,
        //                });
        //            }
        //        }
                
        //    }

        //    return result;
        //}
    }
}