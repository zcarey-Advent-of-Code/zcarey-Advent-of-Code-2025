using System.Collections;
using System.Diagnostics;

namespace Day08
{
    public struct Vector3
    {
        public long X;
        public long Y;
        public long Z;

        public Vector3()
        {}

        public Vector3(long x, long y, long z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public long SquareDistaneTo(Vector3 other)
        {
            long dx = other.X - this.X;
            long dy = other.Y - this.Y;
            long dz = other.Z - this.Z;
            return (dx * dx) + (dy * dy) + (dz * dz);
        }
    }

    public class JunctionBox
    {
        public List<JunctionBox> Circuit = new();
        public readonly Vector3 Position;

        public JunctionBox(Vector3 pos)
        {
            this.Position = pos;
            this.Circuit.Add(this);
        }
    }

    public struct JunctionBoxPair
    {
        public JunctionBox Box1;
        public JunctionBox Box2;
        public long SquareDistance;

        public JunctionBoxPair(JunctionBox box1, JunctionBox box2, long dist_square)
        {
            this.Box1 = box1;
            this.Box2 = box2;
            this.SquareDistance = dist_square;
        }
    }

    public class Day08_Part1 : ISolution
    {
        //private const int NumConnections = 10; // Example
        private const int NumConnections = 1000; // Actual

        protected static IEnumerable<JunctionBox> Parse(string input)
        {
            return input.Split()
                .Select(line => {
                    long[] values = line.Split(',')
                        .Select(long.Parse)
                        .ToArray();
                    return new JunctionBox(new Vector3(values[0], values[1], values[2]));
            });
        }

        protected static List<JunctionBoxPair> SmallestDistances(List<JunctionBox> boxes, int count)
        {
            SortedList<long, JunctionBoxPair> shortest = new();
            for (int i = 0; i < boxes.Count - 1; i++)
            {
                for (int j = i + 1; j < boxes.Count; j++)
                {
                    JunctionBox box1 = boxes[i];
                    JunctionBox box2 = boxes[j];
                    long distance = box1.Position.SquareDistaneTo(box2.Position);

                    if (shortest.Count < count)
                    {
                        // Keep adding any pair until the list is full
                        JunctionBoxPair pair = new JunctionBoxPair(box1, box2, distance);
                        shortest.Add(distance, pair);
                        continue;
                    }

                    // List is full, only add if it is shorter distance
                    if (distance < shortest.Last().Value.SquareDistance)
                    {
                        JunctionBoxPair pair = new JunctionBoxPair(box1, box2, distance);
                        shortest.Remove(shortest.Last().Key);
                        shortest.Add(distance, pair);
                    }
                }
            }

            return shortest.Values.ToList();
        }

        protected void MakeConnection(JunctionBox box1, JunctionBox box2)
        {
            if (box1.Circuit == box2.Circuit)
                return;

            // Update all item in box2 to be on the same circuit as box1
            List<JunctionBox> new_circuit = box1.Circuit;
            List<JunctionBox> old_circuit = box2.Circuit;
            foreach(var box in old_circuit)
            {
                box.Circuit = new_circuit;
                new_circuit.Add(box);
            }
            // old_circuit will be garbage collected later
        }

        protected void MakeConnection(JunctionBoxPair pair)
        {
            MakeConnection(pair.Box1, pair.Box2);
        }

        protected IEnumerable<List<JunctionBox>> GetAllCircuits(List<JunctionBox> boxes)
        {
            return boxes.Select(x => x.Circuit).Distinct();
        }

        public virtual object? Solve(string input)
        {
            List<JunctionBox> boxes = Parse(input).ToList();
            List<JunctionBoxPair> shortestPairs = SmallestDistances(boxes, NumConnections);
            foreach(var pair in shortestPairs)
            {
                MakeConnection(pair);
            }

            IEnumerable<List<JunctionBox>> circuits = GetAllCircuits(boxes)
                .OrderByDescending(x => x.Count);
            
            return circuits.Take(3)
                .Select(circuit => circuit.Count)
                .Aggregate((a, b) => a * b);
        }
        
    }

    public class Day08_Part2 : Day08_Part1
    {
        public override object? Solve(string input)
        {
            List<JunctionBox> boxes = Parse(input).ToList();
            int num_connections = boxes.Count * (boxes.Count - 1) / 2;
            List<JunctionBoxPair> shortestPairs = SmallestDistances(boxes, num_connections);
            JunctionBoxPair? finalConnection = null;
            foreach(var pair in shortestPairs)
            {
                MakeConnection(pair);
                if (pair.Box1.Circuit.Count == boxes.Count)
                {
                    finalConnection = pair;
                    break;
                }
            }

            Debug.Assert(finalConnection != null);
            
            return finalConnection.Value.Box1.Position.X * finalConnection.Value.Box2.Position.X;
        }
    }
}