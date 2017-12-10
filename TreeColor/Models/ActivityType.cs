using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TreeColor.Models
{
    public enum ActivityType
    {
        [Description("школьник")]
        Schoolkid = 0,

        [Description("студент")]
        Student,

        [Description("рабочий")]
        Worker
    }
}