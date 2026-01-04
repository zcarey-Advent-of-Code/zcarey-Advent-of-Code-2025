using System.Diagnostics;
using System.Windows.Input;
using zcarey_Advent_of_Code_2023;

static void Benchmark(Func<string, object> func, string input, int day, int part)
{
    Console.WriteLine($"Running Day {day} part {part}:");
    Console.WriteLine("Running...");
    Stopwatch timer = new Stopwatch();
    timer.Start();
    object result = func(input);
    timer.Stop();
    TimeSpan delta = new TimeSpan(timer.Elapsed.Ticks);
    Console.WriteLine($"Execution time: {delta.Hours}:{delta.Minutes} h:m {delta.Seconds}:{delta.Milliseconds}::{delta.Nanoseconds} s:ms::ns");
    Console.WriteLine("Answer: " + result.ToString());
}

Type[] DayTypes = new Type[25];
var types = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(s => s.GetTypes())
    .Where(p => typeof(AdventOfCodeProblem).IsAssignableFrom(p))
    .Where(t => t.GetConstructor(Type.EmptyTypes) != null);

foreach (var type in types)
{
    if (type.Name.StartsWith("Day"))
    {
        int day;
        if (int.TryParse(type.Name.Substring(3), out day))
        {
            DayTypes[day] = type;
        }
    }
}

int? parseDayArg(string arg)
{
    if (arg == "" || arg == "l" || arg == "L")
    {
        // Get the latest day and run both parts
        for (int i = DayTypes.Length - 1; i >= 0; i--)
        {
            if (DayTypes[i] != null)
            {
                return i;
            }
        }
        return null;
    } else
    {
        int result;
        if (int.TryParse(arg, out result) && result >= 0 && result < DayTypes.Length && DayTypes[result] != null)
        {
            return result;
        }
        else
        {
            return null;
        }
    }
}

int? parsePartArg(string arg)
{
    int part;
    if (int.TryParse(arg, out part) && part >= 1 && part <= 2)
    {
        return part;
    } else
    {
        return null;
    }
}

while (true)
{
    Console.WriteLine("Enter the day (1-25) to run followed by the part (1-2). If no part is entered, both parts will be ran.");
    string input = Console.ReadLine() ?? "";
    string[] arg = input.Split(' ');

    int day;
    int part = 3;
    string file = null;
    if (arg.Length < 1)
    {
        Console.WriteLine("Invalid args");
        continue;
    }

    // First arg is always the day arg
    int? temp = parseDayArg(arg[0]);
    if (temp == null)
    {
        Console.WriteLine("Invalid args");
        continue;
    }
    day = (int)temp;

    if (arg.Length == 2)
    {
        // Second arg could be a number for the part, or a text file to load
        temp = parsePartArg(arg[1]);
        if (temp == null)
        {
            // arg 1 MUST be file
            file = arg[1];
        } else
        {
            // arg 1 is the part
            part = (int)temp;
        }

    } else if (arg.Length >= 3)
    {
        // Second arg MUST be the part, third MUST be the text file
        temp = parsePartArg(arg[1]);
        if (temp == null)
        {
            Console.WriteLine("Invalid args");
            continue;
        }
        part = (int)temp;

        file = arg[2];
    }

    // Check for valid code
    if (DayTypes[day] == null)
    {
        Console.WriteLine("No code for that day was found!");
        continue;
    }

    // Attempt to create a new object from the class
    AdventOfCodeProblem? problem = (AdventOfCodeProblem)Activator.CreateInstance(DayTypes[day]);
    if (problem == null)
    {
        Console.WriteLine("Error occured instantiating type!");
        continue;
    }

    // Check for default file name
    if (file == null)
    {
        file = $"Day{day:00}.txt";
    }

    // Load input data
    string inputText;
    try
    {
        inputText = File.ReadAllText($"input/{file}");
    }
    catch (Exception e)
    {
        Console.WriteLine("Failed to load input data! " + e.Message);
        continue;
    }

    if ((part & 1) > 0)
    {
        Benchmark(problem.Part1, inputText, day, 1);
        Console.WriteLine();
        Console.WriteLine();
    }
    if ((part & 2) > 0)
    {
        Benchmark(problem.Part2, inputText, day, 2);
        Console.WriteLine();
        Console.WriteLine();
    }

    break;
}
