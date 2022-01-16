using Microsoft.EntityFrameworkCore;
using Overture.Areas.User.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Overture.Areas.Database
{
    public static class GridProcesses
    {
        //READ
        public static List<GridModel> GETMFGET(string daUserName)
        {
            var result = new List<GridModel>();

            using (var DB = new OvertureContext())
            {
                var findUser = DB.Users.Where(f => f.UserName == daUserName).FirstOrDefault();

                var userMovies = DB.Movies.Where(f => f.UserID == findUser.ID).Include(c => c.Categories).ToList();

                findUser.Movies = userMovies;

                foreach (var f in findUser.Movies)
                {
                    var movID = f.ID;
                    var movName = f.Name;
                    foreach (var c in f.Categories)
                    {
                        var catID = c.ID;
                        var catName = c.Name;


                        result.Add(new GridModel()
                        {
                            MovieID = movID,
                            MovieName = movName,
                            CategoryID = catID,
                            CategoryName = catName,
                        });
                    }
                }

            }

            return result;
        }



        //ADD
        public static void AddMovie(string daUsername, string daMovieName, string daCategoryName)
        {
            using (var DB = new OvertureContext())
            {
                var findUser = DB.Users.Where(f => f.UserName == daUsername).FirstOrDefault();
                var newMovie = new Movie() { Name = daMovieName };
                var newCat = new Category() { Name = daCategoryName };

                newMovie.Categories.Add(newCat);
                findUser.Movies.Add(newMovie);
                DB.SaveChanges();
            }
        }



        //UPDATE
        public static void UpdateMovie(Movie daIncominMovie, GridModel daIncominModel)
        {
            using (var DB = new OvertureContext())
            {
                var findMovie = DB.Movies.Find(daIncominMovie.ID);
                findMovie.Name = daIncominModel.MovieName;

                //var catID = DB.Categories.FromSqlRaw($@"SELECT ID FROM Categories WHERE MovieID = {daIncominMovie.ID}");
                //var getCat = DB.Categories.Find(catID);

                var getCat = DB.Categories.Where(c => c.MovieID == findMovie.ID).ToList();

                foreach (var f in getCat)
                {
                    DB.Categories.Remove(f);
                }

                findMovie.Categories.Add(new Category() { Name = daIncominModel.CategoryName });

                DB.SaveChanges();
            }
        }


        //DELETE
        public static void DeleteMovie(Movie daIncominMovie)
        {
            using (var DB = new OvertureContext())
            {
                var findMovie = DB.Movies.Find(daIncominMovie.ID);
                DB.Movies.Remove(findMovie);

                DB.SaveChanges();
            }
        }
    }
}