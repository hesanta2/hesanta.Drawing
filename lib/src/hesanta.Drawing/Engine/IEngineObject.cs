using System.Drawing;

namespace hesanta.Drawing.Engine
{
    public interface IEngineObject<T>
    {
        IGraphicsEngine<T> Engine { get; }
        Position Position { get; set; }
        RectangleF Bounds { get; }

        void Draw(params object[] args);

        void DrawPixel(Pen pen, PointF position, bool addToBounds = true);
        void DrawString(string s, Brush brush, PointF position, bool addToBounds = true);
        void DrawRectangle(Pen pen, PointF position, float width, float height, bool addToBounds = true);
        void DrawLine(Pen pen, PointF p1, PointF p2, bool addToBounds = true);
        void DrawPoligon(Pen pen, bool closed, bool relative = true, bool addToBounds = true, params PointF[] points);
    }
}