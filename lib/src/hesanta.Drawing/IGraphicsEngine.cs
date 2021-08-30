using System;
using System.Drawing;

namespace hesanta.Drawing
{
    public interface IGraphicsEngine<T>
    {
        Action Update { get; set; }
        bool EngineRunning { get; set; }
        int FPS { get; }
        float DeltaTime { get; }

        void Start();
        void Flush(IGraphics<T> graphics, Action<string, Color> outputWithColor);
        void Flush2(IGraphics<T> graphics, Action<(PointF, string), Color> outputWithColor);
    }
}