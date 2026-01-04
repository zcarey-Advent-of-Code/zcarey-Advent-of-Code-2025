using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_02 {
    public enum SubDirection {
        forward,
        down,
        up
    }

    public struct SubInstruction : IObjectParser<string[]> {
        public SubDirection Direction;
        public int Units;

        public SubInstruction(SubDirection direction, int units) {
            Direction = direction;
            Units = units;
        }

        public void Parse(string[] input) {
            Direction = Enum.Parse<SubDirection>(input[0]);
            Units = int.Parse(input[1]);
        }

    }
}
