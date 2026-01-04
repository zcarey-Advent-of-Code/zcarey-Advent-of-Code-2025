using AdventOfCode;
using AdventOfCode.Parsing;
using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day7 {
	class Program : ProgramStructure<Rule[]> {

		Program() : base(new Parser()
			.Filter(new LineReader())
			.FilterCreate<Rule>()
			.ToArray()
		) {}

		static void Main(string[] args) {
			new Program().Run(args);
			//new Program().Run("Example.txt");
			//new Program().Run("Example2.txt");
		}

		protected override object SolvePart1(Rule[] input) {
			Dictionary<string, bool> validColors = new Dictionary<string, bool>();
			foreach (Rule rule in input) {
				validColors[rule.BagColor] = false;
			}
			//validColors["shiny gold"] = true;
			bool tryAgain = true;
			while (tryAgain) {
				tryAgain = false;
				foreach (Rule rule in input) {
					if (validColors[rule.BagColor] == false) {
						bool anyColorValid = false;
						foreach (KeyValuePair<string, int> pair in rule.Rules) {
							if (pair.Key == "shiny gold" || validColors[pair.Key] == true) {
								anyColorValid = true;
								break;
							}
						}
						validColors[rule.BagColor] = anyColorValid;
						if (anyColorValid) {
							tryAgain = true;
						}
					}
				}
			}

			int count = validColors.Where(x => x.Value == true).Count();
			return count.ToString();
		}

		protected override object SolvePart2(Rule[] input) {
			Dictionary<string, int> bagCount = new Dictionary<string, int>();
			return calculateBags(input, bagCount, "shiny gold").ToString();
		}

		static int calculateBags(Rule[] input, Dictionary<string, int> bagCount, string color) {
			if (bagCount.ContainsKey(color)) return bagCount[color];

			Rule bagRule = input.Where(x => x.BagColor == color).First();
			int bags = 0;
			foreach (KeyValuePair<string, int> rule in bagRule.Rules) {
				if (bagCount.ContainsKey(rule.Key)) {
					bags += rule.Value + bagCount[rule.Key] * rule.Value;
				} else {
					bags += rule.Value + calculateBags(input, bagCount, rule.Key) * rule.Value;
				}
			}

			bagCount[color] = bags;
			return bags;
		}
	}
}
