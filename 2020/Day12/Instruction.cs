using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Day12 {
	struct Instruction : IObjectParser<string> {

		public char Operation;
		public int Units;

		//public bool IsMoveCommand { get => getIsMoveCommand(); }

		public void Parse(string input) {
			this.Operation = input[0];
			this.Units = int.Parse(input.Substring(1));
		}

		//Only used in part 1
		public Size Update(ref int angle) {
			switch (Operation) {
				case 'N': return new Size(0, -Units);
				case 'S': return new Size(0, Units);
				case 'E': return new Size(Units, 0);
				case 'W': return new Size(-Units, 0);
				case 'F': return new Size(Units * (int)Math.Cos(angle * Math.PI / 180.0), Units * -(int)Math.Sin(angle * Math.PI / 180.0));
				case 'L':
					angle = (angle + Units) % 360;
					return new Size();
				case 'R':
					angle = (angle - Units) % 360;
					return new Size();
				default:
					throw new Exception("Invalid Operation.");
			}
		}
		/*
		private bool getIsMoveCommand() {
			switch (Operation) {
				case 'N':
				case 'S':
				case 'E':
				case 'W':
				case 'F':
					return true;
				default:
					return false;
			}
		}*/

	}
}
