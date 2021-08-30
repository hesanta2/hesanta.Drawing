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
        SortedList<(float, float), (T, Color)> Output2 { get; }

        void DrawString(string s, Brush brush, PointF position);
        void DrawRectangle(Pen pen, PointF position, float width, float height);
        void DrawLine(Pen pen, PointF p1, PointF p2);
        void Clear();
        void DrawPoligon(Pen pen, bool closed, params PointF[] points);
    }
}