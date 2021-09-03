using hesanta.Drawing.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace hesanta.Drawing.ASCII
{
    public class GraphicsEngineASCII : GraphicsEngine<string>
    {
        public GraphicsEngineASCII(IGraphics<string> graphics) : base(graphics) { }

        public override void Flush(Action<string, System.Drawing.Color> outputWithColor)
        {
            var output = Graphics.Output;
            for (int i = 0; i < Graphics.Colors.Count; i++)
            {
                var colorEntry = Graphics.Colors.ElementAt(i);
                var nextColorEntry = i < Graphics.Colors.Count - 1 ? Graphics.Colors.ElementAt(i + 1) : new KeyValuePair<int, System.Drawing.Color>();
                var nextPosition = nextColorEntry.Key;
                var position = i == 0 ? 0 : colorEntry.Key;
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

        public override ConsoleKey? HookKeys()
        {
            if (System.Console.KeyAvailable)
            {
                return System.Console.ReadKey(true).Key;
            }

            return null;
        }
    }
}