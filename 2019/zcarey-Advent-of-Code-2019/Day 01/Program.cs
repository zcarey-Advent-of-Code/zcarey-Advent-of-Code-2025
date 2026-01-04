using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_01 {
	class Program : ProgramStructure<IEnumerable<int>> {

		Program() : base(new Parser()
			.Filter(new LineReader())
			.Filter(int.Parse)
		) { }

		static void Main(string[] args) {
			new Program().Run(args);
		}

		protected override object SolvePart1(IEnumerable<int> mass) {
			IEnumerable<int> fuelRequirement = mass.Select(x => x / 3 - 2);
			return fuelRequirement.Sum();
		}

		protected override object SolvePart2(IEnumerable<int> input) {
			return input.Select(x => FuelRequired(x)).Sum();
		}

		int FuelRequired(int mass) {
			int fuel = Math.Max(0, mass / 3 - 2);
			if (fuel > 0) {
				fuel += FuelRequired(fuel);
			}
			return fuel;
		}

	}
}
