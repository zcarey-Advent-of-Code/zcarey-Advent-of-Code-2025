using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zcarey_Advent_of_Code_2023
{
    internal class Day08 : AdventOfCodeProblem
    {
        public object Part1(string input)
        {
            (var turns, var nodes) = ParseInput(input);
            string currentNode = "AAA";
            int count = 0;
            IEnumerator<Direction> nextTurn = turns.GetEnumerator();
            while (currentNode != "ZZZ")
            {
                nextTurn.MoveNext();
                FollowPath(nodes, ref currentNode, nextTurn.Current);

                count++;
            }

            return count;
        }

        public object Part2(string input)
        {
            // There is some AoC nonsense going on here, the inputs form loops
            // so we just have to find the LCM of the loops.
            (var turns, var nodes) = ParseInput(input);

            List<string> inputs = new();
            foreach(var node in nodes.Keys)
            {
                if (node.EndsWith('A'))
                {
                    inputs.Add(node);
                }
            }

            List<long> loopLengths = new();
            foreach(string start in inputs)
            {
                // Follow path until we find the end node
                string currentNode = start;
                IEnumerator<Direction> directions = turns.GetEnumerator();
                while (directions.MoveNext())
                {
                    FollowPath(nodes, ref currentNode, directions.Current);
                    if (currentNode.EndsWith('Z'))
                    {
                        break;
                    }
                }

                // Now find the loop length
                long count = 0;
                while(directions.MoveNext())
                {
                    FollowPath(nodes, ref currentNode, directions.Current);
                    count++;
                    if (currentNode.EndsWith('Z'))
                    {
                        break;
                    }
                }

                // Add to list of LCM
                loopLengths.Add(count);
            }

            return Utils.LCM(loopLengths);
        }

        void FollowPath(Dictionary<string, Map> map, ref string node, Direction direction)
        {
            if (direction == Direction.Left)
            {
                node = map[node].Left;
            }
            else
            {
                node = map[node].Right;
            }
        }

        enum Direction
        {
            Left,
            Right
        }

        struct Map
        {
            public string Left;
            public string Right;

            public Map(string left, string right)
            {
                Left = left;
                Right = right;
            }
        }

        (IEnumerable<Direction> turns, Dictionary<string, Map> nodes) ParseInput(string input)
        {
            Direction[] turns = input.GetLines().First().Select(c => (c == 'L') ? Direction.Left : Direction.Right).ToArray();
            Dictionary<string, Map> nodes = new();

            foreach(string line in input.GetLines().Skip(2))
            {
                string key = line.Substring(0, 3);
                string left = line.Substring(7, 3);
                string right = line.Substring(12, 3);

                nodes.Add(key, new Map(left, right));
            }

            return (turns.Repeat(), nodes);
        }

    }
}
