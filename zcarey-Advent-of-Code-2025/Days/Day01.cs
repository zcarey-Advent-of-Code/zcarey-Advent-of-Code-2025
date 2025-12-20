
public class Day01 : IDaySolution
{
    public static byte Day => 1;
    public static List<string> InputFiles => new()
    {
        "Input", "Example"
    };

    public static List<Type> Parts => new()
    {
        typeof(Day01_Part1), typeof(Day01_Part2)
    };
}

struct SafeCombo
{
    public char Direction;
    public int Count;

    public static IEnumerable<SafeCombo> Parse(string input)
    {
        foreach (string line in input.Split())
        {
            SafeCombo result = new();
            result.Direction = line[0];
            result.Count = int.Parse(line[1..]);
            yield return result;
        }
    }
}

public class Day01_Part1 : IPartSolution
{
    public object? Solve(string input)
    {
        int dial = 50;
        long zeroCount = 0;
        foreach(var combo in SafeCombo.Parse(input))
        {
            if (combo.Direction == 'L')
            {
                dial -= combo.Count;
            } else if (combo.Direction == 'R')
            {
                dial += combo.Count;
            }
            dial = dial % 100;

            if (dial == 0)
                zeroCount++;
        }
        return zeroCount;
    }
}

public class Day01_Part2 : IPartSolution
{
    public object? Solve(string input)
    {
        int dial = 50;
        long zeroCount = 0;
        foreach(var combo in SafeCombo.Parse(input))
        {
            zeroCount += combo.Count / 100;
            int partial_rotation = combo.Count % 100;
            if (combo.Direction == 'R')
            {
                dial += partial_rotation;
                if (dial > 99)
                {
                    zeroCount++;
                    dial -= 100;
                }
            } else if (combo.Direction == 'L')
            {
                if (dial != 0 && partial_rotation >= dial)
                {
                    zeroCount++;
                }
                dial -= partial_rotation;
                if (dial < 0)
                {
                    dial += 100;
                }
            }
        }
        return zeroCount;
    }
}