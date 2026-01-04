using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Day_01 {
	class Program : ProgramStructure<IEnumerable<int>> {

		Program() : base(new Parser()
			.Parse(new LineReader())
			.Filter(new TextBlockFilter())
			.ForEach(
				new Parser<string[]>().ForEach(int.Parse)
			)
			.ForEach(x => x.Sum())
		) { }

		static void Main(string[] args) {
			new Program().Run(args);
		}

		protected override object SolvePart1(IEnumerable<int> input) {
			return input.Max();
		}

		protected override object SolvePart2(IEnumerable<int> input) {
			List<int> calories = input.ToList();
			calories.Sort();
			return calories.Reverse<int>().Take(3).Sum();
		}

	}
}
