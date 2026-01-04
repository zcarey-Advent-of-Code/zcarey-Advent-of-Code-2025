using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_02 {
    internal class Program : ProgramStructure<IEnumerable<StrategyGuideEntry>> {

        Program() : base(new Parser()
            .Parse(new LineReader())
            .ForEach(x => x.Split(" "))
            .FilterCreate<StrategyGuideEntry>()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
        }

        protected override object SolvePart1(IEnumerable<StrategyGuideEntry> input) {
            return input.Select(x => x.Score()).Sum();
        }

        protected override object SolvePart2(IEnumerable<StrategyGuideEntry> input) {
            return input.Select(x => x.ChangeChoiceToGetResult().Score()).Sum();
        }

    }
}
