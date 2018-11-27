using System;
using System.Collections.Generic;
using System.Text;

namespace ThreeColor.Server.Models
{
    public class Points
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public string Color { get; set; }
        public string Key { get; set; }
        public int IsDeleted { get; set; }

        public virtual Tests Test { get; set; }
        public virtual IEnumerable<Results> Resilts { get; set; }
    }
}
