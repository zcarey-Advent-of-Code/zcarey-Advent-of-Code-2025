using AdventOfCode;
using AdventOfCode.Parsing;
using Common;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day2 {
	class Program : ProgramStructure<string[]> {

		Program() : base(new Parser()
			.Filter(new LineReader())
			.ToArray()
		) {

		}

		static void Main(string[] args) {
			new Program().Run(args);
		}

		protected override object SolvePart1(string[] input) {
			// A bit dirty but I think it still gets the point across
			return input.Select(x => processInputLine(x)).Where(x => x == true).Count().ToString();
		}

		protected override object SolvePart2(string[] input) {
			// A bit dirty but I think it still gets the point across
			return input.Select(x => processInputLinePart2(x)).Where(x => x == true).Count().ToString();
		}

		static bool processInputLine(string input) {
			int separator = input.IndexOf(':');
			return new Policy(input).isValidPassword(input.Substring(separator + 2));
		}

		static bool processInputLinePart2(string input) {
			int separator = input.IndexOf(':');
			return new Policy(input).isValidPart2Password(input.Substring(separator + 2));
		}
	}
}
