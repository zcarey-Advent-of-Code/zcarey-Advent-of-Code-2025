using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Day5 {
	struct Seat : IObjectParser<string> {

		private static Func<char, bool> RowConverter = (char c) => {
			if (c == 'F') return true;
			else if (c == 'B') return false;
			else throw new ArgumentException("Invalid row input.", "c");
		};

		private static Func<char, bool> ColumnConverter = (char c) => {
			if (c == 'L') return true;
			else if (c == 'R') return false;
			else throw new ArgumentException("Invalid column input.", "c");
		};

		public int Row;
		public int Column;
		private bool IsValid;
		public int ID { get => Row * 8 + Column; }
		public bool Filled { get => IsValid; }

		public void Parse(string input) {
			if (input.Length != 10) throw new ArgumentException("Invalid number of input.", "input");
			string rowInput = input.Substring(0, 7);
			string colInput = input.Substring(7, 3);
			Row = BSP(rowInput, 0, RowConverter, 0, 127);
			Column = BSP(colInput, 0, ColumnConverter, 0, 7);
			IsValid = true;
		}

		private static int BSP(string input, int index, Func<char, bool> converter, int min, int max) {
			//Base case
			if (min == max) return min;

			//Error base case
			if (index < 0 || index >= input.Length) throw new IndexOutOfRangeException("Could not determine location.");

			//Search
			int mid = (min + max) / 2;
			bool search = converter(input[index]);
			if (search) {
				return BSP(input, index + 1, converter, min, mid);
			} else {
				return BSP(input, index + 1, converter, mid + 1, max);
			}
		}

	}
}
