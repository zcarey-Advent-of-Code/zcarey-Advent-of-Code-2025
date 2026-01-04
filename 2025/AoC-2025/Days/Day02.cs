namespace Day02
{
    public abstract class Day02 : ISolution
    {
        public abstract object? Solve(string input);

        protected IEnumerable<(ulong Start, ulong End)> parseInput(string input)
        {
            foreach(string line in input.Split(','))
            {
                string[] vals = line.Split('-');
                yield return  (ulong.Parse(vals[0]), ulong.Parse(vals[1]));
            }
        }

        protected bool isRepeatedString(string str, int stride)
        {
            for (int i = 0; i < stride; i++)
            {
                char c = str[i];
                for (int j = i + stride; j < str.Length; j += stride)
                {
                    if (str[j] != c)
                        return false;
                }
            }
            return true;
        }
    }

    public class Day02_Part1 : Day02
    {
        protected virtual IEnumerable<ulong> findInvalidIDs(ulong start, ulong end)
        {
            for (ulong i = start; i <= end; i++)
            {
                string str = i.ToString();

                // Apparently only twice repeated are invalid
                // Other configurations are for part 2
                if (str.Length % 2 != 0)
                    continue;
                if (isRepeatedString(str, str.Length / 2))
                    yield return i;
            }
        }

        public override object? Solve(string input)
        {
            ulong total = 0;
            IEnumerable<(ulong Start, ulong End)> ranges = parseInput(input);
            foreach(var range in ranges)
            {
                foreach(ulong invalid in findInvalidIDs(range.Start, range.End))
                {
                    total += invalid;
                }
            }

            return total;
        }
    }

    public class Day02_Part2 : Day02_Part1
    {
        protected override IEnumerable<ulong> findInvalidIDs(ulong start, ulong end)
        {
            for (ulong i = start; i <= end; i++)
            {
                string str = i.ToString();
                int mid = str.Length / 2;
                for (int stride = 1; stride <= mid; stride++)
                {
                    if (str.Length % stride != 0)
                        continue;
                    
                    if (isRepeatedString(str, stride))
                    {
                        yield return i;
                        break;
                    }
                }
            }
        }

    }
}