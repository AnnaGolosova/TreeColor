﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ThreeColor.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public string Activity { get; set; }
        public string Gender { get; set; }
        public Guid NewId { get; set; }

        public virtual IEnumerable<Result> Results { get; set; }
    }
}