using System;
using System.Collections.Generic;
using System.Text;

namespace ThreeColor.Data.Models
{
    public class Point
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public int Color { get; set; }
        public string Symbol { get; set; }

        public virtual Test Test { get; set; }
        public virtual IEnumerable<Result> Resilts { get; set; }
    }
}
