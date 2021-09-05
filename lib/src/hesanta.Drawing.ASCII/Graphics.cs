using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace hesanta.Drawing.ASCII
{
    public class Graphics : IGraphics<string>
    {
        public string Output { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public SortedList<int, Color> Colors { get; private set; } = new SortedList<int, Color>();

        public Graphics(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            Clear();
        }

        public RectangleF DrawLine(Pen pen, PointF p1, PointF p2)
        {
            if (p1.Y == p2.Y)
            {
                return DrawHorizontalLine(pen, p1, (int)(p2.X - p1.X));
            }
            else if (p1.X == p2.X)
            {
                return DrawVerticalLine(pen, p1, (int)(p2.Y - p1.Y));
            }
            else if (Math.Abs(p1.X - p2.X) == Math.Abs(p1.Y - p2.Y))
            {
                var right = p1.X < p2.X;
                var down = p1.Y < p2.Y;

                var xDif = p2.X - p1.X;
                var yDif = p2.Y - p1.Y;
                return DrawDiagonalLine(pen, p1, (int)Math.Sqrt((xDif * xDif) + (yDif * yDif)) * (down ? 1 : -1), down ? right : !right);
            }
            else
            {
                throw new InvalidOperationException("Only Horizontal, Vertical or Diagonal lines are supported");
            }
        }

        private RectangleF DrawHorizontalLine(Pen pen, PointF position, int length)
        {
            var solidBrush = GetSolidBrush(pen.Brush);
            var color = solidBrush.Color;
            var direction = length > 0 ? 1 : -1;
            for (int i = 0; i < Math.Abs(length); i++)
            {
                float x = position.X + i * direction;
                Write(x, position.Y, "─", color);
            }

            return new RectangleF(position, new SizeF(length, 1));
        }

        private RectangleF DrawVerticalLine(Pen pen, PointF position, int length)
        {
            var solidBrush = GetSolidBrush(pen.Brush);
            var color = solidBrush.Color;
            var direction = length > 0 ? 1 : -1;
            for (int j = 0; j < Math.Abs(length); j++)
            {
                float y = position.Y + j * direction;
                Write(position.X, y, "│", color);
            }

            return new RectangleF(position, new SizeF(1, length));
        }

        private RectangleF DrawDiagonalLine(Pen pen, PointF position, int length, bool right)
        {
            var solidBrush = GetSolidBrush(pen.Brush);
            var color = solidBrush.Color;
            var direction = length > 0 ? 1 : -1;
            float calculatedLength = Math.Abs(length * 0.75f);
            for (int i = 0; i < calculatedLength; i++)
            {
                Write(position.X + i * direction * (right ? 1 : -1), position.Y + i * direction, right ? "\\" : "/", color);
            }

            return new RectangleF(position, new SizeF(calculatedLength, calculatedLength));
        }

        public RectangleF DrawPoligon(Pen pen, bool closed, params PointF[] points)
        {
            var boundsList = new List<RectangleF>();
            for (int i = 0; i < points.Length; i++)
            {
                var p1 = points[i];
                if (i == points.Length - 1)
                {
                    if (closed)
                    {
                        var firstBounds = DrawLine(pen, p1, points[0]);
                        boundsList.Add(firstBounds);
                    }
                    break;
                }

                var p2 = points[i + 1];

                var bounds = DrawLine(pen, p1, p2);
                boundsList.Add(bounds);
            }

            var x = Math.Ceiling(boundsList.Min(x => x.X));
            var y = Math.Ceiling(boundsList.Min(x => x.Y));
            var width = Math.Ceiling(Math.Abs(boundsList.Max(x => x.X) - x)) + 1;
            var height = Math.Ceiling(Math.Abs(boundsList.Max(x => x.Y) - y)) + 1;

            return new RectangleF(new PointF((float)x, (float)y), new SizeF((float)width, (float)height));
        }

        public RectangleF DrawRectangle(Pen pen, PointF position, float width, float height)
        {
            var solidBrush = GetSolidBrush(pen.Brush);
            var color = solidBrush.Color;

            var intX = (int)position.X;
            var intY = (int)position.Y;
            var w = (int)Math.Ceiling(intX + width);
            var h = (int)Math.Ceiling(intY + height);
            for (int j = (int)position.Y; j < h; j++)
                for (int i = (int)position.X; i < w; i++)
                {
                    {
                        string character = null;
                        if (i == intX && j == intY)
                        {
                            character = pen.Width == 1 ? "┌" : "╔";
                        }
                        else if (i > intX && i < w - 1 && (j == intY || j == h - 1))
                        {
                            character = pen.Width == 1 ? "─" : "═";
                        }
                        else if (i == w - 1 && j == intY)
                        {
                            character = pen.Width == 1 ? "┐" : "╗";
                        }
                        else if ((i == intX || i == w - 1) && j < h - 1)
                        {
                            character = pen.Width == 1 ? "│" : "║";
                        }
                        else if (i == intX && j == h - 1)
                        {
                            character = pen.Width == 1 ? "└" : "╚";
                        }
                        else if (i == w - 1 && j == h - 1)
                        {
                            character = pen.Width == 1 ? "┘" : "╝";
                        }

                        if (character != null)
                        {
                            Write(i, j, character, color);
                        }
                    }
                }

            return new RectangleF(position, new SizeF(width, height));
        }

        public RectangleF DrawString(string s, Brush brush, PointF position)
        {
            var solidBrush = GetSolidBrush(brush);

            var tokens = s.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < tokens.Length; i++)
            {
                var token = tokens[i];
                Write(position.X, position.Y + i, token, solidBrush.Color);
            }

            return new RectangleF(position, new SizeF(tokens.Max(x => x.Length), tokens.Length));
        }

        private void Write(float x, float y, string s, Color color)
        {
            int intX = (int)x;
            int intY = (int)y;
            if (intX < 0 || intY < 0 || intX > Width || intY >= Height)
            {
                return;
            }

            var position = (intX + intY * Width) + (intX + intY * Width) / Width;

            Output = Output.Remove(position, s.Length);
            Output = Output.Insert(position, s);

            AddColor(position, s.Length, color);
        }

        private void AddColor(int position, int stringLength, Color color)
        {
            Color? previousColor = null;
            int? previousPosition = null;

            var positions = Colors.Select(x => x.Key);
            if (positions.Any())
            {
                previousPosition = positions.Aggregate((value, next) => position < next ? value : next);
                previousColor = Colors[previousPosition.Value];
            }

            if (color != previousColor)
            {
                if (!Colors.ContainsKey(position))
                {
                    Colors.Add(position, color);
                }
                else if (previousColor != null && color != (Color)(previousColor))
                {
                    Colors.Remove(position);
                    var localLastColor = Colors.Cast<KeyValuePair<int, Color>?>().LastOrDefault();
                    if (localLastColor?.Value != color)
                    {
                        Colors.Add(position, color);
                    }
                }

                var newPosition = position + stringLength;
                if (previousColor != null && !Colors.ContainsKey(newPosition))
                {
                    Colors.Add(newPosition, (Color)(previousColor));
                }
            }
        }

        private static SolidBrush GetSolidBrush(Brush brush)
        {
            var solidBrush = brush as SolidBrush;
            if (solidBrush == null)
            {
                throw new InvalidOperationException($"Only a SolidBrush type Brush is valid: {brush}");
            }

            return solidBrush;
        }

        public void Clear()
        {
            Colors.Clear();
            Output = new string(' ', Height * Width);

            for (int i = 1; i < Height; i++)
            {
                int numberOfBreakLines = (i - 1);
                Output = Output.Insert(Width * i + numberOfBreakLines, "\n");
            }
        }
    }
}