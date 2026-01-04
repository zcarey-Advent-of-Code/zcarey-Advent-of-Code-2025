using AdventOfCode.Parsing;
using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Day19 {

	class Input : IObjectParser<string[]> {

		public Dictionary<int, Rule> Rules { get; } = new Dictionary<int, Rule>();
		private List<string> messages = new List<string>();

		public IEnumerable<string> Messages { get => messages; }

		public Rule Rule0 { get => Rules[0]; }

		public void Parse(string[] input) {
			foreach(string line in input) {
				Rule rule = ParseRule(line);
				/*if(rule != null) {
					Rules[rule.ID] = rule;
				} else {*/
				if(rule == null) { 
					messages.Add(line);
				}
			}
		}

		public Rule ParseRule(string line) {
			int index = line.IndexOf(':');
			if (index >= 0) {
				Rule rule = Rule.Parse(line, index);
				Rules[rule.ID] = rule;
				return rule;
			} else {
				return null;
			}
		}

	}
}
