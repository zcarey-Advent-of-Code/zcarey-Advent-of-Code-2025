using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_06 {
    internal class Program : ProgramStructure<string> {

        Program() : base(new Parser()
            .Parse(new StringReader())
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
        }

        protected override object SolvePart1(string input) {
            // Lol this is so bad and slow but I dont care right now
            for(int i = 0; i < input.Length - 4; i++) {
                if (input.Skip(i).Take(4).Distinct().Count() == 4) {
                    return i + 4;
                }
            }
            return "Error";
        }

        protected override object SolvePart2(string input) {
            // Lol this is so bad and slow but I dont care right now
            for (int i = 0; i < input.Length - 4; i++) {
                if (input.Skip(i).Take(14).Distinct().Count() == 14) {
                    return i + 14;
                }
            }
            return "Error";
        }

    }
}
