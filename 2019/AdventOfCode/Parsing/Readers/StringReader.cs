using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode.Parsing {
	public class StringReader : IParser<StreamReader, string> {
		override internal string Parse(StreamReader input) {
			return input.ReadToEnd();
		}
	}
}
