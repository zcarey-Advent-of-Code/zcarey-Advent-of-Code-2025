using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_05 {
    public struct Point : IObjectParser<string> {

        public int X;
        public int Y;

        public Point(int x, int y) {
            X = x;
            Y = y;
        }

        public void Parse(string input) {
            string[] inputs = input.Trim().Split(',');
            X = int.Parse(inputs[0]);
            Y = int.Parse(inputs[1]);
        }
    }
}
