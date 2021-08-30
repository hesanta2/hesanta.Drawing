using hesanta.Drawing.ASCII;
using System;
using System.Drawing;
using TrueColorConsole;
using Graphics = hesanta.Drawing.ASCII.Graphics;

namespace hesanta.Drawing.Console.Sample
{
    class Program
    {
        private static ConsoleKeyInfo? pressedKey = null;
        private static SolidBrush textBrush = new SolidBrush(Color.Aqua);
        private static float x = 0;
        private static float velocityDirection = 1;
        private static bool colored = true;

        static void Main()
        {
            var diagonalLineLength = 14;
            var diagonalLineP1 = new PointF(11, 6);
            var diagonalLineP2 = new PointF(diagonalLineP1.X + diagonalLineLength, diagonalLineP1.Y + diagonalLineLength);
            var diagonalLine2P1 = new PointF(25, 6);
            var diagonalLine2P2 = new PointF(diagonalLine2P1.X - diagonalLineLength, diagonalLine2P1.Y + diagonalLineLength);

            var lineLength = 15;
            var line1P1 = new PointF(18, 6);
            var line1P2 = new PointF(18, 6 + lineLength);
            var line2P1 = new PointF(0, 13);
            var line2P2 = new PointF(lineLength * 3, 13);
            PointF poligon = new PointF(70, 10);

            IGraphics<string> graphics = new Graphics(120, 30);

            IGraphicsEngine<string> engine = new GraphicsEngineASCII();
            engine.Update = () =>
            {
                pressedKey = HookKey(engine);
                if (pressedKey?.Key == ConsoleKey.Escape) { engine.EngineRunning = false; }

                graphics.Clear();

                graphics.DrawString($"FPS: {engine.FPS}", textBrush, new PointF(0, 0));
                graphics.DrawString($"DeltaTime: {engine.DeltaTime}", textBrush, new PointF(0, 1));
                graphics.DrawString($"Colored (keys:c/b): {colored}", textBrush, new PointF(0, 2));

                graphics.DrawRectangle(new Pen(new SolidBrush(Color.Blue), 2), new PointF(x, 6), 30, 15);
                graphics.DrawRectangle(new Pen(new SolidBrush(Color.Red)), new PointF(x + 7, 9), 16, 8);
                graphics.DrawRectangle(new Pen(new SolidBrush(Color.Pink)), new PointF(x + 11, 11), 7, 4);
                graphics.DrawLine(new Pen(new SolidBrush(Color.DarkGreen)), line1P1, line1P2);
                graphics.DrawLine(new Pen(new SolidBrush(Color.Coral)), line2P1, line2P2);
                graphics.DrawLine(new Pen(new SolidBrush(Color.DarkTurquoise)), diagonalLineP1, diagonalLineP2);
                graphics.DrawLine(new Pen(new SolidBrush(Color.DarkTurquoise)), diagonalLine2P1, diagonalLine2P2);

                graphics.DrawPoligon(
                    new Pen(new SolidBrush(Color.Gold)),
                    true
                    , new PointF(poligon.X + 0, poligon.Y + 0)
                    , new PointF(poligon.X + 0, poligon.Y + 10)
                    , new PointF(poligon.X + 10, poligon.Y + 10)
                    , new PointF(poligon.X + 20, poligon.Y + 0)
                    , new PointF(poligon.X + 16, poligon.Y - 4)
                    , new PointF(poligon.X + 4, poligon.Y - 4)
                    );

                ProcessVelocityAndDirection(engine);

                ProcessColoredKeys();

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
                engine.Flush(graphics, (string output, Color color) =>
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

        private static void ProcessColoredKeys()
        {
            if (!colored)
            {
                colored = pressedKey?.Key == ConsoleKey.C;
            }
            else
            {
                colored = !(pressedKey?.Key == ConsoleKey.B);
            }
        }

        private static ConsoleKeyInfo? HookKey(IGraphicsEngine<string> engine)
        {
            if (System.Console.KeyAvailable)
            {
                return System.Console.ReadKey(true);
            }

            return null;
        }
    }
}
