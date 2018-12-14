using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TreeColor.Models
{
    public class ResultViewModel
    {
        public double Time { get; set; }
        public int FirstErrorCount { get; set; }
        public int SecondErrorCount { get; set; }
    }
}