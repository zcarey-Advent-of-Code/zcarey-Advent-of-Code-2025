using AdventOfCode.Parsing;
using AdventOfCode.Visualization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AdventOfCode {
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T">The type of input data to be parsed.</typeparam>
	public abstract class ProgramStructure<T> {

		// Command line args
		private bool arg_runPart1 = false;
		private bool arg_runPart2 = false;
		private bool arg_visualize = false;

		// Run time timer
		private Stopwatch timer = new Stopwatch();

		// Input parser
		private IParser<StreamReader, T> parser;

		protected ProgramStructure(IParser<StreamReader, T> parser) {
			this.parser = parser;
		}

		abstract protected object SolvePart1(T input, Visualizer vision);

		abstract protected object SolvePart2(T input, Visualizer vision);

		public void Run(string[] args, string filename = "Input.txt") {
			parseArgs(args);

			Console.WriteLine("Reading input...");
			Console.WriteLine();
			Console.WriteLine();
			T input = default(T);

			// Load the file into memory to prevent "closed file" exceptions later
			MemoryStream file = new MemoryStream();
			try {
				using (FileStream stream = File.OpenRead(filename)) {
					stream.CopyTo(file);
					stream.Flush();
				}
			} catch (Exception) {
				Console.WriteLine("Unable to read input file.");
				return;
			}

			Visualizer visualizer = null;

			using (file) {

				if (!arg_runPart1) {
					Console.WriteLine("Skipped part 1.");
				} else {
					if (arg_visualize) {
						visualizer = new Visualizer("Part 1");
					}

					Console.WriteLine("Parsing input...");
					Console.WriteLine();
					Console.WriteLine();
					input = LoadInputData(file);

					Console.WriteLine("Running part 1:");
					timer.Restart();
					object result1 = SolvePart1(input, visualizer);
					timer.Stop();
					Console.WriteLine("Finished in {0:g}.", timer.Elapsed);
					input = default(T);

					if (result1 == null) {
						Console.WriteLine("Part 1 failed to return a result.");
					} else if (result1 is Exception e) {
						Console.WriteLine("Part 1 returned with error: \"" + e.Message + "\"");
					} else {
						Console.WriteLine("Part 1 finished.");
						Console.WriteLine("Solution: " + result1.ToString());
						File.WriteAllText("Solution - Part 1.txt", result1.ToString());
						visualizer?.Run();
					}

					Console.WriteLine();
					Console.WriteLine();
				}

				if (!arg_runPart2) {
					Console.WriteLine("Skipped part 2.");
				} else {
					if (arg_visualize) {
						visualizer = new Visualizer("Part 2");
					}

					Console.WriteLine("Parsing input...");
					Console.WriteLine();
					Console.WriteLine();
					input = LoadInputData(file);

					Console.WriteLine("Running part 2:");
					timer.Restart();
					object result2 = SolvePart2(input, visualizer);
					timer.Stop();
					Console.WriteLine("Finished in {0:g}.", timer.Elapsed);
					input = default(T);

					if (result2 == null) {
						Console.WriteLine("Part 2 failed to return a result.");
					} else if (result2 is Exception e) {
						Console.WriteLine("Part 2 returned with error: \"" + e.Message + "\"");
					} else {
						Console.WriteLine("Part 2 finished.");
						Console.WriteLine("Solution: " + result2.ToString());
						File.WriteAllText("Solution - Part 2.txt", result2.ToString());
						visualizer?.Run();
					}

					Console.WriteLine();
					Console.WriteLine();
				}

				//Console.Write("Press any key to exit.");
				//Console.ReadKey();
			}
		}

		private void parseArgs(string[] args) {
			foreach (string arg in args) {
				if (arg == "--part1") {
					arg_runPart1 = true;
				} else if (arg == "--part2") {
					arg_runPart2 = true;
				} else if(arg == "--visualize") {
					arg_visualize = true;
				} else {
					throw new ArgumentException("Unknown argument.", arg);
				}
			}

			if (!arg_runPart1 && !arg_runPart2) {
				arg_runPart1 = true;
				arg_runPart2 = true;
			}
		}

		private T LoadInputData(MemoryStream stream) {
			stream.Position = 0;
			return parser.Parse(new StreamReader(stream));
		}
	}

	/*public abstract class ProgramStructure : ProgramStructure<string> {
		
		protected ProgramStructure() : base(new StringParser()) {

		}

	}*/
}
