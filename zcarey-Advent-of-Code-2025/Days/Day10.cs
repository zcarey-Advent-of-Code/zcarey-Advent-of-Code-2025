using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Z3;

namespace Day10
{
    public struct Button
    {
        public int[] Indices;
        public uint Mask;

        public Button()
        {
            Indices = Array.Empty<int>();
            Mask = 0;
        }

        public Button(string input, int lightCount)
        {
            Debug.Assert(input[0] == '(');
            this.Indices = input[1..^1].Split(',').Select(int.Parse).ToArray();
            this.Mask = 0;
            foreach(int index in Indices)
            {
                Mask |= 1u << (lightCount - index - 1);
            }
            //Console.WriteLine($"{input} => {Convert.ToString(result, 2).PadLeft(lightCount, '0')}");
        }
    }

    public struct Manual
    {
        public uint Lights;
        public int LightCount;
        public List<Button> Buttons;
        public List<uint> Joltage;

        public static IEnumerable<Manual> Parse(string input)
        {
            return input.Split('\n')
                .Select(line =>
                {
                    Manual result = new();
                    string[] segments = line.Split(' ');

                    Debug.Assert(segments[0][0] == '[', "First segment must be the lights");
                    result.LightCount = segments[0].Length - 2;
                    result.Lights = ParseLights(segments[0][1..^1]);

                    Debug.Assert(segments[^1][0] == '{', "The last segment must be the joltages.");
                    result.Joltage = segments[^1][1..^1]
                        .Split(',')
                        .Select(uint.Parse)
                        .ToList();

                    result.Buttons = segments.Skip(1).SkipLast(1)
                        .Select(s => new Button(s, result.LightCount))
                        .ToList();

                    return result;
                });
        }

        private static uint ParseLights(string input)
        {
            uint result = 0;
            for (int i = 0; i < input.Length; i++)
            {
                result = (result << 1) | (input[i] == '#' ? 1u : 0u);
            }
            return result;
        }
    }

    public class Day10_Part1 : ISolution
    {
        ulong GetFewestButtonPresses(Manual manual)
        {
            Queue<(uint state, uint button, ulong depth)> queue = new();
            HashSet<uint> seen = new();
            foreach (uint button in manual.Buttons.Select(x => x.Mask)) {
                queue.Enqueue((0, button, 1));
            }

            // Use breadth-first-search to find a solution
            while(queue.Count > 0) {
                var bfs = queue.Dequeue();
                uint new_state = bfs.state ^ bfs.button;
                if (new_state == manual.Lights) {
                    return bfs.depth;
                }
                if (seen.Add(new_state)) 
                {
                    foreach (uint button in manual.Buttons.Select(x => x.Mask)) {
                        queue.Enqueue((new_state, button, bfs.depth + 1));
                    }
                }
            }
            throw new Exception("Could not find a solution.");
        }

        public object? Solve(string input)
        {
            return Manual.Parse(input)
                .Select(GetFewestButtonPresses)
                .Sum();
        }
    }

    public class Day10_Part2 : ISolution
    {
        // Thanks to Josh Ackland for insight into how to use the Z3 solver for this
        // https://github.com/joshackland/advent_of_code/blob/master/2025/c%23/AoC2025/day10.cs

        ulong GetMinimumButtonPresses(Manual manual)
        {
            using (var ctx = new Context())
            {
                Optimize opt = ctx.MkOptimize();

                // These lists store the arguments for each equation
                List<ArithExpr>[] joltageEquations = manual.Joltage.Select(x => new List<ArithExpr>()).ToArray();
                List<IntExpr> variables = new();

                // Go through the buttons to find the terms in each equation
                foreach((int index, Button button) in manual.Buttons.Index())
                {
                    // Create a variable for how many times this button is pressed
                    IntExpr expr = (IntExpr)ctx.MkIntConst($"x_{index}");
                    variables.Add(expr);
                    opt.Add(ctx.MkGe(expr, ctx.MkInt(0)));

                    // Add arguments to the relevant joltage equation
                    foreach(int joltageIndex in button.Indices)
                    {
                        joltageEquations[joltageIndex].Add(expr);
                    }
                }

                // Build each equation and add them to the solver
                foreach((List<ArithExpr> terms, uint joltage) in joltageEquations.Zip(manual.Joltage))
                {
                    ArithExpr lhs;
                    if (terms.Count == 0)
                        lhs = ctx.MkInt(0);
                    else if (terms.Count == 1)
                        lhs = terms[0];
                    else
                        lhs = ctx.MkAdd(terms.ToArray());

                    opt.Add(ctx.MkEq(lhs, ctx.MkInt(joltage)));
                }

                // Run the solver, solving for our unknown variables (number of button presses)
                ArithExpr totalExpr;
                if (manual.Buttons.Count == 1)
                    totalExpr = variables[0];
                else
                {
                    totalExpr = ctx.MkAdd(variables);
                }
                opt.MkMinimize(totalExpr);

                Debug.Assert(opt.Check() == Status.SATISFIABLE, "Solver failed.");
                
                Model model = opt.Model;

                // Retreive solutions for how many times each button needs to be pressed, then add them
                // to get the solution to this particular manual.
                return variables.Select(b => (ulong)((IntNum)model.Evaluate(b, true)).Int).Sum();
            }
        }

        public object? Solve(string input)
        {
            return Manual.Parse(input)
                .Select(GetMinimumButtonPresses)
                .Sum();
        }
    }
}