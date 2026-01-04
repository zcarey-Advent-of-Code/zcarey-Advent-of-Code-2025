using AdventOfCode;
using AdventOfCode.Parsing;
using Common;
using System.Linq;
using System.Numerics;

namespace Day18 {
	class Program : ProgramStructure<Expression[]> {

		Program() : base(new Parser()
			.Filter(new LineReader())
			.Filter(Expression.Parse)
			.ToArray()
		) { }

		static void Main(string[] args) {
			new Program().Run(args);
			//new Program().Run("Example26.txt");
			//new Program().Run("Example51.txt");
			//new Program().Run("Example71.txt");
			//new Program().Run("Example437.txt");
			//new Program().Run("Example12240.txt");
			//new Program().Run("Example13632.txt");
		}

		protected override object SolvePart1(Expression[] input) {
			BigInteger result = BigInteger.Zero;
			foreach(BigInteger answer in input.Select(x => x.Calculate(false))) {
				result += answer;
			}
			return result.ToString();
		}

		protected override object SolvePart2(Expression[] input) {
			BigInteger result = BigInteger.Zero;
			foreach (BigInteger answer in input.Select(x => x.Calculate(true))) {
				result += answer;
			}
			return result.ToString();
		}
	}
}
