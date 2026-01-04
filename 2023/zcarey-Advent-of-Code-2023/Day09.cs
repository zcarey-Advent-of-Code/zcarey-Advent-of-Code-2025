using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zcarey_Advent_of_Code_2023
{
    internal class Day09 : AdventOfCodeProblem
    {
        public object Part1(string input)
        {
            long sum = 0;
            foreach(string line in input.GetLines())
            {
                sum += predictNextNumber(line.Split().Select(long.Parse));
            }

            return sum;
        }

        public object Part2(string input)
        {
            long sum = 0;
            foreach(string line in input.GetLines())
            {
                sum += predictNextNumber(line.Split().Select(long.Parse).Reverse());
            }

            return sum;
        }

        long predictNextNumber(IEnumerable<long> values)
        {
            long[] oasis = values.ToArray();

            int n = 0;
            bool nonZero = true;
            while (nonZero)
            {
                nonZero = false;
                for (int i = 0; i < oasis.Length - n - 1; i++)
                {
                    oasis[i] = oasis[i + 1] - oasis[i];
                    nonZero |= (oasis[i] != 0);
                }
                n++;
            }

            // Now calculate what the next value should be
            for (int i = oasis.Length - n + 1; i < oasis.Length; i++)
            {
                oasis[i] += oasis[i - 1];
            }

            return oasis[oasis.Length - 1];
        }
    }
}
