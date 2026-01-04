using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_09 {
    public struct Direction : IObjectParser<string[]> {

        public int DX;
        public int DY;
        public int Count;

        public static Point operator+(Point left, Direction right) {
            return new Point(left.X + right.DX, left.Y + right.DY);
        }

        public void Parse(string[] input) {
            switch (input[0]) {
                case "R":
                    DX = 1;
                    DY = 0;
                    break;
                case "L":
                    DX = -1;
                    DY = 0;
                    break;
                case "U":
                    DX = 0;
                    DY = -1;
                    break;
                case "D":
                    DX = 0;
                    DY = 1;
                    break;
                default:
                    throw new Exception("Invalid input.");
            }
            Count = int.Parse(input[1]);
        }
    }
}
