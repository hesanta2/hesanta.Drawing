using System.Drawing;

namespace hesanta.Drawing.Engine
{
    public interface IEngineObject<T>
    {
        IGraphicsEngine<T> Engine { get; }
        Position Position { get; set; }
        SizeF Size { get; }

        void Draw(params object[] args);

        void DrawString(string s, Brush brush, PointF position, bool addToBounds = true);
        void DrawRectangle(Pen pen, PointF position, float width, float height, bool addToBounds = true);
        void DrawLine(Pen pen, PointF p1, PointF p2, bool addToBounds = true);
        void DrawPoligon(Pen pen, bool closed, bool addToBounds = true, params PointF[] points);
    }
}