using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace hesanta.Drawing.ASCII
{
    public class Graphics : IGraphics<string>
    {
        private const int scaleX = 2;

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

        public RectangleF DrawPixel(Pen pen, PointF position)
        {
            Write(position.X, position.Y, "██", pen.Color);

            return new RectangleF(position, new SizeF(1, 1));
        }

        public RectangleF DrawLine(Pen pen, PointF p1, PointF p2)
        {
            var x1 = (int)p1.X;
            var x2 = (int)p2.X;
            var y1 = (int)p1.Y;
            var y2 = (int)p2.Y;

            int x, y, dx, dy, dx1, dy1, px, py, xe, ye, i;
            dx = x2 - x1;
            dy = y2 - y1;
            dx1 = Math.Abs(dx);
            dy1 = Math.Abs(dy);
            px = 2 * dy1 - dx1;
            py = 2 * dx1 - dy1;
            if (dy1 <= dx1)
            {
                if (dx >= 0)
                {
                    x = x1; y = y1; xe = x2;
                }
                else
                {
                    x = x2; y = y2; xe = x1;
                }
                DrawPixel(pen, new PointF(x, y));
                for (i = 0; x < xe; i++)
                {
                    x++;
                    if (px < 0)
                    {
                        px += 2 * dy1;
                    }
                    else
                    {
                        if ((dx < 0 && dy < 0) || (dx > 0 && dy > 0))
                        {
                            y++;
                        }
                        else
                        {
                            y--;
                        }
                        px += 2 * (dy1 - dx1);
                    }
                    DrawPixel(pen, new PointF(x, y));
                }
            }
            else
            {
                if (dy >= 0)
                {
                    x = x1; y = y1; ye = y2;
                }
                else
                {
                    x = x2; y = y2; ye = y1;
                }
                DrawPixel(pen, new PointF(x, y));
                for (i = 0; y < ye; i++)
                {
                    y++;
                    if (py <= 0)
                    {
                        py += 2 * dx1;
                    }
                    else
                    {
                        if ((dx < 0 && dy < 0) || (dx > 0 && dy > 0))
                        {
                            x++;
                        }
                        else
                        {
                            x--;
                        }
                        py += 2 * (dx1 - dy1);
                    }
                    DrawPixel(pen, new PointF(x, y));
                }
            }

            var xMax = Math.Max(x1, x2);
            var xMin = Math.Min(x1, x2);
            var yMax = Math.Max(y1, y2);
            var yMin = Math.Min(y1, y2);

            return new RectangleF(new PointF(xMin, yMin), new SizeF(xMax - xMin, yMax - yMin));
        }

        private RectangleF DrawHorizontalLine(Pen pen, PointF position, int length)
        {
            var solidBrush = GetSolidBrush(pen.Brush);
            var color = solidBrush.Color;
            var direction = length > 0 ? 1 : -1;
            for (int i = 0; i < Math.Abs(length); i++)
            {
                float x = position.X + i * direction;
                Write(x, position.Y, "██", color);
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
                Write(position.X, y, "██", color);
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
                Write(position.X + i * direction * (right ? 1 : -1), position.Y + i * direction, right ? "██" : "██", color);
            }

            return new RectangleF(position, new SizeF(calculatedLength, calculatedLength));
        }

        public RectangleF DrawPoligon(Pen pen, bool closed, bool relative = true, params PointF[] points)
        {
            var boundsList = new List<RectangleF>();
            PointF firstPoint = points[0];
            for (int i = 0; i < points.Length; i++)
            {
                var p1 = points[i];
                if (relative && i > 0)
                {
                    p1 = new PointF(firstPoint.X + p1.X, firstPoint.Y + p1.Y);
                }

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
                if (relative)
                {
                    p2 = new PointF(firstPoint.X + p2.X, firstPoint.Y + p2.Y);
                }

                var bounds = DrawLine(pen, p1, p2);
                boundsList.Add(bounds);
            }

            var x = boundsList.Min(x => x.X);
            var y = boundsList.Min(x => x.Y);
            var width = Math.Abs(boundsList.Max(x => x.X) - x);
            var height = Math.Abs(boundsList.Max(x => x.Y) - y);

            return new RectangleF(new PointF((float)x, (float)y), new SizeF((float)width, (float)height));
        }

        public RectangleF DrawRectangle(Pen pen, PointF position, float width, float height)
        {
            return DrawPoligon(pen, true, false,
                    position,
                    new PointF(position.X + width, position.Y),
                    new PointF(position.X + width, position.Y + height),
                    new PointF(position.X, position.Y + height));
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
            int intX = (int)x * scaleX;
            int intY = (int)y;
            int width = Width * scaleX;
            if (intX < 0 || intY < 0 || intX > width || intY >= Height)
            {
                return;
            }

            var position = (intX + intY * width) + (intX + intY * width) / width;

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
            Output = new string(' ', Height * Width * scaleX);

            for (int i = 1; i < Height; i++)
            {
                int numberOfBreakLines = (i - 1);
                Output = Output.Insert(Width * scaleX * i + numberOfBreakLines, "\n");
            }
        }
    }
}