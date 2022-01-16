using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Overture.Areas.Database
{
    public class Category
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }
        public Movie Movie { get; set; }
        public int MovieID { get; set; }
    }
}