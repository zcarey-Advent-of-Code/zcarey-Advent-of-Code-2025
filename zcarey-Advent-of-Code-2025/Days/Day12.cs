using System.Diagnostics;

namespace Day12
{
    // Always a 3x3
    public struct Present
    {
        bool[] Shape;
        public long Area;

        public Present(bool[] shape)
        {
            Debug.Assert(shape.Length == 9);
            this.Shape = shape;
            this.Area = shape.Where(x => x).Count();
        }

        public bool this[int x, int y]
        {
            get => Shape[y*3 + x];
        }
    }

    public struct Tree
    {
        public long Width;
        public long Height;
        public long Area => Width * Height;
        public List<int> PresentCount;
    }

    public class Day12_Input
    {
        public List<Present> Presents = new();
        public List<Tree> Trees = new();

        public Day12_Input(string input)
        {
            string[] lines = input.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line[1] == ':')
                {
                    IEnumerable<bool> shape = lines.Skip(i + 1)
                        .Take(3)
                        .Select(x => x.Trim())
                        .SelectMany(x => x)
                        .Select(c => c == '#');
                    Presents.Add(new Present(shape.ToArray()));
                    i += 4;
                } else
                {
                    Tree tree = new();
                    int split = line.IndexOf(':');
                    int size = line.IndexOf('x');
                    tree.Width = int.Parse(line[..size]);
                    tree.Height = int.Parse(line[(size+1)..split]);
                    tree.PresentCount = line[(split + 2)..]
                        .Split()
                        .Select(int.Parse)
                        .ToList();
                    Trees.Add(tree);
                }
            }
        }
    }

    public class Day12_Part1 : ISolution
    {
        public object? Solve(string input_str)
        {
            var input = new Day12_Input(input_str);

            int validTrees = 0;
            foreach (Tree tree in input.Trees)
            {
                long areaOfPresents = tree.PresentCount
                    .Index()
                    .Select(x => input.Presents[x.Index].Area * x.Item)
                    .Sum();

                // Impossible to pack if presents take up more room
                // than the tree has
                if (areaOfPresents > tree.Width * tree.Height)
                    continue;

                long numPresents = tree.PresentCount.Sum();
                long unpackedSize = (tree.Width/3) * (tree.Height/3);
                if (numPresents <= unpackedSize)
                {
                    // If there is enough space for each present to
                    // get it's own 3x3 area under the tree, we dont
                    // even need to attempt packing.
                    validTrees++;
                    continue;
                }

                // Apparently interlocking not needed?
                throw new NotImplementedException("Interlock presents");
            }

            return validTrees;
        }
    }
}