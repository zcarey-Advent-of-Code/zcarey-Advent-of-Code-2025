using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Day06
{
    public class Worksheet
    {
        // The starting X range for each problem in the worksheet
        readonly Range[] ProblemIndexer;

        // The raw text of the worksheet (numbers only)
        readonly string[] Numbers;

        // Size of the workseet (numbers only)
        readonly int Width;
        readonly int Height;

        // The operator for each problem
        char[] Operators;

        public int NumberOfProblems => Operators.Length;

        public Worksheet(string input)
        {
            string[] lines = input.Split("\n");
            this.Height = lines.Length - 1;
            this.Width = lines[0].Length;
            Numbers = lines.Take(this.Height).ToArray();
            this.Operators = Regex.Matches(lines.Last(), @"[\*\+]")
                .Select(match => match.Value[0])
                .ToArray();
            this.ProblemIndexer = new Range[NumberOfProblems];

            // Find the location/size of each problem
            string ops = lines.Last();
            int lastEndIndex = -2;
            for (int problem = 0; problem < NumberOfProblems; problem++)
            {
                int start = lastEndIndex + 2;
                // Find the next operator (or end of string)
                int x;
                for (x = start + 1; x < Width; x++)
                {
                    if (ops[x] != ' ')
                        break;
                }

                Range current;
                if (x == Width)
                {
                    current = new(start, x - 1);
                } else
                {
                    current = new(start, x - 2);
                }
                ProblemIndexer[problem] = current;
                lastEndIndex = current.End.Value;
            }
        }

        public IEnumerable<ulong> GetHorizontalNumbers(int problem)
        {
            Range range = ProblemIndexer[problem];

            for (int y = 0; y < Height; y++)
            {
                yield return ulong.Parse(Numbers[y][range.Start.Value..(range.End.Value + 1)]);
            }
        }

        public IEnumerable<ulong> GetVerticalNumbers(int problem)
        {
            Range range = ProblemIndexer[problem];

            for (int x = range.Start.Value; x <= range.End.Value; x++)
            {
                ulong value = 0;
                for (int y = 0; y < Height; y++)
                {
                    char c = Numbers[y][x];
                    if (c == ' ')
                        continue;
                    value = value * 10 + (ulong)(c - '0');    
                }
                yield return value;
            }
        }

        public char GetOperator(int column)
        {
            return Operators[column];
        }
    }

    public class Day06_Part1 : ISolution
    {
        protected virtual IEnumerable<ulong> GetNumbers(Worksheet worksheet, int problem)
        {
            return worksheet.GetHorizontalNumbers(problem);
        }

        public object? Solve(string input)
        {
            Worksheet worksheet = new(input);

            ulong grand_total = 0;
            for (int problem = 0; problem < worksheet.NumberOfProblems; problem++)
            {
                Func<ulong, ulong, ulong> operation;
                if (worksheet.GetOperator(problem) == '*')
                    operation = (a, b) => a * b;
                else if (worksheet.GetOperator(problem) == '+')
                    operation = (a, b) => a + b;
                else
                    throw new Exception("Invalid math operator!");

                ulong answer = this.GetNumbers(worksheet, problem).Aggregate(operation);
                grand_total += answer;
            }

            return grand_total;
        }
    }

    public class Day06_Part2 : Day06_Part1
    {
        protected override IEnumerable<ulong> GetNumbers(Worksheet worksheet, int problem)
        {
            return worksheet.GetVerticalNumbers(problem);
        }
    }
}