using System.Collections.Generic;
using System.Drawing;

namespace hesanta.Drawing
{
    public interface IGraphics<T>
    {
        T Output { get; }
        int Width { get; }
        int Height { get; }
        SortedList<int, Color> Colors { get; }

        RectangleF DrawPixel(Pen pen, PointF position);
        RectangleF DrawString(string s, Brush brush, PointF position);
        RectangleF DrawRectangle(Pen pen, PointF position, float width, float height);
        RectangleF DrawLine(Pen pen, PointF p1, PointF p2);
        RectangleF DrawPoligon(Pen pen, bool closed, bool relative = true, params PointF[] points);
        void Clear();
    }
}