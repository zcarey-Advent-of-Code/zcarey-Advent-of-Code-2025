using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zcarey_Advent_of_Code_2023
{
    internal class Day03 : AdventOfCodeProblem
    {
        public object Part1(string input)
        {
            Graph graph = ParseInput(input);
            return graph.Nodes.Where(x => x.Edges.Count > 0) // Find all numbers next to a symbol
                .Select(x => (long)x.Value) // Get the number
                .Sum(); // add all the numbers together
        }

        public object Part2(string input)
        { 
            Graph graph = ParseInput(input);
            return graph.Edges.Where(x => x.Value == '*') // Get all gear symbols (*)
                .Where(x => x.Nodes.Count == 2) // Gears can only have 2 numbers attatched
                .Select(x => (long)x.Nodes[0].Value * (long)x.Nodes[1].Value) // Calculate the gear ratio
                .Sum(); // Add all gear ratios together
        }

        private class Node
        {
            public readonly int ID;
            public int Value;
            public List<Edge> Edges = new();
            public Node(int ID, int value)
            {
                this.ID = ID;
                Value = value;
            }

            public void Connect(Edge edge)
            {
                this.Edges.Add(edge);
                edge.Nodes.Add(this);
            }
        }

        private class Edge
        {
            public readonly int ID;
            public readonly char Value;
            public List<Node> Nodes = new();
            public Edge(int ID, char value)
            {
                this.ID = ID;
                Value = value;
            }

            public void Connect(Node node)
            {
                this.Nodes.Add(node);
                node.Edges.Add(this);
            }
        }

        private class Graph
        {
            public List<Node> Nodes = new();
            public List<Edge> Edges = new();

            public Node CreateNode(int value)
            {
                Node node = new Node(Nodes.Count * 2, value);
                Nodes.Add(node);
                return node;
            }

            public Edge CreateEdge(char value)
            {
                Edge edge = new Edge(Edges.Count * 2 + 1, value);
                Edges.Add(edge);
                return edge;
            }

            public Node GetNode(int id)
            {
                if (id % 2 != 0) throw new Exception("Invalid node ID");
                return Nodes[id / 2];
            }

            public Edge GetEdge(int id)
            {
                if (id % 2 != 1) throw new Exception("Invalid edge ID");
                return Edges[(id - 1) / 2];
            }

            public bool IdIsNode(int id)
            {
                return id % 2 == 0;
            }
        }

        private static Graph ParseInput(string input)
        {
            Graph graph = new Graph();
            List<List<int>> map = new List<List<int>>(); // Our self-expanding map to find connections

            int y = -1; // Current height in map
            foreach(string line in input.GetLines())
            {
                y++;
                List<int> currentMap = new List<int>();
                map.Add(currentMap);

                for (int x = 0; x < line.Length; x++)
                {
                    char c = line[x];
                    if (c == '.')
                    {
                        currentMap.Add(-1); // Just a nothing-burger
                    }
                    else if (char.IsDigit(c))
                    {
                        int startX = x;
                        // Read the number
                        Node node = graph.CreateNode(0);
                        while (x < line.Length && char.IsDigit(c = line[x]))
                        {
                            node.Value *= 10;
                            node.Value += (c - '0');
                            currentMap.Add(node.ID);
                            x++;
                        }
                        // x is now 1 past the number (possible out of bounds), move back one so when the loop continues it will check this char
                        x--;
                    }
                    else
                    {
                        // Must be a symbol
                        Edge edge = graph.CreateEdge(line[x]);
                        currentMap.Add(edge.ID);   
                    }
                }
            }

            // Now parse graph looking for connections
            for(y = 0; y < map.Count; y++)
            {
                List<int> currentLine = map[y];
                for (int x = 0; x < currentLine.Count; x++)
                {
                    if (currentLine[x] >= 0 && !graph.IdIsNode(currentLine[x]))
                    {
                        // Found a symbol, get all connections
                        Edge edge = graph.GetEdge(currentLine[x]);
                        HashSet<int> IDs = new();

                        for (int dy = y - 1; dy <= y + 1; dy++)
                        {
                            if (dy < 0 || dy >= map.Count) continue;
                            List<int> temp = map[dy];
                            for (int dx = x - 1; dx <= x + 1; dx++)
                            {
                                if (dx < 0 || dx >= temp.Count) continue;
                                if (temp[dx] >= 0 && graph.IdIsNode(temp[dx]))
                                {
                                    IDs.Add(temp[dx]);
                                }
                            }
                        }

                        foreach(var id in IDs)
                        {
                            edge.Connect(graph.GetNode(id));
                        }
                    }
                }
            }

            return graph;
        }

    }
}
