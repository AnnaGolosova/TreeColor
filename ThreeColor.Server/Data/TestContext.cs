using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TreeColor.Models;

namespace TreeColor.Server.Data
{
    public class TestContext : DbContext
    {
        public TestContext()
            : base("SomeeDotCom") { }

        public virtual DbSet<Tests> Tests { get; set; }
    }
}