using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace hesanta.Drawing.Engine
{
    public abstract class GraphicsEngine<T> : IGraphicsEngine<T>
    {
        private List<EngineObject<T>> internalEngineObjects = new List<EngineObject<T>>();

        public IEnumerable<EngineObject<T>> EngineObjects => internalEngineObjects;
        public bool EngineRunning { get; set; }
        public Action<ConsoleKey?> Update { get; set; }
        public int FPS => currentFps;
        public float DeltaTime { get; protected set; } = 0.0001f;
        public IGraphics<T> Graphics { get; }
        public ConsoleKey? PressedKey { get; protected set; }

        private static readonly Stopwatch stopwatchFps = new Stopwatch();
        private static readonly Stopwatch stopwatchDelta = new Stopwatch();
        private static int frames = 0;
        private static int currentFps = 0;

        public GraphicsEngine(IGraphics<T> graphics)
        {
            Graphics = graphics ?? throw new ArgumentNullException(nameof(graphics));
        }

        public void AddEngineObject(EngineObject<T> engineObject)
        {
            internalEngineObjects.Add(engineObject);
        }

        public void Start()
        {
            EngineRunning = true;
            stopwatchFps.Start();
            while (EngineRunning)
            {
                stopwatchDelta.Restart();
                PressedKey = HookKeys();
                Update(PressedKey);
                CalculateDeltaAndFrames();
            }
        }

        public abstract ConsoleKey? HookKeys();

        private void CalculateDeltaAndFrames()
        {
            DeltaTime = (float)(stopwatchDelta.Elapsed.TotalMilliseconds / 1000);
            if (stopwatchFps.ElapsedMilliseconds > 1000)
            {
                stopwatchDelta.Restart();

                currentFps = frames;
                frames = 0;
                stopwatchFps.Restart();
            }
            frames++;
        }

        public abstract void Flush(Action<string, Color> outputWithColor);

        public void Reset()
        {
            internalEngineObjects.Clear();
        }
    }
}