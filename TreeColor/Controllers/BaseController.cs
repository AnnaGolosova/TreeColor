using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TreeColor.Models;

namespace TreeColor.Controllers
{
    public class BaseController : Controller
    {
        public static UserDbContext UserDB;

        static BaseController()
        {
            if (UserDB == null)
                UserDB = new UserDbContext();
        }
    }
}