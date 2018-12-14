using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace TreeColor.Utils
{
    public class ConfigManager
    {
        public static string ServerName
        {
            get
            {
#if DEBUG
                return WebConfigurationManager.AppSettings["TestServerUrl"];
#elif RELEASE
                 return WebConfigurationManager.AppSettings["ProdServerUrl"];
#elif LOCAL_TEST
                 return WebConfigurationManager.AppSettings["LocalServerUrl"];
#elif LOCAL_PROD
                 return WebConfigurationManager.AppSettings["LocalServerUrl"];
#endif
            }
        }

        public static string ConnectionString
        {
            get
            {
#if DEBUG
                return WebConfigurationManager.ConnectionStrings["TestConnection"].ConnectionString;
#elif RELEASE
                return WebConfigurationManager.ConnectionStrings["ProdConnection"].ConnectionString;
#elif LOCAL_TEST
                return WebConfigurationManager.ConnectionStrings["TestConnection"].ConnectionString;
#elif LOCAL_PROD
                return WebConfigurationManager.ConnectionStrings["ProdConnection"].ConnectionString;
#endif
            }
        }

        public static string AdminId
        {
            get
            {
#if DEBUG
                return "A6F69003-A5C0-4C5E-9506-204C3DF4306A";
#elif RELEASE
                return "b90b3a12-2a08-4fca-9523-34217947404c";
#elif LOCAL_TEST
                return "A6F69003-A5C0-4C5E-9506-204C3DF4306A";
#elif LOCAL_PROD
                return "b90b3a12-2a08-4fca-9523-34217947404c";
#endif
            }
        }
    }
}