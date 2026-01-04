using AdventOfCode.Parsing;
using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Day20 {
	class Input : IObjectParser<string[]> {

		public Tile Tile { get; private set; }

		public Input() {}

		public void Parse(string[] input) {
			this.Tile = new Tile(input);
		}
	}
}
