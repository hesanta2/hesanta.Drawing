using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace hesanta.Drawing.ASCII.Test
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class GraphicsTests
    {
        private IGraphics<string> graphics;

        [TestInitialize]
        public void Initialize()
        {
            graphics = new Graphics(100, 50);
        }

        [TestMethod]
        public void Graphics_ShouldInstantiateProperly()
        {
            //Arrange
            var width = 100;
            var height = 50;

            //Act
            graphics = new Graphics(width, height);

            //Assert
            Assert.AreEqual(width, graphics.Width);
            Assert.AreEqual(height, graphics.Height);
            Assert.AreEqual(100, graphics.Output.IndexOf('\n'));
            Assert.AreEqual(201, graphics.Output.IndexOf('\n', 101));
            Assert.AreEqual(302, graphics.Output.IndexOf('\n', 202));
            Assert.AreEqual(403, graphics.Output.IndexOf('\n', 303));
        }

        [TestMethod]
        public void DrawString_ShouldOuputContainTheString()
        {
            //Arrange
            var x = 5;
            var y = 5;
            var s = "SomeString";

            //Act
            graphics.DrawString(s, new SolidBrush(Color.White), new PointF(x, y));

            //Assert
            Assert.AreEqual(graphics.Width * y + x + y, graphics.Output.IndexOf(s));
        }

        [TestMethod]
        public void DrawRectangle_ShouldOuputContainTheStringRectangle()
        {
            //Arrange
            var x = 5;
            var y = 5;
            var width = 5;
            var height = 5;

            //Act
            graphics.DrawRectangle(new Pen(Color.White, 1), new PointF(x, y), width, height);

            //Assert
            Assert.AreEqual(graphics.Width * y + x + y, graphics.Output.IndexOf("┌"));
        }
    }
}
