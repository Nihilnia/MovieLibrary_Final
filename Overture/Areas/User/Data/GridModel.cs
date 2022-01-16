using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Overture.Areas.User.Data
{
    public class GridModel
    {
        public int MovieID { get; set; }
        public string MovieName { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}