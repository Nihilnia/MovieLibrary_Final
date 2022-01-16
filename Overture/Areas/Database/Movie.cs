using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Overture.Areas.Database
{
    public class Movie
    {
        public Movie()
        {
            this.Categories = new List<Category>();
        }

        [Key]
        public int ID { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }
        //public User User { get; set; }
        public int UserID { get; set; }
        public List<Category> Categories { get; set; }
    }
}