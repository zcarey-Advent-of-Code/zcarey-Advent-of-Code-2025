using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Parsing {
	public class SeparatedParser : IParser<string, IEnumerable<string>> {

		private string separator;

		public SeparatedParser() {
			separator = null;
		}

		public SeparatedParser(string separator) {
			this.separator = separator;
		}

		internal override IEnumerable<string> Parse(string input) {
			if (separator != null) {
				return input.Split(separator);
			} else {
				return input.Split();
			}
		}
	}
}
