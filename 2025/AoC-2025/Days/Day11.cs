using System.Diagnostics.CodeAnalysis;

namespace Day11
{
    public class Node //: IEquatable<Node>
    {
        public string Name;
        public List<Node> Connections;

        private Node(string name)
        {
            this.Name = name;
            this.Connections = new();
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        /*public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is Node n && Equals(n);
        }

        public bool Equals(Node? other)
        {
            return this.Name == other?.Name;
        }*/

        public static Dictionary<string, Node> Parse(string input)
        {
            Dictionary<string, Node> nodes = new();
            nodes.Add("out", new Node("out"));
            foreach(string line in input.Split('\n'))
            {
                int sep = line.IndexOf(':');
                string name = line[..sep];
                nodes[name] = new Node(name);
            }
            
            foreach(string line in input.Split('\n'))
            {
                int sep = line.IndexOf(':');
                string name = line[..sep];
                string[] connectionNames = line[(sep+2)..].Split();
                Node node = nodes[name];
                node.Connections.AddRange(connectionNames.Select(x => nodes[x]));
            }

            return nodes;
        }
    }

    public class Day11_Part1 : ISolution
    {

        static ulong GetNumPaths(Node node, HashSet<Node> visited, Dictionary<Node, ulong> paths, Node target)
        {
            if (!visited.Add(node))
                return 0; // Prevent looping

            ulong numPaths = 0;
            foreach (Node neighbor in node.Connections)
            {
                if (neighbor == target)
                {
                    numPaths++;
                } else if (paths.TryGetValue(neighbor, out ulong n))
                {
                    numPaths += n;
                } else
                {
                    numPaths += GetNumPaths(neighbor, visited, paths, target);
                }
            }
            paths[node] = numPaths;
            return numPaths;
        }

        public static ulong GetNumPaths(Node start, Node target)
        {
            HashSet<Node> visited = new();
            Dictionary<Node, ulong> paths = new();
            return GetNumPaths(start, visited, paths, target);
        }

        public object? Solve(string input)
        {
            Dictionary<string, Node> nodes = Node.Parse(input);
            return GetNumPaths(nodes["you"], nodes["out"]);
        }
    }

    public class Day11_Part2 : ISolution
    {
        public object? Solve(string input)
        {
            Dictionary<string, Node> nodes = Node.Parse(input);

            ulong paths1 = Day11_Part1.GetNumPaths(nodes["svr"], nodes["dac"]);
            paths1 *= Day11_Part1.GetNumPaths(nodes["dac"], nodes["fft"]);
            paths1 *= Day11_Part1.GetNumPaths(nodes["fft"], nodes["out"]);

            ulong paths2 = Day11_Part1.GetNumPaths(nodes["svr"], nodes["fft"]);
            paths2 *= Day11_Part1.GetNumPaths(nodes["fft"], nodes["dac"]);
            paths2 *= Day11_Part1.GetNumPaths(nodes["dac"], nodes["out"]);

            return paths1 + paths2;
        }
    }
}