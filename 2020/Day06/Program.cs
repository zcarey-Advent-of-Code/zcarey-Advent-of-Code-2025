using AdventOfCode;
using AdventOfCode.Parsing;
using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Day6 {
	class Program : ProgramStructure<IEnumerable<string[]>> {

		Program() : base(new Parser()
			.Filter(new LineReader())
			.Filter(new TextBlockFilter())
		) { }

		static void Main(string[] args) {
			new Program().Run(args);
		}

		protected override object SolvePart1(IEnumerable<string[]> input) {
			int count = 0;
			foreach (string[] group in input) {
				count += calculateGroup(group);
			}

			return count.ToString();
		}

		static int calculateGroup(string[] input) {
			List<char> questions = new List<char>();
			foreach (string line in input) {
				foreach (char c in line) {
					if (!questions.Contains(c)) {
						questions.Add(c);
					}
				}
			}
			return questions.Count;
		}

		protected override object SolvePart2(IEnumerable<string[]> input) {
			int count = 0;
			foreach (string[] group in input) {
				count += calculateGroup2(group);
			}

			return count.ToString();
		}

		static int calculateGroup2(string[] input) {
			Dictionary<char, int> questions = new Dictionary<char, int>();
			int people = 0;
			foreach (string line in input) {
				people++;
				foreach (char c in line) {
					if (!questions.ContainsKey(c)) {
						questions[c] = 1;
					} else {
						questions[c]++;
					}
				}
			}

			int count = 0;
			foreach (KeyValuePair<char, int> pair in questions) {
				if (pair.Value == people) {
					count++;
				}
			}
			return count;
		}
	}
}
