using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace hesanta.Drawing.Engine
{
    public abstract class EngineObject<T> : IEngineObject<T>
    {
        public IGraphicsEngine<T> Engine { get; }
        public Position Position { get; set; } = new PointF(0, 0);
        public SizeF Size { get; protected set; }

        protected EngineObject(IGraphicsEngine<T> engine)
        {
            Engine = engine ?? throw new ArgumentNullException(nameof(engine));
        }

        public abstract IEnumerable<RectangleF> InternalDraw(params object[] args);

        public void Draw(params object[] args)
        {
            var boundsList = InternalDraw(args);
            var x = Math.Ceiling(boundsList.Min(x => x.X));
            var y = Math.Ceiling(boundsList.Min(x => x.Y));
            var width = Math.Ceiling(Math.Abs(boundsList.Max(x => x.X + x.Width) - x));
            var height = Math.Ceiling(Math.Abs(boundsList.Max(x => x.Y + x.Height) - y));
            Size = new SizeF((float)width, (float)height);
            Position = new PointF((float)x, (float)y);
        }
    }
}