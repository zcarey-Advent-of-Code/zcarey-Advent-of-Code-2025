using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Day8 {
	struct Instruction : IObjectParser<string> {

		public Operation Op;
		public int Arg;

		/// <summary>
		/// Parse the instruction from a line of input
		/// </summary>
		/// <param name="input"></param>
		public void Parse(string input) {
			string op = input.Substring(0, 3);
			string arg = input.Substring(4);

			this.Op = ParseOp(op);
			if (!int.TryParse(arg, out this.Arg) && !string.IsNullOrWhiteSpace(arg)) {
				throw new ArgumentException("Unable to parse argument.", "input");
			}
		}

		private static Operation ParseOp(string input) {
			switch (input) {
				case "acc": return Operation.Acc;
				case "jmp": return Operation.Jmp;
				case "nop": return Operation.Nop;
				default:
					throw new ArgumentException("Unable to parse operation.", "input");
			}
		}

		internal void Execute(ref int programCounter, ref int accumulator) {
			switch (this.Op) {
				case Operation.Acc:
					accumulator += Arg;
					programCounter++;
					break;
				case Operation.Jmp:
					programCounter += Arg;
					break;
				case Operation.Nop:
					programCounter++;
					break;
				default:
					throw new Exception("Unable to execute instruction.");
			}
		}
	}
}
