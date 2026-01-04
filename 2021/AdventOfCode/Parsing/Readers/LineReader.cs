using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode.Parsing {

	public class LineReader : IParser<StreamReader, IEnumerable<string>> {
		override internal IEnumerable<string> Parse(StreamReader input) {
			List<string> lines = new();

			string line;
			while((line = input.ReadLine()) != null) {
				lines.Add(line);
			}

			return lines;
		}
	}

}
