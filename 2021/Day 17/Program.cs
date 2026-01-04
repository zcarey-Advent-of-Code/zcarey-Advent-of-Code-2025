using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Day_17 {
    internal class Program : ProgramStructure<Rectangle> {

        Program() : base(new Parser()
            .Parse(new StringReader()) // "target area: x=257..286, y=-101..-57"
            .Parse(x => x.Substring("target area: ".Length)) // "x=257..286, y=-101..-57"
            .Filter(new SeparatedParser(", ")) // "x=257..286", "y=-101..-57"
            .Filter(x => x.Substring(2)) // "257..286", "-101..-57"
            .ForEach(
                new Parser<string>() // "257..286"
                .Filter(new SeparatedParser("..")) // "257", "287"
                .Filter(int.Parse) // 257, 287
            ) // [[257, 287], [-101, -57]]
            .Combine() // 257, 287, -101, -57
            .ToArray()
            .Parse(x => new Rectangle(
                Math.Min(x[0], x[1]),
                Math.Max(x[2], x[3]),
                Math.Abs(x[0] - x[1]),
                Math.Abs(x[2] - x[3])
            ))
        ) { }

        static void Main(string[] args) {
            //new Program().Run(args);
            new Program().Run(args, "Example.txt");
        }

        protected override object SolvePart1(Rectangle input) {
            // Let t = number of steps (discrete steps)
            // For now, assume positive x only
            // velX(t) = -t + V0x + 1, where t <= V0x
            // X(t) = -0.5t^2 + V0xt + 1/2t where t<= V0x
            // The step that has the furthest x distance is t=V0x + 1
            // Therefore, the furthest distance is x=0.5V0x^2 + 0.5V0x
            // Using quadratic we can find
            // V0x = (-1 + sqrt(8x + 1)) / 2

            /* int minVelx = (-1 + (int)Math.Sqrt(8 * input.Left + 1)) / 2;
             int maxVelx = (-1 + (int)Math.Sqrt(8 * input.Right + 1)) / 2;

             // I tried using math but I got too angry so here is my brute force (that still uses math)
             for (int velx = minVelx; velx <= maxVelx; velx++) {
                 int t = velx + 1;
                 int x = (-t * t + 2 * velx * t + t) / 2;

                 y = -0.5t ^ 2 + V0yt + 1 / 2t
             }*/

            // I tried using math but I got too angry so here is my brute force (that still uses math)
            //int vMin = (-1 + sqrt(8 * input.Left))
            int maxY = int.MinValue;
            for(int velx = 0; velx < input.Right; velx++) {
                for(int vely = 0; vely < 500; vely++) {
                    int x = 0;
                    int y = 0;
                    int vx = velx;
                    int vy = vely;
                    int localMaxY = int.MinValue;
                    for(int t = 0; t < 10000; t++) {
                        x += vx;
                        y += vy;
                        vx = Math.Max(0, vx - 1);
                        vy--;

                        localMaxY = Math.Max(localMaxY, y);
                        if (input.Contains(new Point(x, y))) {
                            maxY = Math.Max(maxY, localMaxY);
                        }
                        if (y < input.Bottom || x > input.Right) {
                            break;
                        }
                    }
                }
            }


            return maxY;
        }

        protected override object SolvePart2(Rectangle input) {
            return null;
        }

    }
}
