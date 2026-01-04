using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Day_01 {
	class Program : ProgramStructure<IEnumerable<int>> {

		Program() : base(new Parser()
			.Filter(new LineReader())
			.Filter(int.Parse)
		) { }

		static void Main(string[] args) {
			new Program().Run(args);
			//new Program().Run(args, "Example.txt");
		}

		protected override object SolvePart1(IEnumerable<int> input) {
			int increasedCount = 0;
			int previousDepth = input.First();

			foreach(int depth in input.Skip(1))
            {
				if(depth > previousDepth)
                {
					increasedCount++;
                }
				previousDepth = depth;
            }

			return increasedCount;
		}

		protected override object SolvePart2(IEnumerable<int> input) {
			int[] summingValues = input.Take(3).ToArray();
			int summingValuesIndex = 0;
			int averageDepth = summingValues.Sum();

			int increasedCount = 0;
			int previousDepth = averageDepth;

			foreach (int depth in input.Skip(3)) {
				averageDepth -= summingValues[summingValuesIndex];
				summingValues[summingValuesIndex] = depth;
				averageDepth += depth;

				if(averageDepth > previousDepth) {
					increasedCount++;
                }

				previousDepth = averageDepth;
				summingValuesIndex = (summingValuesIndex + 1) % 3;
			}

			return increasedCount;
		}

	}
}
