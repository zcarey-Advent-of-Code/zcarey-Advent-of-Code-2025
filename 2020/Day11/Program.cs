using System;
using AdventOfCode;
using AdventOfCode.Parsing;
using Common;

namespace Day11 {
	class Program : ProgramStructure<Map>{

		Program() : base(new Parser()
			.Filter(new LineReader())
			.ToArray()
			.Create<Map>()
		) { }

		static void Main(string[] args) {
			new Program().Run(args);
		}

		protected override object SolvePart1(Map input) {
			bool updated = true;
			while (updated) {
				input.UpdateOccupiedCount();
				updated = input.UpdateSeatState(4);
			}
			return input.CountOccupiedSeats().ToString();
		}

		protected override object SolvePart2(Map input) {
			bool updated = true;
			while (updated) {
				input.UpdateOccupiedCount2();
				updated = input.UpdateSeatState(5);
			}
			return input.CountOccupiedSeats().ToString();
		}
	}
}
