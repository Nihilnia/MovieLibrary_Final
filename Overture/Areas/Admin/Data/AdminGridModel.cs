using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Overture.Areas.Admin.Data
{
    public class AdminGridModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int MovieID { get; set; }
        public string MovieName { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}