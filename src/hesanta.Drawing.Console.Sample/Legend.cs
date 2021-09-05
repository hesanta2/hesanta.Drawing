using hesanta.Drawing.Engine;
using System.Collections.Generic;
using System.Drawing;

namespace hesanta.Drawing.Console.Sample
{
    public class Legend : EngineObject<string>
    {
        private SolidBrush textBrush = new SolidBrush(Color.Aqua);
        public Legend(IGraphicsEngine<string> engine) : base(engine) { }

        public override void InternalDraw(params object[] args)
        {
            bool colored = args?.Length > 0 ? (bool)args[0] : false;

            DrawString($@"
FPS: {Engine.FPS} 
DeltaTime: {Engine.DeltaTime}
Colored(keys: c / b): {colored}
", textBrush, Position);
        }
    }
}
