//#define PRINT_DEBUG

using System.Drawing;

namespace Day04
{
    public class PaperMap
    {
        char[][] Map;
        int[][] Minesweeper; // Number of surrounding papers at each location

        public readonly int Width;
        public readonly int Height;

        public PaperMap(string input)
        {
            string[] lines = input.Split();
            this.Width = lines[0].Length;
            this.Height = lines.Length;

            this.Map = new char[Height][];
            this.Minesweeper = new int[Height][];
            for (int y = 0; y < Height; y++)
            {
                Map[y] = lines[y].ToCharArray();
                Minesweeper[y] = new int[Width];
            }

            // Fill in the initial minesweeper map
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Minesweeper[y][x] = GetSurroundingPapers(new Point(x, y)).Count();
                }
            }
        }

        public bool this[int x, int y]
        {
            get => Map[y][x] == '@';
            private set => Map[y][x] = (value ? '@' : '.');
        }

        public bool this[Point p]
        {
            get => Map[p.Y][p.X] == '@';
            private set => Map[p.Y][p.X] = (value ? '@' : '.');
        }

        public IEnumerable<Point> GetSurroundingPapers(Point p)
        {
            return  GetSurroundingPoints(p)
                .Where(point => this[point]);
        }

        public IEnumerable<Point> GetSurroundingPoints(Point p)
        {
            for (int dy = p.Y - 1; dy <= p.Y + 1; dy++)
            {
                for (int dx = p.X - 1; dx <= p.X + 1; dx++)
                {
                    if (dy < 0 || dy >= Height || dx < 0 || dx >= Width || (dx == p.X && dy == p.Y))
                        continue;
                    yield return new Point(dx, dy);
                }
            }
        }

        public int GetPaperCount(Point p)
        {
            return this.Minesweeper[p.Y][p.X];
        }

        public void RemovePaper(Point p)
        {
            this[p] = false;

            // Update surrounding numbers
            foreach(Point loc in GetSurroundingPoints(p))
            {
                this.Minesweeper[loc.Y][loc.X]--;
            }
        }

        public void Print(HashSet<Point>? debug = null)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Point p = new Point(x, y);
                    if (debug != null && debug.Contains(p))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    } else {
                        Console.ForegroundColor = this[p] ? ConsoleColor.Red : ConsoleColor.White;
                    }
                    Console.Write(Minesweeper[p.Y][p.X]);
                }
                Console.WriteLine();
            }
        }
    }

    public class Day04_Part1 : ISolution
    {
        public object? Solve(string input)
        {
            PaperMap map = new(input);
            int count = 0;
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    Point p = new Point(x, y);
                    if (map[p] && map.GetPaperCount(p) < 4)
                        count++;
                }
            }
            return count;
        }
    }

    public class Day04_Part2 : ISolution
    {
        public object? Solve(string input)
        {
            int totalRemoved = 0;
            PaperMap map = new(input);
            HashSet<Point> delete = new();

            #if PRINT_DEBUG
                map.Print();
                Console.WriteLine();
            #endif

            do
            {
                // To get started, find every paper that can be deleted and mark it
                delete.Clear();
                for (int y = 0; y < map.Height; y++)
                {
                    for (int x = 0; x < map.Width; x++)
                    {
                        Point p = new Point(x, y);
                        if (map[p] && map.GetPaperCount(p) < 4)
                            delete.Add(p);
                    }
                }

                #if PRINT_DEBUG
                    Console.WriteLine("Marked for deletion:");
                    map.Print(delete);
                    Console.WriteLine();
                #endif

                // Delete all papers what have been marked
                foreach (Point paper in delete)
                {
                    // Delete this paper (and update numbers)
                    map.RemovePaper(paper);
                    totalRemoved++;
                }

                #if PRINT_DEBUG
                    Console.WriteLine("After deletion:");
                    map.Print(delete);
                    Console.WriteLine();
                #endif
            } while (delete.Count > 0);

            return totalRemoved;
        }
    }
}