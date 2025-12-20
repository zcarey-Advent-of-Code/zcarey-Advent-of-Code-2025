using System.Diagnostics;
using System.Reflection;

// Selects the latest day and part available
const string Test_Latest = "latest";

string? TestSelection = Test_Latest;
string? TestInput = null;

// Find all days using reflection
IEnumerable<Type> dayTypes = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(type =>
        typeof(IDaySolution).IsAssignableFrom(type)
        && type.IsClass
        && !type.IsAbstract);

List<DayInfo> days = new();
foreach(var type in dayTypes)
{
    DayInfo info = new();
    info.Type = type;

    byte? numberField = (byte?)type
        .GetProperty("Day", BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty)
        ?.GetValue(null);
    Debug.Assert(numberField != null, "Failed to find day number.");
    info.Number = numberField.Value;

    days.Add(info);
}


// Verify day numbers are unique
Debug.Assert(days.DistinctBy(day => day.Number).Count() == days.Count);


// Sort the days by their number to make the next step simpler
days.Sort((a, b) => b.Number.CompareTo(a.Number));


// Select which day to run
DayInfo selectedDay;
if (TestSelection != null)
{
    if (TestSelection == Test_Latest)
    {
        selectedDay = days.Last();
    } else
    {
        throw new Exception("Invalid test input");
    }
} else
{
    // Ask the user
    Console.WriteLine("Which day to run?");
    foreach(var day in days)
    {
        Console.WriteLine($"\tDay {day.Number}");
    }
    int selectedDayIndex;
    if (!int.TryParse(Console.ReadLine(), out selectedDayIndex))
        throw new Exception("Invalid day input.");
    selectedDayIndex = days.FindIndex(info => info.Number == selectedDayIndex);
    if (selectedDayIndex < -1)
        throw new Exception("Invalid day input.");
    selectedDay = days[selectedDayIndex];
}
Console.WriteLine($"Day {selectedDay.Number} selected.");


// Select which part to run
List<Type> parts = (List<Type>)selectedDay.Type
    .GetProperty("Parts", BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty)
    ?.GetValue(null);
Debug.Assert(parts != null && parts.Count > 0, "Failed to get parts from day.");

int selectedPartIndex;
if (TestSelection != null)
{
    if (TestSelection == Test_Latest)
    {
        selectedPartIndex = parts.Count - 1;
    } else
    {
        throw new Exception("Invalid part selection.");
    }
} else
{
    // Ask the user
    Console.WriteLine("Which part to run?");
    for (int i = 0; i < parts.Count; i++)
    {
        Console.WriteLine($"\tPart {i + 1}");
    }
    if (!int.TryParse(Console.ReadLine(), out selectedPartIndex))
        throw new Exception("Invalid part input.");
    selectedPartIndex--;
}
Debug.Assert(selectedPartIndex >= 0 && selectedPartIndex < parts.Count, "Invalid part input.");
Console.WriteLine($"Part {selectedPartIndex + 1} selected.");


// Determine which input to use
// If only one available, don't ask
List<string> inputFiles = (List<string>)selectedDay.Type
    .GetProperty("InputFiles", BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty)
    ?.GetValue(null);
Debug.Assert(inputFiles != null && inputFiles.Count > 0, "Failed to get input files from day.");

string selectedInputPath;
if (TestInput != null)
{
    Debug.Assert(inputFiles.Contains(TestInput), "Invalid test input file.");
    selectedInputPath = TestInput;
} else if (inputFiles.Count == 1)
{
    selectedInputPath = inputFiles[0];
} else
{
    // Ask the user
    Console.WriteLine("Which input file?");
    for (int i = 0; i < inputFiles.Count; i++)
    {
        Console.WriteLine($"\t{i + 1}: {inputFiles[i]}");
    }
    int selectedIndex;
    if (!int.TryParse(Console.ReadLine(), out selectedIndex) || selectedIndex < 1 || selectedIndex > inputFiles.Count)
        throw new Exception("Invalid input file selection.");
    selectedInputPath = inputFiles[selectedIndex - 1];    
}

// Adjust file name for selected day
selectedInputPath = $"Day{selectedDay.Number:00}_{selectedInputPath}.txt";
Console.WriteLine($"Selected input file: {selectedInputPath}");

// Adjust file path for the correct folder
selectedInputPath = Path.Combine("inputs", selectedInputPath);


// Run program using selected parameters
IPartSolution program = (IPartSolution)Activator.CreateInstance(parts[selectedPartIndex]);
Debug.Assert(program != null, "Failed to create solution object.");
string input = File.ReadAllText(selectedInputPath);
object? result = program.Solve(input);
Console.WriteLine("Result:");
Console.WriteLine(result);

// Struct to hold info about the days
struct DayInfo
{
    public Type Type;
    public byte Number;
}
