using hesanta.Drawing.Engine;
using System.Collections.Generic;
using System.Drawing;

namespace hesanta.Drawing.Console.Sample
{
    public class Poligon : EngineObject<string>
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
}
