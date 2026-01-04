using AdventOfCode;
using AdventOfCode.Parsing;
using Common;
using System.Linq;

namespace Day19 {
	class Program : ProgramStructure<Input> {
		
		Program() : base(new Parser()
			.Filter(new LineReader())
			.ToArray()
			.Create<Input>()
		) { }

		static void Main(string[] args) {
			new Program().Run(args);
			//new Program().Run("Example.txt");
			//new Program().Run("Example2.txt", true);
		}

		protected override object SolvePart1(Input input) {
			return input.Messages.Where(x => input.Rule0.Match(input.Rules, x)).Count().ToString();
		}

		protected override object SolvePart2(Input input) {
			input.ParseRule("8: 42 | 42 8");
			input.ParseRule("11: 42 31 | 42 11 31"); 
			return input.Messages.Where(x => input.Rule0.MatchLooping(input.Rules, x)).Count().ToString();
		}
	}
}
