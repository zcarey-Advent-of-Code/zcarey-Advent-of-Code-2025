using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AdventOfCode.Visualization {
	public partial class Visualizer : Form {

		// TODO each character color should be customizable

		/// <summary>
		/// The number of pixels for each side of a character
		/// </summary>
		public int DPI { get; set; } = 30;

		/// <summary>
		/// Width of the image in characters
		/// </summary>
		public new int Width { get; private set; } = 0;

		/// <summary>
		/// Height of the image in characters
		/// </summary>
		public new int Height { get; private set; } = 0;

		/// <summary>
		/// Offset of the Origin in characters.
		/// Set OffsetX = 1 will cause the coordinate of 1 to be drawn at the origin.
		/// </summary>
		public int OffsetX { get; set; } = 0;

		/// <summary>
		/// Offset of the Origin in characters.
		/// Set OffsetY = 1 will cause the coordinate of 1 to be drawn at the origin.
		/// </summary>
		public int OffsetY { get; set; } = 0;

		/// <summary>
		/// Flip X coordinate, so OriginX (default 0) starts on the right side of the image.
		/// </summary>
		public bool FlipX { get; set; } = false;

		/// <summary>
		/// Flip Y coordinate, so OriginY (default 0) starts on the bottom side of the image.
		/// </summary>
		public bool FlipY { get; set; } = false;

		public char DefaultCharacter = ' ';

		private char[,] characters = null;

		public Visualizer(string title) {
			InitializeComponent();
			this.Text = title;
		}

		public void InitializeImage(int width, int height) {
			if(width <= 0 || height <= 0) {
				throw new Exception("Invalid size.");
			}
			this.Width = width;
			this.Height = height;
			characters = new char[width, height];
		}

		private Point PointToArrayIndex(int x, int y) {
			x -= OffsetX;
			y -= OffsetY;
			if (FlipX) {
				x = Width - x - 1;
			}
			if (FlipY) {
				y = Height - y - 1;
			}

			return new Point(x, y);
		}

		private bool IndexInsideArrayBounds(Point index) {
			return index.X >= 0 && index.X < Width
				&& index.Y >= 0 && index.Y < Height;
		}

		public char this[int x, int y] {
			get {
				return this[new Point(x, y)];
			}

			set {
				this[new Point(x, y)] = value;
			}
		}

		public char this[Point point] {
			get {
				Point index = PointToArrayIndex(point.X, point.Y);
				if (IndexInsideArrayBounds(index)) {
					return characters[index.X, index.Y];
				} else {
					return DefaultCharacter;
				}
			}

			set {
				Point index = PointToArrayIndex(point.X, point.Y);
				if (IndexInsideArrayBounds(index)) {
					characters[index.X, index.Y] = value;
				}
			}
		}

		private void Visualizer_Load(object sender, EventArgs e) {
			if(characters == null) {
				throw new NullReferenceException("Image was not initialized.");
			}

			Bitmap image = new Bitmap(Width * DPI, Height * DPI);
			Graphics g = Graphics.FromImage(image);

			// Background
			Brush BackgroundBrush = new SolidBrush(Color.Black);
			g.FillRectangle(BackgroundBrush, new Rectangle(0, 0, image.Width, image.Height));

			// Draw characters
			StringFormat format = new StringFormat();
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;

			Brush FontBrush = new SolidBrush(Color.White);
			string fontName = "Consolas";
			Font font = new Font(fontName, FindFontSize(fontName, DPI));
			for(int y = 0; y < Height; y++) {
				for(int x = 0; x < Width; x++) {
					char c = characters[x, y];
					RectangleF area = new RectangleF(
						x * DPI,
						y * DPI,
						DPI,
						DPI
					);
					g.DrawString(c.ToString(), font, FontBrush, area, format);
				}
			}

			this.PictureBox.Image = image;
		}

		public void Run() {
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			//Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(this);
		}

		private static int FindFontSize(string FontName, int CharacterSize) {
			char ASCII_Start = ' ';
			char ASCII_End = '~';

			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			sf.LineAlignment = StringAlignment.Center;
			FontStyle Style = FontStyle.Regular;
			
			try {
				Bitmap map = new Bitmap(CharacterSize, CharacterSize);
				using (Graphics g = Graphics.FromImage(map)) {
					// each char must fit into this size:
					SizeF szF = map.Size;

					// fallback font and size
					int fntSize = 8;
					Font fnt = new Font(FontName, fntSize, Style);

					// figure out the largest font size that will fit into "szF" above:
					bool smaller = true;
					while (smaller) {
						Font newFnt = new Font(FontName, fntSize, Style);
						for (char c = ASCII_Start; c <= ASCII_End; c++) {
							SizeF charSzF = g.MeasureString(c.ToString(), newFnt);
							if (charSzF.Width > szF.Width || charSzF.Height > szF.Height) {
								smaller = false;
								break;
							}
						}
						if (smaller) {
							if (fnt != null) {
								fnt.Dispose();
							}
							fnt = newFnt;
							fntSize++;
						}
					}

					return fntSize;
				}
			} catch (Exception) {
				return 8;
			}
		}

	}
}
