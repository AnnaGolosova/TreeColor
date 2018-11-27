using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThreeColor.Server.Data
{
    public class AuthDataContext : IdentityDbContext<IdentityUser>
    {
        public AuthDataContext()
            : base("Test2")
        {

        }
    }
}