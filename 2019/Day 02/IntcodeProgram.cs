using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Day_02 {
	public class IntcodeProgram : IObjectParser<int[]> {

		public IntcodeProgram() {
			RegisterOpCodes();
		}

		public IntcodeProgram(IntcodeProgram copy) : this() {
			this.Program = new int[copy.Program.Length];
			Array.Copy(copy.Program, this.Program, copy.Program.Length);
		}

		public int[] Program;

		private Dictionary<int, Func<int, int?>> Opcodes = new Dictionary<int, Func<int, int?>>();

		public void Parse(int[] input) {
			this.Program = input;
		}

		public void Register(int number, Func<int, int?> opcode) {
			Opcodes[number] = opcode;
		}

		// Op codes should return null for default code stepping, or an int of what index to execute next
		// If an op code tries to jump to an invalid index (such as -1) or an unregistered opcode is encountered, whether intended or not the program will halt.
		virtual protected void RegisterOpCodes() {
			Register(1, OpCode_1);
			Register(2, OpCode_2);
			Register(99, OpCode_99);
		}

		public int this[int index] {
			get => Program[index];
			set => Program[index] = value;
		}

		public void Run(int DefaultCodeStep = 1) {
			int programIndex = 0;
			while (programIndex >= 0 && programIndex < Program.Length) {
				Func<int, int?> opcode = null;
				if(!Opcodes.TryGetValue(Program[programIndex], out opcode)) {
					break;
				} else {
					int? jump = opcode.Invoke(programIndex);
					if (jump == null) {
						programIndex += DefaultCodeStep;
					} else {
						programIndex = (int)jump;
					}
				}
			}
		}

		#region Default OpCodes
		//Add the next 2 pointers together and store them in the third pointer
		public int? OpCode_1(int index) {
			int arg1 = Program[index + 1];
			int arg2 = Program[index + 2];
			int arg3 = Program[index + 3];
			Program[arg3] = Program[arg1] + Program[arg2];
			return index + 4;
		}

		// Multiply the next 2 pointers and store them in the third pointer
		public int? OpCode_2(int index) {
			int arg1 = Program[index + 1];
			int arg2 = Program[index + 2];
			int arg3 = Program[index + 3];
			Program[arg3] = Program[arg1] * Program[arg2];
			return index + 4;
		}

		// Halt the program
		public int? OpCode_99(int index) {
			return -1;
		}
		#endregion

	}
}
