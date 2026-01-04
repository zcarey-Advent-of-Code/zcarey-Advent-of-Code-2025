using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day__07 {
    internal class Program : ProgramStructure<string> {

        Program() : base(new Parser()
            .Parse(new StringReader())
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
        }

        protected override object SolvePart1(string input) {
            return input;
        }

        protected override object SolvePart2(string input) {
            return null;
        }

    }
}
