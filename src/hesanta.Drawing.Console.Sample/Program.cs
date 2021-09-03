using hesanta.Drawing.ASCII;
using hesanta.Drawing.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using TrueColorConsole;
using Graphics = hesanta.Drawing.ASCII.Graphics;

namespace hesanta.Drawing.Console.Sample
{
    class Program
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

                legend.Draw();
                movingXSquare.Draw();
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

        private class Legend : EngineObject<string>
        {
            public Legend(IGraphicsEngine<string> engine) : base(engine) { }

            public override IEnumerable<RectangleF> InternalDraw(params object[] args)
            {
                var b1 = Engine.Graphics.DrawString($@"
FPS: {Engine.FPS} 
DeltaTime: { Engine.DeltaTime}
Colored(keys: c / b): { colored}
", textBrush, Position);

                return new List<RectangleF> { b1 };
            }
        }

        private class Poligon : EngineObject<string>
        {
            PointF poligon = new PointF(70, 5);

            public Poligon(IGraphicsEngine<string> engine) : base(engine) { }

            public override IEnumerable<RectangleF> InternalDraw(params object[] args)
            {
                var b1 = Engine.Graphics.DrawPoligon(
                    new Pen(new SolidBrush(Color.Gold)),
                    true
                    , new PointF(poligon.X + 0, poligon.Y + 0)
                    , new PointF(poligon.X + 10, poligon.Y + 0)
                    , new PointF(poligon.X + 15, poligon.Y + 5)
                    , new PointF(poligon.X + 15, poligon.Y + 10)
                    , new PointF(poligon.X + 10, poligon.Y + 15)
                    , new PointF(poligon.X + 0, poligon.Y + 15)
                    , new PointF(poligon.X - 5, poligon.Y + 10)
                    , new PointF(poligon.X - 5, poligon.Y + 5)
                    );

                return new List<RectangleF> { b1 };
            }
        }

        private class MovingXSquare : EngineObject<string>
        {
            int diagonalLineLength = 14;
            PointF diagonalLineP1;
            PointF diagonalLineP2;
            PointF diagonalLine2P1;
            PointF diagonalLine2P2;

            int lineLength = 15;
            PointF line1P1;
            PointF line1P2;
            PointF line2P1;
            PointF line2P2;

            public MovingXSquare(IGraphicsEngine<string> engine) : base(engine)
            {
                diagonalLineP1 = new PointF(11, 6);
                diagonalLineP2 = new PointF(diagonalLineP1.X + diagonalLineLength, diagonalLineP1.Y + diagonalLineLength);
                diagonalLine2P1 = new PointF(25, 6);
                diagonalLine2P2 = new PointF(diagonalLine2P1.X - diagonalLineLength, diagonalLine2P1.Y + diagonalLineLength);
                line1P1 = new PointF(18, 6);
                line1P2 = new PointF(18, 6 + lineLength);
                line2P1 = new PointF(0, 13);
                line2P2 = new PointF(lineLength * 3, 13);
            }

            public override IEnumerable<RectangleF> InternalDraw(params object[] args)
            {
                var b = Engine.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Blue), 2), new PointF(x, 6), 30, 15);
                var b1 = Engine.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Red)), new PointF(x + 7, 9), 16, 8);
                var b2 = Engine.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Pink)), new PointF(x + 11, 11), 7, 4);
                var b3 = Engine.Graphics.DrawLine(new Pen(new SolidBrush(Color.DarkGreen)), line1P1, line1P2);
                var b4 = Engine.Graphics.DrawLine(new Pen(new SolidBrush(Color.Coral)), line2P1, line2P2);
                var b5 = Engine.Graphics.DrawLine(new Pen(new SolidBrush(Color.DarkTurquoise)), diagonalLineP1, diagonalLineP2);
                var b6 = Engine.Graphics.DrawLine(new Pen(new SolidBrush(Color.DarkTurquoise)), diagonalLine2P1, diagonalLine2P2);

                return new List<RectangleF> { b, b1, b2, b3, b4, b5, b6 };
            }
        }
    }
}
