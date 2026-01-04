using AdventOfCode;
using AdventOfCode.Parsing;
using Common;
using System;

namespace Day25 {
	class Program : ProgramStructure<PublicKey[]> {

		Program() : base(new Parser()
			.Filter(new LineReader())
			.FilterCreate<PublicKey>()
			.ToArray()
		) { }

		static void Main(string[] args) {
			new Program().Run(args);
			//new Program().Run("Example.txt");
		}

		protected override object SolvePart1(PublicKey[] input) {
			PublicKey cardPublicKey = input[0];
			PublicKey doorPublicKey = input[1];

			int cardLoopSize = cardPublicKey.FindLoopSize(7);
			int doorLoopSize = doorPublicKey.FindLoopSize(7);

			long encryptionKey = PublicKey.Transform(doorPublicKey.Key, cardLoopSize);
			return encryptionKey.ToString();
		}

		protected override object SolvePart2(PublicKey[] input) {
			return null;
		}
	}
}
