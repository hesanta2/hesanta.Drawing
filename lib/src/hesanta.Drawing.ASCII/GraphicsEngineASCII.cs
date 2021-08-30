using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace hesanta.Drawing.ASCII
{
    public class GraphicsEngineASCII : GraphicsEngine<string>
    {
        public override void Flush(IGraphics<string> graphics, Action<string, System.Drawing.Color> outputWithColor)
        {
            var output = graphics.Output;
            for (int i = 0; i < graphics.Colors.Count; i++)
            {
                var colorEntry = graphics.Colors.ElementAt(i);
                var nextColorEntry = i < graphics.Colors.Count - 1 ? graphics.Colors.ElementAt(i + 1) : new KeyValuePair<int, System.Drawing.Color>();
                var nextPosition = nextColorEntry.Key;
                var position = colorEntry.Key;
                var color = colorEntry.Value;
                string subOutput;
                if (nextPosition == 0)
                {
                    subOutput = output.Substring(position);
                }
                else
                {
                    var length = nextPosition - position;
                    subOutput = output.Substring(position, length);
                }
                outputWithColor(subOutput, color);
            }
        }

        public override void Flush2(IGraphics<string> graphics, Action<(PointF, string), System.Drawing.Color> outputWithColor)
        {
            foreach (var o in graphics.Output2)
            {
                PointF position = new PointF(o.Key.Item1, o.Key.Item2);
                string s = o.Value.Item1;
                Color color = o.Value.Item2;
                outputWithColor((position, s), color);
            }
        }

    }
}