using hesanta.Drawing.Engine;
using System.Collections.Generic;
using System.Drawing;

namespace hesanta.Drawing.Console.Sample
{
    public class Poligon : EngineObject<string>
    {
        PointF poligon = new PointF(70, 5);
        public override Position Position => poligon;

        public Poligon(IGraphicsEngine<string> engine) : base(engine) { }

        public override void InternalDraw(params object[] args)
        {
            DrawPoligon(
                new Pen(new SolidBrush(Color.Gold)),
                true,
                relative: false,
                addToBounds: true
                , new PointF(poligon.X + 0, poligon.Y + 0)
                , new PointF(poligon.X + 10, poligon.Y + 0)
                , new PointF(poligon.X + 15, poligon.Y + 5)
                , new PointF(poligon.X + 15, poligon.Y + 10)
                , new PointF(poligon.X + 10, poligon.Y + 15)
                , new PointF(poligon.X + 0, poligon.Y + 15)
                , new PointF(poligon.X - 5, poligon.Y + 10)
                , new PointF(poligon.X - 5, poligon.Y + 5)
                );
        }
    }
}
