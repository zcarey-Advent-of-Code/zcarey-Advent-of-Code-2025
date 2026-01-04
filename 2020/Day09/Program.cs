using AdventOfCode;
using AdventOfCode.Parsing;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day09 {
	class Program : ProgramStructure<long[]> {

		private const int PREAMBLE_LEN = 25;
		//private const int PREAMBLE_LEN = 5;

		Program() : base(new Parser()
			.Filter(new LineReader())
			.Filter(long.Parse)
			.ToArray()
		) {
		}

		static void Main(string[] args) {
			new Program().Run(args);
			//new Program().Run("Example.txt");
		}

		private static long calcPart1(long[] input) {
			List<long> preamble = new List<long>();
			for (int i = 0; i < PREAMBLE_LEN; i++) {
				preamble.Add(input[i]);
			}
			foreach (long data in input.Skip(PREAMBLE_LEN)) {
				if (!ValidXMas(preamble, data)) {
					return data;
				} else {
					preamble.RemoveAt(0);
					preamble.Add(data);
				}
			}
			throw new Exception("Could not compute.");
		}

		protected override object SolvePart1(long[] input) {
			return calcPart1(input).ToString();
		}

		static bool ValidXMas(List<long> preamble, long data) {
			for(int i = 0; i < preamble.Count - 1; i++) {
				for(int j = i + 1; j < preamble.Count; j++) {
					if(preamble[i] + preamble[j] == data) {
						return true;
					}
				}
			}
			return false;
		}

		protected override object SolvePart2(long[] input) {
			long target = calcPart1(input);
			for (int i = 0; i < input.Length - 1; i++) {
				for(int j = i + 1; j < input.Length; j++) {
					long result = SumRange(input, i, j);
					if(result == target) {
						return AddMinMax(input, i, j).ToString();
					}else if(result > target) {
						break;
					}
				}
			}
			throw new Exception("Could not compute.");
		}

		static long SumRange(long[] input, int i, int j) {
			long count = 0;
			for(; i <= j; i++) {
				count += input[i];
			}
			return count;
		}

		static long AddMinMax(long[] input, int i, int j) {
			IEnumerable<long> data = input.Skip(i).Take(j - i);
			return data.Min() + data.Max();
		}
	}
}
