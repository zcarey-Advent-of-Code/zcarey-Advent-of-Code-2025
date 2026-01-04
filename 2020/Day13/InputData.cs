using AdventOfCode.Parsing;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day13 {
	class InputData : IObjectParser<string[]> {

		public int EarliestTime;
		public int[] BusIDs;

		public void Parse(string[] input) {
			EarliestTime = int.Parse(input[0]);
			BusIDs = input[1].Split(',').Select(x => (x == "x") ? -1 : int.Parse(x)).ToArray();
		}
	}
}
