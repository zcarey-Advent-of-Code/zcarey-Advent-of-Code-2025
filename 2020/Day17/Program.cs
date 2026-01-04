using AdventOfCode;
using AdventOfCode.Parsing;
using Common;
using System;

namespace Day17 {
	class Program : ProgramStructure<Map> {

		Program() : base(new Parser()
			.Filter(new LineReader())
			.ToArray()
			.Create<Map>()
		) { }

		static void Main(string[] args) {
			new Program().Run(args);
			//new Program().Run("Example.txt");
		}

		protected override object SolvePart1(Map input) {
			for(int i = 0; i < 6; i++) {
				input.Simulate(2, 3, 3);
			}
			return input.CountActive().ToString();
		}

		protected override object SolvePart2(Map input) {
			input.Initialize4D();
			for (int i = 0; i < 6; i++) {
				input.Simulate(2, 3, 3);
			}
			return input.CountActive().ToString();
		}
	}
}
