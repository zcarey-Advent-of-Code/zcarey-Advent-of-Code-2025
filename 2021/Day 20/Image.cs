using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_20 {
    public class Image : IObjectParser<bool[][]>, IEnumerable<bool> {

        public int Width { get; private set; }
        public int Height { get; private set; }
        public readonly bool DefaultPixel;

        private bool[,] pixels;

        private const int MaxIndex = 0b111111111;

        public Image() {
            DefaultPixel = false;
        }

        private Image(int width, int height, bool defaultPixel) {
            this.Width = width;
            this.Height = height;
            this.pixels = new bool[width, height];
            this.DefaultPixel = defaultPixel;
        }

        private Image(Bounds bounds, Dictionary<Point, bool> image, bool defaultPixel) {
            this.Width = bounds.Width;
            this.Height = bounds.Height;
            this.pixels = new bool[bounds.Width, bounds.Height];
            this.DefaultPixel = defaultPixel;
            int dx = -bounds.Left;
            int dy = -bounds.Top;
            // We are probably adding unecessary processing by converting to an array but it seems to be fast enough
            for (int y = bounds.Top; y <= bounds.Bottom; y++) {
                for (int x = bounds.Left; x <= bounds.Right; x++) {
                    Point point = new(x, y);
                    bool pixel;
                    if(!image.TryGetValue(point, out pixel)) {
                        pixel = defaultPixel;
                    }
                    this.pixels[x + dx, y + dy] = pixel;
                }
            }
        }

        public void Parse(bool[][] input) {
            Width = input[0].Length;
            Height = input.Length;
            pixels = new bool[Width, Height];
            for(int y = 0; y < Height; y++) {
                for (int x = 0; x < Width; x++) {
                    pixels[x, y] = input[y][x];
                }
            }
        }

        public bool this[int x, int y] {
            get {
                if (x < 0 || x >= Width || y < 0 || y >= Height) {
                    return DefaultPixel;
                } else {
                    return pixels[x, y];
                }
            }
        }

        public Image Enhance(bool[] algorithm) {
            HashSet<Point> visited = new();
            Dictionary<Point, bool> image = new();

            for(int y = 0; y < Height; y++) {
                for (int x = 0; x < Width; x++) {
                    Point point = new(x, y);
                    int index = GetAlgorithmIndex(x, y);
                    image[point] = algorithm[index];
                    visited.Add(point);
                }
            }

            HashSet<Point> queue = new();
            Bounds imageSize = new(0, 0);
            imageSize.Add(Width - 1, Height - 1);

            for(int x = -1; x <= Width; x++) {
                queue.Add(new Point(x, -1));
                queue.Add(new Point(x, Height));
            }
            for(int y = 0; y < Height; y++) {
                queue.Add(new Point(-1, y));
                queue.Add(new Point(Width, y));
            }

            while(queue.Count > 0) {
                Point point = queue.First();
                queue.Remove(point);
                int index = GetAlgorithmIndex(point.X, point.Y);

                if ((DefaultPixel == false && index == 0) || (DefaultPixel == true && index == MaxIndex)) {
                    // Just let this be taken care of by the default pixels
                    continue;
                } else {
                    image[point] = algorithm[index];
                    visited.Add(point);
                    imageSize.Add(point);
                    // Add surrounding pixels to the queue
                    queue.AddRange(GetKernel(point.X, point.Y).Where(x => x != point && !visited.Contains(x)));
                }
            }

            bool newDefaultPixel = (this.DefaultPixel ? algorithm[MaxIndex] : algorithm[0]);
            return new Image(imageSize, image, newDefaultPixel); 
        }

        private int GetAlgorithmIndex(int x, int y) {
            int index = 0;
            foreach(bool bit in GetKernelPixels(x, y)) {
                index = (index << 1) | (bit ? 1 : 0);
            }
            return index;
        }

        private IEnumerable<bool> GetKernelPixels(int X, int Y) {
            return GetKernel(X, Y).Select(p => this[p.X, p.Y]);
        }

        private IEnumerable<Point> GetKernel(int X, int Y) {
            for(int y = Y - 1; y <= Y + 1; y++) {
                for (int x = X - 1; x <= X + 1; x++) {
                    yield return new Point(x, y);
                }
            }
        }

        public IEnumerator<bool> GetEnumerator() {
            return GetAllPixels().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetAllPixels().GetEnumerator();
        }

        private IEnumerable<bool> GetAllPixels() {
            for(int y = 0; y < Height; y++) {
                for (int x = 0; x < Width; x++) {
                    yield return pixels[x, y];
                }
            }
        }

        public override string ToString() {
            StringBuilder sb = new();
            for(int y = 0; y < Height; y++) {
                if (y != 0) {
                    sb.AppendLine();
                }
                for(int x = 0; x < Width; x++) {
                    sb.Append(pixels[x, y] ? '#' : '.');
                } 
            }

            return sb.ToString();
        }
    }
}
