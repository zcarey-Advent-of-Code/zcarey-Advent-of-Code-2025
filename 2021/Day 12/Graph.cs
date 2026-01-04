using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_12 {
    public class Graph : IObjectParser<IEnumerable<Tuple<string, string>>> {

        public List<Cave> Nodes = new();

        public Cave StartNode = new(false, "start");
        public Cave EndNode = new(false, "end");

        public void Parse(IEnumerable<Tuple<string, string>> input) {
            Dictionary<string, Cave> nodes = new() {
                { "start", StartNode },
                { "end", EndNode }
            };
            
            foreach(var connection in input) {
                if (!nodes.ContainsKey(connection.Item1)) {
                    nodes[connection.Item1] = new Cave(char.IsUpper(connection.Item1[0]), connection.Item1);
                }
                if (!nodes.ContainsKey(connection.Item2)) {
                    nodes[connection.Item2] = new Cave(char.IsUpper(connection.Item2[0]), connection.Item2);
                }

                Cave cave1 = nodes[connection.Item1];
                Cave cave2 = nodes[connection.Item2];

                cave1.Connections.Add(cave2);
                cave2.Connections.Add(cave1);
            }

            Nodes = nodes.Values.ToList();
        }

        public void ResetTraversed() {
            foreach(Cave node in Nodes) {
                node.Traversed = false;
            }
        }
    }
}
