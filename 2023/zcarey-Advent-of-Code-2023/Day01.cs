using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zcarey_Advent_of_Code_2023
{
    internal class Day01 : AdventOfCodeProblem
    {
        public object Part1(string input)
        {
            StringReader reader = new(input);

            long total = 0;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                int[] numbers = line.Where(char.IsDigit)
                    .Select(c => c - '0')
                    .ToArray();
                int n = numbers.First() * 10 + numbers.Last();
                total += n;
            }

            return total;
        }

        public object Part2(string input)
        {
            StringReader reader = new(input);

            long total = 0;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                int[] numbers = line.Where(char.IsDigit)
                    .Select(c => c - '0')
                    .ToArray();
                int first = numbers.First();
                int last = numbers.Last();

                // Check for additional digits
                int strIndex = line.IndexOf((char)('0' + first));
                for (int i = 0; i < strIndex; i++)
                {
                    int? n = ParseDigit(line.AsSpan(i, strIndex - i));
                    if (n != null)
                    {
                        first = (int)n;
                        break;
                    }
                }

                strIndex = line.LastIndexOf((char)('0' + last));
                for (int i = strIndex + 1; i < line.Length; i++)
                {
                    int? n = ParseDigit(line.AsSpan(i, line.Length - i));
                    if (n != null)
                    {
                        last = (int)n;
                        // Cant break early because we have to find the LAST number in the string
                        //break;
                    }
                }

                total += first * 10 + last;
            }

            return total;
        }

        string[] Digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        public int? ParseDigit(ReadOnlySpan<char> str)
        {
            if (str.Length < 3) return null;

            for (int i = 0; i < Digits.Length; i++)
            {
                if (str.Length < Digits[i].Length) continue;

                bool match = true;
                for(int j = 0; j < Digits[i].Length; j++)
                {
                    if (str[j] != Digits[i][j])
                    {
                        match = false; 
                        break;
                    }
                }

                if (match)
                {
                    return i;
                }
            }

            return null;
        }
    }
}
