using System;
using System.Collections.Generic;
using System.Text;

namespace ThreeColor.Data.Models
{
    public class Result
    {
        public int Id { get; set; }
        public int TestingNumber { get; set; }
        public int UserId { get; set; }
        public int PointId { get; set; }
        public int Time { get; set; }
        public ErrorCodes ErrorCode { get; set; }
        public DateTime Date { get; set; }

        public virtual Point Point { get; set; }
        public virtual User User { get; set; }
    }
}
