using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Day7 {
	class Rule : IObjectParser<string> {

		public string BagColor;
		public Dictionary<string, int> Rules = new Dictionary<string, int>();
		bool NoBags { get => Rules.Count == 0; }

		public void Parse(string input) {
			int index = input.IndexOf("bags");
			BagColor = input.Substring(0, index - 1);

			string data = input.Substring(index + 13);
			if (data != "no other bags.") {
				string[] rules = data.Split(", ");
				foreach (string rawRule in rules) {
					index = rawRule.IndexOf(' ');
					int count = int.Parse(rawRule.Substring(0, index));
					string color = trimBagColor(rawRule.Substring(index + 1));
					this.Rules[color] = count;
				}
			}
		}

		private string trimBagColor(string input) {
			string color = input.Trim().Trim('.').TrimEnd('s');
			return color.Substring(0, color.Length - 4);
		}

	}
}
