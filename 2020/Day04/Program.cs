using AdventOfCode;
using AdventOfCode.Parsing;
using Common;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day4 {
	class Program : ProgramStructure<Passport[]> {

		Program() : base(new Parser()
			.Filter(new LineReader())
			.Filter(new TextBlockFilter())
			.ForEach(
				// For each text block:
				new Parser<string[]>()
				.ForEach(
					// For each line in a text block...
					new SeparatedParser()
				) //So now we have a list of each line which contains a list of the elements in that line
				.Combine() // Turns it into a single list containing all the elements
				.ToArray() // Change the list into an array
				.Create<Passport>()
			) //ForEach returns a list of passports
			.ToArray()
		) { }

		static void Main(string[] args) {
			new Program().Run(args);
			//new Program().Run("Valids.txt");
			//new Program().Run("Examples.txt");
		}

		protected override object SolvePart1(Passport[] input) {
			int valid = 0;
			foreach (Passport passport in input) {
				passport.Validate();
				if (passport.IsValid) {
					valid++;
				}
			}
			return valid.ToString();
		}

		protected override object SolvePart2(Passport[] input) {
			int valid = 0;
			foreach (Passport passport in input) {
				passport.Validate2();
				if (passport.IsValid) {
					valid++;
				}
			}
			return valid.ToString();
		}
	}
}
