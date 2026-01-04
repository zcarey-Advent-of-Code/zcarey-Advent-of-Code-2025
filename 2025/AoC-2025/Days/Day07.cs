using System.Drawing;

namespace Day07
{

    public class BeamMap
    {
        public Point Start;
        public int[][] Splitters; 

        public int Width;
        public int Height;

        public BeamMap(string input)
        {
            string[] lines = input.Split();
            Width = lines[0].Length;
            Height = lines.Length;
            Splitters = new int[Height][];
            for (int y = 0; y < lines.Length; y++)
            {
                string line = lines[y];
                List<int> splitters = new();
                for (int x = 0; x < line.Length; x++)
                {
                    if (line[x] == 'S')
                    {
                        this.Start = new Point(x, y);
                    } else if (line[x] == '^')
                    {
                        splitters.Add(x);
                    }
                }
                this.Splitters[y] = splitters.ToArray();
            }
        }
    }

    public class Day07_Part1 : ISolution
    {
        public object? Solve(string input)
        {
            BeamMap map = new(input);
            uint splits = 0;

            HashSet<int> beams = new();
            HashSet<int> next_beams = new();
            beams.Add(map.Start.X);
            for (int y = 0; y < map.Height; y++)
            {
                if (map.Splitters[y].Length == 0)
                    continue;

                foreach(int beam in beams)
                {
                    if (map.Splitters[y].Contains(beam))
                    {
                        next_beams.Add(beam - 1);
                        next_beams.Add(beam + 1);
                        splits++;
                    } else
                    {
                        next_beams.Add(beam);
                    }
                }

                var temp = beams;
                beams = next_beams;
                next_beams = temp;
                next_beams.Clear();
            }

            return splits;
        }
    }

    public class Day07_Part2 : ISolution
    {
        public object? Solve(string input)
        {
            BeamMap map = new(input);

            ulong[] timelines = new ulong[map.Width];
            ulong[] next_timelines = new ulong[map.Width];
            Array.Fill<ulong>(timelines, 0);
            Array.Fill<ulong>(next_timelines, 0);

            timelines[map.Start.X] = 1;

            for (int y = 0; y < map.Height; y++)
            {
                if (map.Splitters[y].Length == 0)
                    continue;

                for (int x = 0; x < map.Width; x++)
                {
                    if (map.Splitters[y].Contains(x))
                    {
                        // There is a splitter at this location
                        // Add timelines to both sides of the splitter
                        next_timelines[x - 1] += timelines[x];
                        next_timelines[x + 1] += timelines[x];
                    } else
                    {
                        // There is no splitter at this location
                        // Continue any timelines as normal
                        next_timelines[x] += timelines[x];
                    }
                }

                var temp = timelines;
                timelines = next_timelines;
                next_timelines = temp;
                Array.Fill<ulong>(next_timelines, 0);
            }

            return timelines.Sum();
        }
    }
}