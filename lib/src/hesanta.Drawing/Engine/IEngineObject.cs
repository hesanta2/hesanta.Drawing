using System.Drawing;

namespace hesanta.Drawing.Engine
{
    public interface IEngineObject<T>
    {
        IGraphicsEngine<T> Engine { get; }
        Position Position { get; set; }
        SizeF Size { get; }

        void Draw(params object[] args);
    }
}