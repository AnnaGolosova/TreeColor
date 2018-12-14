using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ThreeColor.Data.Models;
using TreeColor.Utils;

namespace TreeColor.Models
{
    public class UserDbContext : IdentityDbContext<ApplicationUser>
    {
        public UserDbContext()
            : base(ConfigManager.ConnectionString, throwIfV1Schema: false) { }

        public static UserDbContext Create()
        {
            return new UserDbContext();
        }
    }
}