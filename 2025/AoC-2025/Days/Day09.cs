using System.Drawing;

namespace Day09
{
    
    public class Day09_Part1 : ISolution
    {
        protected static IEnumerable<Point> Parse(string input)
        {
            return input.Split()
                .Select(line =>
                {
                   int[] values = line.Split(',').Select(int.Parse).ToArray();
                   return new Point(values[0], values[1]); 
                });
        }

        protected static long GetArea(Point a, Point b)
        {
            long dx = Math.Abs(a.X - b.X) + 1;
            long dy = Math.Abs(a.Y - b.Y) + 1;
            return dx * dy;
        }
        
        public virtual object? Solve(string input)
        {
            Point[] corners = Parse(input).ToArray();
            return corners.Permutations()
                .Select(values => GetArea(values.Item1, values.Item2))
                .Max();
        }

    }

    public struct Line
    {
        public Point Start;
        public Point End;
        public bool IsVertical => Start.X == End.X;

        public Line(){}
        public Line(Point start, Point end)
        {
            this.Start = new Point(Math.Min(start.X, end.X), Math.Min(start.Y, end.Y));
            this.End = new Point(Math.Max(start.X, end.X), Math.Max(start.Y, end.Y));
        }
    }

    public class Day09_Part2 : Day09_Part1
    {   
        Point[] corners = Array.Empty<Point>();
        Line[] lines = Array.Empty<Line>();
        List<Line> verticalLines = new List<Line>();
        List<Line> horizontalLines = new List<Line>();

        public bool PointInside(Point p)
        {
            // We are doing a raycast to the right and checking
            // the number of intersections with the surrounding area
            // We assume there are no inline collisions with the poly
            int count = 0;
            foreach(var line in verticalLines)
            {
                if (p.Y > line.Start.Y
                    && p.Y < line.End.Y 
                    && p.X < line.Start.X)
                {
                    // only count if it cross
                    count++;
                }
            }
            return (count % 2) == 1;
        }

        public bool ValidEdge(Line test)
        {
            // The line must not collide with any polygon walls.
            // Assume inline collisions are impossible
            if (test.IsVertical)
            {
                foreach(var line in horizontalLines)
                {
                    if (test.Start.X > line.Start.X
                        && test.Start.X < line.End.X
                        && line.Start.Y > test.Start.Y
                        && line.Start.Y < test.End.Y)
                    {
                        // Collision!
                        return false;
                    }
                }
            } else
            {
                foreach(var line in verticalLines)
                {
                    if (test.Start.Y > line.Start.Y
                        && test.Start.Y < line.End.Y
                        && line.Start.X > test.Start.X
                        && line.Start.X < test.End.X)
                    {
                        // Collision!
                        return false;
                    }
                }
            }

            return true;
        }

        public override object? Solve(string input)
        {
            // Explode the positions by 10 to make the math easier later
            corners = Parse(input)
                .Select(p => new Point(p.X * 10, p.Y * 10))
                .ToArray();

            lines = new Line[corners.Length];
            for (int i = 1; i < corners.Length; i++)
            {
                lines[i] = new Line(corners[i - 1], corners[i]);
            }
            lines[0] = new Line(corners.Last(), corners[0]);
            verticalLines = lines.Where(line => line.IsVertical).ToList();
            horizontalLines = lines.Where(line => !line.IsVertical).ToList();

            long largestArea = 0;
            foreach(var pair in corners.Permutations())
            {
                Point upper_left = new Point(Math.Min(pair.Item1.X, pair.Item2.X), Math.Min(pair.Item1.Y, pair.Item2.Y));
                Point lower_right = new Point(Math.Max(pair.Item1.X, pair.Item2.X), Math.Max(pair.Item1.Y, pair.Item2.Y));

                // Now shrink the size by 1 to make detection easier
                // This make it so we dont have to worry about edge cases where the
                // rectangle lines are overlapping with the polygon lines
                upper_left.X++;
                upper_left.Y++;
                lower_right.X--;
                lower_right.Y--;

                Point upper_right = new Point(lower_right.X, upper_left.Y);
                Point lower_left = new Point(upper_left.X, lower_right.Y);

                // To determine if the rectange is fully inside, first check the points are all interior
                if (!PointInside(upper_left) || !PointInside(upper_right) || !PointInside(lower_left) || !PointInside(lower_right))
                    continue; // Rectangle doesnt fit

                // Next, check that none of the lines collide with the polygon
                Line left = new Line(upper_left, lower_left);
                Line right = new Line(upper_right, lower_right);
                Line top = new Line(upper_left, upper_right);
                Line bottom = new Line(lower_left, lower_right);
                if (!ValidEdge(left) || !ValidEdge(right) || !ValidEdge(top) || !ValidEdge(bottom))
                    continue; // Rectangle doesnt fit

                // If it fits, then check the size using the original points
                long area = GetArea(new Point(pair.Item1.X / 10, pair.Item1.Y / 10), 
                    new Point(pair.Item2.X / 10, pair.Item2.Y / 10)
                );
                largestArea = Math.Max(largestArea, area);
            }

            return largestArea;
        }

    }

}