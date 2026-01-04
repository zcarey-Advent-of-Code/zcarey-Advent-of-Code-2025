using System;
using System.Collections.Generic;
using System.Text;

namespace Day16 {
	class Rule {

		public string Key { get; private set; }
		private Range range1;
		private Range range2;

		public Rule(string parse) {
			int keyIndex = parse.IndexOf(':');
			int rangeIndex = parse.IndexOf("or", keyIndex);
			this.Key = parse.Substring(0, keyIndex);
			this.range1 = new Range(parse.Substring(keyIndex + 1, rangeIndex - (keyIndex + 1)));
			this.range2 = new Range(parse.Substring(rangeIndex + 2));
		}

		public bool FitsInRange(int test) {
			return range1.InRange(test) || range2.InRange(test);
		}

		struct Range {
			int Low;
			int High;

			public Range(int low, int high) {
				this.Low = low;
				this.High = high;
			}

			public Range(string parse) {
				parse = parse.Trim();
				int index = parse.IndexOf('-');
				this.Low = int.Parse(parse.Substring(0, index));
				this.High = int.Parse(parse.Substring(index + 1));
			}

			public bool InRange(int value) {
				return (value >= this.Low) && (value <= this.High);
			}
		}

	}
}
