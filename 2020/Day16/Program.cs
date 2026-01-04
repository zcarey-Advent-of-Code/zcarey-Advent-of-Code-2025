using AdventOfCode;
using AdventOfCode.Parsing;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day16 {
	class Program : ProgramStructure<InputData> {

		Program() : base(new Parser()
			.Filter(new LineReader())
			.Filter(new TextBlockFilter())
			.Create<InputData>()
		) { }

		static void Main(string[] args) {
			new Program().Run(args);
			//new Program().Run("example.txt");
		}

		protected override object SolvePart1(InputData input) {
			int ticketScanningErrorRate = 0;
			foreach(int[] ticket in input.OtherTickets) {
				foreach(int number in ticket) {
					if(!FindValidRules(input, number).Any()) {
						ticketScanningErrorRate += number;
					}
				}
			}
			return ticketScanningErrorRate.ToString();
		}

		private IEnumerable<Rule> FindValidRules(InputData input, int num) {
			foreach(Rule rule in input.Rules) {
				if (rule.FitsInRange(num)) {
					yield return rule;
				}
			}
		}

		protected override object SolvePart2(InputData input) {
			//Initialize all valid rules
			List<Rule>[] validRules = new List<Rule>[input.TicketLength];
			for(int i = 0; i < validRules.Length; i++) {
				validRules[i] = new List<Rule>(input.Rules);
			}

			//Determine valid rules by checking against valid tickets
			foreach(int[] ticket in GetValidTickets(input)) {
				for(int i = 0; i < ticket.Length; i++) {
					validRules[i].RemoveAll(rule => !rule.FitsInRange(ticket[i]));
				}
			}

			//Keep eliminating rules by finding fields that only have 1 validm rule and remove that rule from all other fields
			bool edited = true;
			while (edited) {
				edited = false;
				foreach(List<Rule> rules in validRules) {
					if(rules.Count == 1) {
						Rule confirmedRule = rules[0];
						foreach(List<Rule> otherRules in validRules.Where(x => x != rules)) {
							edited |= otherRules.Remove(confirmedRule);
						}
					}
				}
			}

			//Assert that we found them all correctly!
			foreach(List<Rule> rules in validRules) {
				if(rules.Count != 1) {
					throw new Exception("Could not determine rules!");
				}
			}

			//Calcualte answer
			long result = 1;
			for(int i = 0; i < input.MyTicket.Length; i++) {
				Rule rule = validRules[i][0];
				int myTicketNumber = input.MyTicket[i];
				if (rule.Key.StartsWith("departure")) {
					result *= myTicketNumber;
				}
			}

			return result.ToString();
		}

		private IEnumerable<int[]> GetValidTickets(InputData input) {
			foreach(int[] ticket in input.OtherTickets) {
				bool validTicket = true;
				foreach(int number in ticket) {
					if(!FindValidRules(input, number).Any()) {
						validTicket = false;
						break;
					}
				}
				if (validTicket) {
					yield return ticket;
				}
			}
		}
	}
}
