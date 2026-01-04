using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Day_11 {
	class Program : ProgramStructure<Monkey[]> {

		Program() : base(new Parser()
			.Parse(new LineReader())
			.Filter(new TextBlockFilter())
			.FilterCreate<Monkey>()
			.ToArray()
		) { }

		static void Main(string[] args) {
			new Program().Run(args);
			//new Program().Run(args, "Example.txt");
		}

		protected override object SolvePart1(Monkey[] input) {
			long[] inspections = new long[input.Length];
			Func<long, long> WorryReducer = x => x / 3;
			for(int round = 0; round < 20; round++) {
				SimulateRound(input, inspections, WorryReducer);
            }
			long[] TopTwoMonkeys = inspections.OrderByDescending(x => x).Take(2).ToArray();
			return TopTwoMonkeys[0] * TopTwoMonkeys[1];
		}

		private void SimulateRound(Monkey[] monkeys, long[] inspections, Func<long, long> WorryReducer) {
			for(int i = 0; i < monkeys.Length; i++) {
				monkeys[i].Simulate(monkeys, ref inspections[i], WorryReducer);
            }
        }

		protected override object SolvePart2(Monkey[] input) {
			// Calculate prime product
			long primeProduct = 1;
			foreach(Monkey monkey in input) {
				primeProduct *= monkey.Test;
            }

			long[] inspections = new long[input.Length];
			Func<long, long> WorryReducer = x => x % primeProduct;
			for (int round = 0; round < 10000; round++) {
				SimulateRound(input, inspections, WorryReducer);
			}
			long[] TopTwoMonkeys = inspections.OrderByDescending(x => x).Take(2).ToArray();
			return (long)TopTwoMonkeys[0] * (long)TopTwoMonkeys[1];
		}

	}
}
