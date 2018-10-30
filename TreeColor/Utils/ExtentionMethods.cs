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
        
        public static void UpdateTest(this Tests oldTest, Tests newTest)
        {
            oldTest.field_color = newTest.field_color;
            oldTest.int_max = newTest.int_max;
            oldTest.int_min = newTest.int_min;
            oldTest.Point_Size = newTest.Point_Size;
            oldTest.Speed = newTest.Speed;
            oldTest.test_name = newTest.test_name;

            oldTest.Points.Clear();
            oldTest.Points = newTest.Points.Select(p => new Points()
            {
                color = p.color,
                id = p.id,
                Symbol = p.Symbol,
                testid = oldTest.id
            }).ToList();
        }
    }
}