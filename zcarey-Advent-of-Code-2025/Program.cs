using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

// Selects the latest day and part available
const string Test_Latest = "latest";

string? TestSelection = Test_Latest;
string? TestInput = null;


// Internal Variables
Regex InputFileRegex = new Regex(@"^Day(\d\d)_([^_\.]+)\.txt$", RegexOptions.Compiled);
Regex SolutionRegex = new Regex(@"^Day(\d\d)_Part(\d+)$", RegexOptions.Compiled);

// Find all days using reflection
IEnumerable<Type> solutionTypes = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(type =>
        typeof(ISolution).IsAssignableFrom(type)
        && type.IsClass
        && !type.IsAbstract);

List<SolutionInfo> solutions = new();
foreach(var type in solutionTypes)
{
    SolutionInfo info = new();
    info.Type = type;

    var match = SolutionRegex.Match(type.Name);
    Debug.Assert(match.Success, "Regex failed on type name.");
    
    info.Day = byte.Parse(match.Groups[1].Value);
    info.Part = byte.Parse(match.Groups[2].Value);
    Debug.Assert(info.Day >= 1 && info.Day <= 31, "Invalid day number");
    Debug.Assert(info.Part >= 1 && info.Part <= 99, "Invalid part number.");

    solutions.Add(info);
}


// Verify day/part numbers are unique
Debug.Assert(solutions.DistinctBy(day => $"{day.Day}|{day.Part}").Count() == solutions.Count);


// Select which day to run
List<byte> days = solutions.Select(x => x.Day).Distinct().Order().ToList();
byte selectedDay;
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
        Console.WriteLine($"\tDay {day}");
    }
    if (!byte.TryParse(Console.ReadLine(), out selectedDay))
        throw new Exception("Invalid day input.");
    if (!days.Contains(selectedDay))
        throw new Exception("Invalid day input.");
}
Console.WriteLine($"Day {selectedDay} selected.");


// Select which part to run
List<byte> parts = solutions
    .Where(x => x.Day == selectedDay)
    .Select(x => x.Part)
    .Order()
    .ToList();
byte selectedPart;
if (TestSelection != null)
{
    if (TestSelection == Test_Latest)
    {
        selectedPart = parts.Last();
    } else
    {
        throw new Exception("Invalid part selection.");
    }
} else
{
    // Ask the user
    Console.WriteLine("Which part to run?");
    foreach (var part in parts)
    {
        Console.WriteLine($"\tPart {part}");
    }
    if (!byte.TryParse(Console.ReadLine(), out selectedPart))
        throw new Exception("Invalid part input.");
    if (!parts.Contains(selectedPart))
        throw new Exception("Invalid part input.");
}
Console.WriteLine($"Part {selectedPart} selected.");


// Get the relavent solution info
var selectedSolution = solutions.Where(x => x.Day == selectedDay && x.Part == selectedPart).First();


// Find all input files
List<InputFileInfo> inputFiles = new();
foreach(string path in Directory.GetFiles("inputs"))
{
    string filename = Path.GetFileName(path);
    var match = InputFileRegex.Match(filename);
    Debug.Assert(match.Success, "Failed to parse input file name.");

    InputFileInfo info = new();
    info.Day = byte.Parse(match.Groups[1].Value);
    info.Name = match.Groups[2].Value;
    info.Path = path;

    if (info.Day == selectedDay)
    {
        inputFiles.Add(info);
    }
}
Debug.Assert(inputFiles.Count > 0, "Failed to find any input files.");


// Determine which input to use
// If only one available, don't ask
InputFileInfo selectedInput;
if (TestInput != null)
{
    selectedInput = inputFiles.First(x => x.Name == TestInput);
} else if (inputFiles.Count == 1)
{
    selectedInput = inputFiles[0];
} else
{
    // Ask the user
    Console.WriteLine("Which input file?");
    for (int i = 0; i < inputFiles.Count; i++)
    {
        Console.WriteLine($"\t{i + 1}: {inputFiles[i].Name}");
    }
    int selectedIndex;
    if (!int.TryParse(Console.ReadLine(), out selectedIndex) || selectedIndex < 1 || selectedIndex > inputFiles.Count)
        throw new Exception("Invalid input file selection.");
    selectedInput = inputFiles[selectedIndex - 1];    
}

// Run program using selected parameters
ISolution program = (ISolution)Activator.CreateInstance(selectedSolution.Type);
Debug.Assert(program != null, "Failed to create solution object.");
string input = File.ReadAllText(selectedInput.Path);
object? result = program.Solve(input);
Console.WriteLine("Result:");
Console.WriteLine(result);

// Struct to hold info about the days
struct SolutionInfo
{
    public Type Type;
    public byte Day;
    public byte Part;
}

// Struct to hold info about the input files
struct InputFileInfo
{
    public byte Day;
    public string Name;
    public string Path;
}
