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
        private static IGraphics<string> graphics;
        private static IGraphicsEngine<string> engine;

        private static SolidBrush textBrush = new SolidBrush(Color.Aqua);
        private static float x = 0;
        private static float velocityDirection = 1;
        private static bool colored = true;

        private static IEngineObject<string> legend;
        private static IEngineObject<string> movingXSquare;
        private static IEngineObject<string> poligon;
        private static IEngineObject<string> ship;

        static void Main()
        {
            graphics = new Graphics(120, 30);
            engine = new GraphicsEngineASCII(graphics);

            legend = new Legend(engine);
            movingXSquare = new MovingXSquare(engine);
            poligon = new Poligon(engine);

            engine.Update = (pressedKey) =>
            {
                ProcessKeys(pressedKey);
                ProcessVelocityAndDirection(engine);

                if (pressedKey == ConsoleKey.Escape) { engine.EngineRunning = false; }
                var velocity = 1f;

                graphics.Clear();
                legend.Draw(colored);
                movingXSquare.Draw(x);
                poligon.Draw();

                if (pressedKey == ConsoleKey.LeftArrow && ship.Position.X > 0)
                    ship.Position.X -= velocity;
                if (pressedKey == ConsoleKey.RightArrow && ship.Position.X < engine.Graphics.Width - ship.Size.Width)
                    ship.Position.X += velocity;


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

        private static void ProcessKeys(ConsoleKey? key)
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
