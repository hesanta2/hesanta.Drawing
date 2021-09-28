using System;
using System.Collections.Generic;
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
        IEnumerable<EngineObject<T>> EngineObjects { get; }

        void Start();
        void Flush(Action<T, Color> outputWithColor);
        ConsoleKey? HookKeys();
        void AddEngineObject(EngineObject<T> engineObject);
        void Reset();
    }
}