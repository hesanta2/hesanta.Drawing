using hesanta.Drawing.ASCII;
using hesanta.Drawing.Engine;
using System;
using System.Drawing;
using TrueColorConsole;
using Graphics = hesanta.Drawing.ASCII.Graphics;

namespace hesanta.Drawing.Console.Sample
{
    partial class Program
    {
        private static SolidBrush textBrush = new SolidBrush(Color.Aqua);
        private static float x = 0;
        private static float velocityDirection = 1;
        private static bool colored = true;

        static void Main()
        {
            IGraphics<string> graphics = new Graphics(120, 30);
            IGraphicsEngine<string> engine = new GraphicsEngineASCII(graphics);

            IEngineObject<string> legend = new Legend(engine);
            IEngineObject<string> movingXSquare = new MovingXSquare(engine);
            IEngineObject<string> poligon = new Poligon(engine);

            engine.Update = (pressedKey) =>
            {
                if (pressedKey == ConsoleKey.Escape) { engine.EngineRunning = false; }

                graphics.Clear();

                legend.Draw(colored);
                movingXSquare.Draw(x);
                poligon.Draw();

                ProcessVelocityAndDirection(engine);
                ProcessColoredKeys(pressedKey);
                Render(graphics, engine);
            };
            engine.Start();
        }

        private static void Render(IGraphics<string> graphics, IGraphicsEngine<string> engine)
        {
            System.Console.CursorVisible = false;
            System.Console.SetCursorPosition(0, 0);
            if (colored)
            {
                engine.Flush((string output, Color color) =>
                {
                    VTConsole.Write(output, color);
                });
            }
            else
            {
                VTConsole.Write(graphics.Output, Color.White);
            }
        }

        private static void ProcessVelocityAndDirection(IGraphicsEngine<string> engine)
        {
            if (velocityDirection == 1 && x >= 10)
            {
                velocityDirection = -1;
            }
            else if (velocityDirection == -1 && x <= 0)
            {
                velocityDirection = 1;
            }

            x += 25 * engine.DeltaTime * velocityDirection;
        }

        private static void ProcessColoredKeys(ConsoleKey? key)
        {
            if (!colored)
            {
                colored = key == ConsoleKey.C;
            }
            else
            {
                colored = !(key == ConsoleKey.B);
            }
        }
    }
}
