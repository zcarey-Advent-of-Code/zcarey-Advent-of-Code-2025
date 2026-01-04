using AdventOfCode;
using AdventOfCode.Parsing;
using Common;
using System;

namespace Day13 {
	class Program : ProgramStructure<InputData> {

		Program() : base(new Parser()
			.Filter(new LineReader())
			.ToArray()
			.Create<InputData>()
		) { }

		static void Main(string[] args) {
			new Program().Run(args);
			//new Program().Run("Example.txt");
		}

		protected override object SolvePart1(InputData input) {
			int bestBusId = -1;
			int bestTime = int.MaxValue;
			foreach(int busId in input.BusIDs) {
				if (busId == -1) continue;
				int waitTime = busId - (input.EarliestTime % busId);
				if (waitTime < bestTime) {
					bestBusId = busId;
					bestTime = waitTime;
				}
			}

			return (bestBusId * bestTime).ToString();
		}

		protected override object SolvePart2(InputData input) {
			//printDebug(input);
			int[] data = input.BusIDs;
			long t = 1;
			long LCM = 1;
			for(int i = data.Length - 1; i >= 0; i--) {
				if (data[i] == -1) continue;
				while (!BusDepartsAt(data[i], t + i)) {
					t += LCM;
				}
				LCM = FindLCM(LCM, data[i]);
			}
			return t.ToString();
		}

		private static void printDebug(InputData input) {
			int[] data = input.BusIDs;
			Console.WriteLine("time\t\tbus 7   bus 13  bus xx  bus xx  bus 59  bus xx  bus 31  bus 19");
			for (int i = 0; i < 1000; i++) {
				Console.Write(i.ToString());
				Console.Write("\t\t");
				for(int j = 0; j < data.Length; j++) {
					if(BusDepartsAt(data[j], i)) {
						Console.Write("D       ");
					} else {
						Console.Write(".       ");
					}
				}
				Console.WriteLine();
			}
			Console.ReadLine();
		} 

		private static long FindLCM(long a, long b) {
			long num1, num2;
			if(a > b) {
				num1 = a;
				num2 = b;
			} else {
				num1 = b;
				num2 = a;
			}

			for(long i = 1; i < num2; i++) {
				long mult = num1 * i;
				if(mult % num2 == 0) {
					return mult;
				}
			}
			return num1 * num2;
		}
	
		private static bool BusDepartsAt(int bus, long t) {
			if (bus == -1) return true;
			else return (t % bus) == 0; 
		}
	
	}
}
