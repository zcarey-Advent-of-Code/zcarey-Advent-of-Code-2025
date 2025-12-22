namespace Day03
{
    public class Day03_Part1 : ISolution
    {
        protected virtual IEnumerable<ulong> GetJoltages(string bank)
        {
            for (int i = 0; i < bank.Length - 1; i++)
            {
                for (int j = i + 1; j < bank.Length; j++)
                {
                    ulong joltage = (ulong)(bank[i] - '0') * 10ul + (ulong)(bank[j] - '0');
                    yield return joltage;
                }
            }
        }

        protected IEnumerable<IEnumerable<ulong>> ParseBanks(string input)
        {
            foreach (string line in input.Split())
            {
                yield return GetJoltages(line);
            }
        }

        public virtual object? Solve(string input)
        {
            return ParseBanks(input)
                .Select(x => x.Max())
                .Sum();
        }
    }

    public class Day03_Part2 : Day03_Part1
    {
        private const int NumBatteries = 12;
        private ulong GetJoltage(string bank, int depth, int start, ulong n)
        {
            if (depth == NumBatteries)
                return n;

            // Find the largest digits (which would create the largest number)
            int largestDigit = 0;
            for (int i = start; i < bank.Length - (NumBatteries - depth - 1); i++)
            {
                int digit = bank[i] - '0';
                if (digit > largestDigit)
                    largestDigit = digit;
            }

            // Now try and find the largest number by trying all of the largest digits
            // i.e. there may be multiple '9' digits to try and get the largest
            ulong maxValue = 0;
            for (int i = start; i < bank.Length - (NumBatteries - depth - 1); i++)
            {
                int digit = bank[i] - '0';
                if (digit != largestDigit)
                    continue; // Skip numbers that wont create a larger joltage

                ulong value = GetJoltage(bank, depth + 1, i + 1, n * 10 + (ulong)digit);
                if (value > maxValue)
                    maxValue = value;
            }

            return maxValue;
        }

        protected override IEnumerable<ulong> GetJoltages(string bank)
        {
            yield return GetJoltage(bank, 0, 0, 0);
        }
    }
}