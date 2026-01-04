using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_04 {
    internal class Program : ProgramStructure<IEnumerable<Range[]>> {

        Program() : base(new Parser()
            .Parse(new LineReader())
            .ForEach(
                new Parser<string>() // Each line of the input
                .Parse(new SeparatedParser(","))
                .ForEach(x => { // Left elf vs right elf
                    string[] str = x.Split('-');
                    return new Range(int.Parse(str[0]), int.Parse(str[1]));
                }).ToArray() // A pair of ranges for each pair of elves
            )
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
        }

        protected override object SolvePart1(IEnumerable<Range[]> input) {
            return input.Where(x => x[0].ContainsRange(x[1]) || x[1].ContainsRange(x[0])).Count();
        }

        protected override object SolvePart2(IEnumerable<Range[]> input) {
            return input.Where(x => x[0].Intersects(x[1])).Count();
        }

    }

    public static class RangeExtensions {

        public static bool ContainsRange(this Range range, Range other) {
            return (other.Start.Value >= range.Start.Value && other.End.Value <= range.End.Value);
        }

        public static bool Intersects(this Range range, Range other) {
            return (other.Start.Value <= range.End.Value && other.End.Value >= range.Start.Value);
        }

    }
}
