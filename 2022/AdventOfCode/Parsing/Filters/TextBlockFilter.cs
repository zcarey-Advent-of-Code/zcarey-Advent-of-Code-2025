using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Parsing {

	/// <summary>
	/// Returns the lines of strings that are separated by a single blank line (text blocks)
	/// </summary>
	public class TextBlockFilter : IParser<IEnumerable<string>, IEnumerable<string[]>> {
		internal override IEnumerable<string[]> Parse(IEnumerable<string> input) {
			List<string> group = new List<string>();
			foreach (string line in input) {
				if (string.IsNullOrWhiteSpace(line)) {
					yield return group.ToArray();
					group.Clear();
				} else {
					group.Add(line);
				}
			}

			if (group.Count > 0) {
				yield return group.ToArray();
			}
		}
	}
}
