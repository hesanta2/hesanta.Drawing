using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace hesanta.Drawing
{
    public abstract class EngineObject<T> : IEngineObject<T>
    {
        public IGraphicsEngine<T> Engine { get; }
        public PointF Position { get; set; } = new Point(0, 0);
        public SizeF Size { get; protected set; }

        protected EngineObject(IGraphicsEngine<T> engine)
        {
            Engine = engine ?? throw new ArgumentNullException(nameof(engine));
        }

        public abstract IEnumerable<RectangleF> InternalDraw();

        public void Draw()
        {
            var boundsList = InternalDraw();
            var x = Math.Ceiling(boundsList.Min(x => x.X));
            var y = Math.Ceiling(boundsList.Min(x => x.Y));
            var width = Math.Ceiling(Math.Abs(boundsList.Max(x => x.X + x.Width) - x));
            var height = Math.Ceiling(Math.Abs(boundsList.Max(x => x.Y + x.Height) - y));
            Size = new SizeF((float)width, (float)height);
            Position = new PointF((float)x, (float)y);
        }
    }
}