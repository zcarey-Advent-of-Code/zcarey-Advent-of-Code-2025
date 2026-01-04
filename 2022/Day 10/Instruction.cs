using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_10 {
    public struct Instruction : IObjectParser<string[]> {

        int instruction;
        int arg1;

        // Returns number of cycles to complete
        public int Execute(ref int registerX) {
            switch (instruction) {
                case 0:
                    return 1;
                case 1:
                    registerX += arg1;
                    return 2;
                default:
                    throw new Exception("Invalid instruction");
            }
        }

        public void Parse(string[] input) {
            switch (input[0]) {
                case "noop":
                    instruction = 0;
                    break;
                case "addx":
                    instruction = 1;
                    arg1 = int.Parse(input[1]);
                    break;
                default:
                    throw new Exception("Invalid instruction");
            }
        }
    }
}
