using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TreeColor.Models;

namespace TreeColor.Utils
{
    public static class ExtentionMethods
    {
        public static bool CompareTo(this Users first, Users second)
        {
            return string.Equals(first.Activity, second.Activity) &&
                string.Equals(first.Gender, second.Gender) &&
                first.Age == second.Age;
        }
    }

}