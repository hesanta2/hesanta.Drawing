using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace hesanta.Drawing.Engine
{
    public abstract class EngineObject<T> : IEngineObject<T>
    {
        protected List<RectangleF> boundsList = new List<RectangleF>();

        public IGraphicsEngine<T> Engine { get; }
        public virtual Position Position { get; set; } = new PointF(0, 0);
        public virtual RectangleF Bounds { get; protected set; }

        protected EngineObject(IGraphicsEngine<T> engine)
        {
            Engine = engine ?? throw new ArgumentNullException(nameof(engine));
            Engine.AddEngineObject(this);
        }

        public abstract void InternalDraw(params object[] args);

        public void Draw(params object[] args)
        {
            boundsList.Clear();
            InternalDraw(args);

            if (boundsList.Any())
            {
                var x = Math.Ceiling(boundsList.Min(x => x.X));
                var y = Math.Ceiling(boundsList.Min(x => x.Y));
                var width = Math.Ceiling(Math.Abs(boundsList.Max(x => x.X + x.Width) - x));
                var height = Math.Ceiling(Math.Abs(boundsList.Max(x => x.Y + x.Height) - y));

                Bounds = new RectangleF((float)x, (float)y, (float)width, (float)height);
            }
        }


        public void DrawPixel(Pen pen, PointF position, bool addToBounds = true)
        {
            var bounds = Engine.Graphics.DrawPixel(pen, position);
            if (addToBounds)
            {
                boundsList.Add(bounds);
            }
        }

        public void DrawString(string s, Brush brush, PointF position, bool addToBounds = true)
        {
            var bounds = Engine.Graphics.DrawString(s, brush, position);
            if (addToBounds)
            {
                boundsList.Add(bounds);
            }
        }

        public void DrawRectangle(Pen pen, PointF position, float width, float height, bool addToBounds = true)
        {
            var bounds = Engine.Graphics.DrawRectangle(pen, position, width, height);
            if (addToBounds)
            {
                boundsList.Add(bounds);
            }
        }

        public void DrawLine(Pen pen, PointF p1, PointF p2, bool addToBounds = true)
        {
            var bounds = Engine.Graphics.DrawLine(pen, p1, p2);
            if (addToBounds)
            {
                boundsList.Add(bounds);
            }
        }

        public void DrawPoligon(Pen pen, bool closed, bool relative = true, bool addToBounds = true, params PointF[] points)
        {
            var bounds = Engine.Graphics.DrawPoligon(pen, closed, relative, points);
            if (addToBounds)
            {
                boundsList.Add(bounds);
            }
        }
    }
}