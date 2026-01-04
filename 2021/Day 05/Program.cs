using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Day_05 {
    internal class Program : ProgramStructure<Grid> {

        Program() : base(new Parser()
            .Filter(new LineReader())
            .ForEach( // Convert each line input into a Line object
                new Parser<string>()
                .Filter(new SeparatedParser("->")) // Split into two strings, one for each point
                .FilterCreate<Point>()
                .ToArray()
                .Create<Line>()
            ).Create<Grid>()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
        }

        protected override object SolvePart1(Grid input) {
            return input.Intersections.Where(x => x.Value >= 2).Count();
        }

        protected override object SolvePart2(Grid input) {
            input.AddDiagonals();
            return input.Intersections.Where(x => x.Value >= 2).Count();
        }

    }
}
