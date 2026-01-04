using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode.Parsing {

	public class LineReader : IParser<StreamReader, IEnumerable<string>> {
		override internal IEnumerable<string> Parse(StreamReader input) {
			// We must read everything into memory so that IEnumerables act as expected later.
			// If we don't, we won't be able to iterate over the values multiple times because the memory stream that is provided doesn't
			// reset the position when enumerating multiple times.
			List<string> lines = new();

			string line;
			while ((line = input.ReadLine()) != null) {
				lines.Add(line);
			}

			return lines;
		}
	}

}
