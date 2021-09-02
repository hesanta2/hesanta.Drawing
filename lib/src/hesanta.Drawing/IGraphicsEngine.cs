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
        IGraphics<T> Graphics { get; }

        void Start();
        void Flush(Action<string, Color> outputWithColor);
    }
}