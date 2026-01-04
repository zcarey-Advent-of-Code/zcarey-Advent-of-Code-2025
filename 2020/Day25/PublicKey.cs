using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Day25 {
	class PublicKey : IObjectParser<string> {

		public long Key { get; private set; }

		public void Parse(string input) {
			this.Key = long.Parse(input);
		}

		public static long Transform(long subjectMatter, int loopSize) {
			long result = 1;
			for(int i = 0; i < loopSize; i++) {
				result *= subjectMatter;
				result %= 20201227;
			}
			return result;
		}

		public int FindLoopSize(long subjectMatter) {
			int loopSize = 0;
			long result = 1;
			while(result != this.Key) {
				loopSize++;
				result *= subjectMatter;
				result %= 20201227;
			}
			return loopSize;
		}

	}
}
