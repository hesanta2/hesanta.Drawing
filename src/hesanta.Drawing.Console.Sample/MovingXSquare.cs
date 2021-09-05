using hesanta.Drawing.Engine;
using System.Collections.Generic;
using System.Drawing;

namespace hesanta.Drawing.Console.Sample
{
    public class MovingXSquare : EngineObject<string>
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
        float x = 0;

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

        public override void InternalDraw(params object[] args)
        {
            float x = args?.Length > 0 ? (float)args[0] : 0;

            DrawRectangle(new Pen(new SolidBrush(Color.Blue), 2), new PointF(x, 6), 30, 15);
            DrawRectangle(new Pen(new SolidBrush(Color.Red)), new PointF(x + 7, 9), 16, 8);
            DrawRectangle(new Pen(new SolidBrush(Color.Pink)), new PointF(x + 11, 11), 7, 4);
            DrawLine(new Pen(new SolidBrush(Color.DarkGreen)), line1P1, line1P2);
            DrawLine(new Pen(new SolidBrush(Color.Coral)), line2P1, line2P2);
            DrawLine(new Pen(new SolidBrush(Color.DarkTurquoise)), diagonalLineP1, diagonalLineP2);
            DrawLine(new Pen(new SolidBrush(Color.DarkTurquoise)), diagonalLine2P1, diagonalLine2P2);
        }
    }
}
