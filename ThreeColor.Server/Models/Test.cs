using System;
using System.Collections.Generic;
using System.Text;

namespace ThreeColor.Data.Models
{
    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FieldColor { get; set; }
        public int PointSize { get; set; }
        public int Speed { get; set; }
        public int MinimumInterval { get; set; }
        public int MaximumInterval { get; set; }

        public virtual IEnumerable<Point> Points { get; set; }
    }
}
