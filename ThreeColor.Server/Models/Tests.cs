using System;
using System.Collections.Generic;
using System.Text;

namespace ThreeColor.Server.Models
{
    public class Tests
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FieldColor { get; set; }
        public int PointSize { get; set; }
        public int Speed { get; set; }
        public int MinInterval { get; set; }
        public int MaxInterval { get; set; }
        public int IsDeleted { get; set; }
    }
}
