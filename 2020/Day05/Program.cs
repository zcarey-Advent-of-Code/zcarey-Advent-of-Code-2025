using AdventOfCode;
using AdventOfCode.Parsing;
using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Day5 {
	class Program : ProgramStructure<Seat[]> {

		Program() : base(new Parser()
			.Filter(new LineReader())
			.FilterCreate<Seat>()
			.ToArray()
		) { }

		static void Main(string[] args) {
			new Program().Run(args);
		}

		protected override object SolvePart1(Seat[] input) {
			return input.Max(x => x.ID).ToString();
		}

		protected override object SolvePart2(Seat[] input) {
			Seat[,] map = fillMap(input);
			for (int y = 0; y <= 127; y++) {
				for (int x = 1; x <= 6; x++) {
					if (!map[x, y].Filled && map[x - 1, y].Filled && map[x + 1, y].Filled) {
						return map[x, y].ID.ToString();
					}
				}
			}
			throw new Exception("Unable to solve!");
		}

		private static Seat[,] fillMap(Seat[] seats) {
			Seat[,] map = new Seat[8, 128];
			foreach (Seat seat in seats) {
				map[seat.Column, seat.Row] = seat;
			}
			for (int y = 0; y <= 127; y++) {
				for (int x = 0; x <= 7; x++) {
					if (!map[x, y].Filled) {
						map[x, y].Column = x;
						map[x, y].Row = y;
					}
				}
			}
			return map;
		}

		static void printPart2Debug(Seat[] input) {
			Seat[,] map = fillMap(input);
			for (int y = 0; y < 128; y++) {
				StringBuilder sb = new StringBuilder();
				for (int x = 0; x < 8; x++) {
					if (map[x, y].Filled) {
						sb.Append('#');
					} else {
						sb.Append('.');
					}
				}
				Console.WriteLine(sb);
			}
		}
	}
}
