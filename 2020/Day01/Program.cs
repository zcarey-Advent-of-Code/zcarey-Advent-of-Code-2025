using AdventOfCode;
using AdventOfCode.Parsing;
using Common;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day1 {
	class Program : ProgramStructure<int[]> { //ParsedInputProgramStructure<int> {

		Program() : base(new Parser()
			.Filter(new LineReader())
			.Filter(int.Parse)
			.ToArray()
		) {

		}

		static void Main(string[] args) {
			new Program().Run(args);
		}

		protected override object SolvePart1(int[] input) {
			for (int i = 0; i < input.Length - 1; i++) {
				for (int j = i + 1; j < input.Length; j++) {
					if (input[i] + input[j] == 2020) {
						return (input[i] * input[j]).ToString();
					}
				}
			}
			return new Exception("Unable to find the answer!!!");
		}

		protected override object SolvePart2(int[] input) {
			for (int i = 0; i < input.Length - 2; i++) {
				for (int j = i + 1; j < input.Length - 1; j++) {
					for (int k = j + 1; k < input.Length; k++) {
						if (input[i] + input[j] + input[k] == 2020) {
							return (input[i] * input[j] * input[k]).ToString();
						}
					}
				}
			}
			return new Exception("Unable to find the answer!!!");
		}
	}
}
