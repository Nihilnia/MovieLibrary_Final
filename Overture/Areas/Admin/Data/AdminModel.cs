using Overture.Areas.User.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Overture.Areas.Admin.Data
{
    public class AdminModel
    {
        public AdminModel()
        {
            this.GridModel = new List<GridModel>();
        }

        public int ID { get; set; }
        public string UserName { get; set; }
        public string ProfilePic { get; set; }
        public List<GridModel> GridModel { get; set; }
    }
}



