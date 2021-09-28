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

        private static float x = 0;
        private static float velocityDirection = 1;
        private static bool colored = true;

        private static IEngineObject<string> legend;
        private static IEngineObject<string> movingXSquare;
        private static IEngineObject<string> poligon;
        private static IEngineObject<string> ship;

        static void Main()
        {
            float aspectProportion = 0.9f;
            int width = (int)(System.Console.LargestWindowWidth * aspectProportion);
            int height = (int)(System.Console.LargestWindowHeight * aspectProportion);
            graphics = new Graphics(100, 50);
            engine = new GraphicsEngineASCII(width, height, graphics);

            legend = new Legend(engine);
            legend.Position.X += 2;
            legend.Position.Y += 2;
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
                graphics.DrawRectangle(new Pen(new SolidBrush(Color.Red)), new PointF(poligon.Bounds.X, poligon.Bounds.Y), poligon.Bounds.Width, poligon.Bounds.Height);

                //graphics.DrawPixel(new Pen(new SolidBrush(Color.BlueViolet)), new PointF(25, 25));
                //graphics.DrawLine(new Pen(new SolidBrush(Color.Fuchsia)), new PointF(1, 3), new PointF(13, 8));
                //var bounds = graphics.DrawLine(new Pen(new SolidBrush(Color.Fuchsia)), new PointF(3, 20), new PointF(23, 10));
                //graphics.DrawRectangle(new Pen(new SolidBrush(Color.Red)), new PointF(bounds.X, bounds.Y), bounds.Width, bounds.Height);

                //graphics.DrawRectangle(new Pen(Color.Red), new PointF(0, 0), width, height);

                //graphics.DrawRectangle(new Pen(Color.Red), new PointF(2, 2), 25, 25);

                for (int x = 0; x < graphics.Width; x++)
                {
                    graphics.DrawString(x.ToString(), new SolidBrush(Color.White), new PointF(x, 0));
                }

                for (int y = 0; y < graphics.Height; y++)
                {
                    graphics.DrawString(y.ToString(), new SolidBrush(Color.White), new PointF(0, y));
                }

                if (pressedKey == ConsoleKey.LeftArrow && ship.Position.X > 0)
                    ship.Position.X -= velocity;
                if (pressedKey == ConsoleKey.RightArrow && ship.Position.X < engine.Graphics.Width - ship.Bounds.Width)
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
            if (velocityDirection == 1 && x >= 15)
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
