using Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AdventOfCode.Parsing;

namespace Day16 {
	class InputData : IObjectParser<IEnumerable<string[]>> {

		public Rule[] Rules { get; private set; }
		public int TicketLength { get; private set; }
		public int[] MyTicket { get; private set; }
		public int[][] OtherTickets { get; private set; }

		public void Parse(IEnumerable<string[]> input) {
			IEnumerator<IEnumerable<string>> enumerator = input.GetEnumerator();
			enumerator.MoveNext();
			parseRules(enumerator.Current);
			enumerator.MoveNext();
			parseMyTicket(enumerator.Current);
			enumerator.MoveNext();
			parseOtherTickets(enumerator.Current);

			TicketLength = MyTicket.Length;
		}

		private void parseRules(IEnumerable<string> input) {
			this.Rules = input.Select(x => new Rule(x)).ToArray();
		}

		private void parseMyTicket(IEnumerable<string> input) {
			if (!input.First().StartsWith("your ticket:")) throw new Exception("Bad input.");
			MyTicket = input.Skip(1).First().Split(',').Select(int.Parse).ToArray();
		}

		private void parseOtherTickets(IEnumerable<string> input) {
			if (!input.First().StartsWith("nearby tickets:")) throw new Exception("Bad input.");
			OtherTickets = input.Skip(1).Select(x => x.Split(',').Select(int.Parse).ToArray()).ToArray();
		}
	}
}
