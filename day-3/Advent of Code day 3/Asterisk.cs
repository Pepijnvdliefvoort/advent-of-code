using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_day_3
{
    internal class Asterisk
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Value { get; set; }

        public Asterisk(int x, int y, int value)
        {
            X = x;
            Y = y;
            Value = value;
        }
    }
}
