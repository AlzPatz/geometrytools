using Xunit;
using GeometryTools;

namespace GeometryToolsTest
{
    public class CircleTest
    {
        [Fact]
        public void TestCircleMove()
        {
            var circle = new Circle(new System.Numerics.Vector2(10.0f, 10.0f), 100.0f);

            circle.Translate(new System.Numerics.Vector2(15.0f, -30.0f));

            Assert.Equal(25.0f, circle.Position.X);
            Assert.Equal(-20.0f, circle.Position.Y);
        }
    }
}