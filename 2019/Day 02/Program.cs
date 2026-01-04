using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_02 {
	class Program : ProgramStructure<IntcodeProgram> {

		Program() : base(new Parser()
			.Parse(new StringReader())
			.Filter(new SeparatedParser(","))
			.Filter(int.Parse)
			.ToArray()
			.Create<IntcodeProgram>()
		) { }

		static void Main(string[] args) {
			new Program().Run(args);
		}

		protected override object SolvePart1(IntcodeProgram program) {
			program[1] = 12;
			program[2] = 2;
			program.Run();
			return program[0];
		}

		protected override object SolvePart2(IntcodeProgram program) {
			for(int noun = 0; noun <= 99; noun++) {
				for(int verb = 0; verb <= 99; verb++) {
					IntcodeProgram test = new IntcodeProgram(program);
					test[1] = noun;
					test[2] = verb;
					test.Run();
					if(test[0] == 19690720) {
						return 100 * noun + verb;
					}
				}
			}

			return "Could not find solution.";
		}

	}
}
