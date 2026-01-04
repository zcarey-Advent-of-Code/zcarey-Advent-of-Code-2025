using AdventOfCode;
using AdventOfCode.Parsing;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day10 {
	class Program : ProgramStructure<int[]> {

		Program() : base(new Parser()
			.Filter(new LineReader())
			.Filter(int.Parse)
			.ToArray()
		) { }

		static void Main(string[] args) {
			new Program().Run(args);
			//new Program().Run(args, "Example.txt");
		}

		protected override object SolvePart1(int[] input) {
			Array.Sort(input);
			int jolt = 0;
			int delta1Jolt = 0;
			int delta3Jolt = 0;
			foreach(int adapter in input) {
				int delta = adapter - jolt;
				if (delta == 1) delta1Jolt++;
				else if (delta == 3) delta3Jolt++;
				jolt = adapter;
			}

			//Phone adapter is always 3 jolts above the highest adapter
			delta3Jolt++;

			return (delta1Jolt * delta3Jolt).ToString();
		}

		protected override object SolvePart2(int[] input) {
			Array.Sort(input);
			IEnumerable<int> adapters = input.Reverse().Concat(0);
			Dictionary<int, long> connections = new Dictionary<int, long>();
			int PhoneJolts = input[input.Length - 1] + 3;
			//Add the phone charger as a base case
			connections[PhoneJolts] = 1;
			foreach(int jolts in adapters) {
				long totalConnections = 0;
				for(int j = jolts + 1; j <= jolts + 3; j++) {
					if (connections.ContainsKey(j)) {
						totalConnections += connections[j];
					}
				}
				connections[jolts] = totalConnections;
			}
			return connections[0].ToString();
		}
	}
}
