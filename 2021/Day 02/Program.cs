using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_02 {
    internal class Program : ProgramStructure<IEnumerable<SubInstruction>> {

        Program() : base(new Parser()
            .Filter(new LineReader())
            .Filter(x => x.Split())
            .FilterCreate<SubInstruction>()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
        }

        protected override object SolvePart1(IEnumerable<SubInstruction> input) {
            Submarine position = new();

            foreach(SubInstruction instruction in input) {
                if (instruction.Direction == SubDirection.forward) {
                    position.Horizontal += instruction.Units;
                } else if (instruction.Direction == SubDirection.up) {
                    position.Depth -= instruction.Units;
                } else if (instruction.Direction == SubDirection.down) {
                    position.Depth += instruction.Units;
                }
            }

            return position.Horizontal * position.Depth;
        }

        protected override object SolvePart2(IEnumerable<SubInstruction> input) {
            Submarine position = new();

            foreach (SubInstruction instruction in input) {
                if (instruction.Direction == SubDirection.up) {
                    position.Aim -= instruction.Units;
                } else if (instruction.Direction == SubDirection.down) {
                    position.Aim += instruction.Units;
                } else if (instruction.Direction == SubDirection.forward) {
                    position.Horizontal += instruction.Units;
                    position.Depth += position.Aim * instruction.Units;
                }
            }

            return position.Horizontal * position.Depth;
        }

    }
}
