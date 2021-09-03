using System;
using System.Drawing;

namespace hesanta.Drawing.Engine
{
    public interface IGraphicsEngine<T>
    {
        Action<ConsoleKey?> Update { get; set; }
        bool EngineRunning { get; set; }
        int FPS { get; }
        float DeltaTime { get; }
        IGraphics<T> Graphics { get; }
        ConsoleKey? PressedKey { get; }

        void Start();
        void Flush(Action<string, Color> outputWithColor);
        ConsoleKey? HookKeys();
    }
}