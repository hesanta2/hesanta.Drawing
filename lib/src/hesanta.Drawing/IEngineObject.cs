using System.Drawing;

namespace hesanta.Drawing
{
    public interface IEngineObject<T>
    {
        IGraphicsEngine<T> Engine { get; }
        PointF Position { get; set; }
        SizeF Size { get; }

        void Draw(params object[] args);
    }
}