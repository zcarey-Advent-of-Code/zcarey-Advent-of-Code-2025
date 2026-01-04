namespace Day05
{
    public struct IdRange
    {
        public ulong Start;
        public ulong End;

        public static (IEnumerable<IdRange> Ranges, IEnumerable<ulong> IDs) Parse(string input)
        {
            string[] blocks = input.Split("\n\n");
            IEnumerable<IdRange> ranges = blocks[0]
                .Split()
                .Select(line => {
                    string[] str = line.Split("-");
                    IdRange range = new();
                    range.Start = ulong.Parse(str[0]);
                    range.End = ulong.Parse(str[1]);
                    return range;
                });
            IEnumerable<ulong> ids = blocks[1]
                .Split()
                .Select(ulong.Parse);
            return (ranges, ids);
        }

        public bool Contains(ulong value)
        {
            return (value >= Start) && (value <= End);
        }
    }

    public class CombinedRange
    {
        List<IdRange> PartialRanges = new(); 

        public CombinedRange(IEnumerable<IdRange> ranges)
        {
            List<IdRange> arr = ranges.ToList();
            arr.Sort((a, b) => a.Start.CompareTo(b.Start));

            IdRange last = arr[0];
            foreach (IdRange current in arr.Skip(1))
            {
                // If current interval overlaps with the last merged
                // interval, merge them 
                if (current.Start <= last.End)
                {
                    last.End = Math.Max(last.End, current.End);
                } else
                {
                    PartialRanges.Add(last);
                    last = current;
                }
            }
            PartialRanges.Add(last);
        }

        public bool Contains(ulong value)
        {
            int i;
            for (i = 0; i < PartialRanges.Count; i++)
            {
                if (value < PartialRanges[i].Start)
                    break;
            }
            i--;
            if (i < 0)
                return false;
            return PartialRanges[i].Contains(value);
        }

        public IEnumerable<IdRange> GetCombinedRanges()
        {
            return PartialRanges;
        }

        public void Print()
        {
            foreach(var range in PartialRanges)
            {
                Console.WriteLine($"{range.Start}-{range.End}");
            }
        }
    }

    public class Day05_Part1 : ISolution
    {
        public object? Solve(string input)
        {
            (var Ranges, var IDs) = IdRange.Parse(input);
            CombinedRange range = new CombinedRange(Ranges);
            //range.Print();

            return IDs.Count(range.Contains);
        }
    }

    public class Day05_Part2 : ISolution
    {
        public object? Solve(string input)
        {
            (var Ranges, var IDs) = IdRange.Parse(input);
            CombinedRange range = new CombinedRange(Ranges);

            return range.GetCombinedRanges()
                .Select(range => range.End - range.Start + 1)
                .Sum();
        }
    }

}