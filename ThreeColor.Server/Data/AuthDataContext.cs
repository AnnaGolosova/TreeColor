using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TreeColor.Server.Data
{
    public class AuthDataContext : IdentityDbContext<IdentityUser>
    {
        public AuthDataContext()
            : base("DataContext")
        {

        }
    }
}