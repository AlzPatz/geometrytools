using Xunit;
using GeometryTools;
using System.Numerics;

namespace GeometryToolsTest
{
    public class CatmullRomTest
    {
        [Fact]
        public void Catmull_TestStart()
        {
            var p0 = new Vector2(-10.0f, -10.0f);
            var p1 = new Vector2(10.0f, -10.0f);
            var p2 = new Vector2(30.0f, -20.0f);
            var p3 = new Vector2(50.0f, 0.0f);

            var p = CatMullRom.CatmullRomSpline(p0, p1, p2, p3, 0.0f);

            Assert.Equal(10.0f, p.X);
            Assert.Equal(-10.0f, p.Y);
        }

        [Fact]
        public void Catmull_TestEnd()
        {
            var p0 = new Vector2(-10.0f, -10.0f);
            var p1 = new Vector2(10.0f, -10.0f);
            var p2 = new Vector2(30.0f, -20.0f);
            var p3 = new Vector2(50.0f, 0.0f);

            var p = CatMullRom.CatmullRomSpline(p0, p1, p2, p3, 1.0f);

            Assert.Equal(30.0f, p.X);
            Assert.Equal(-20.0f, p.Y);
        }

        [Fact]
        public void Catmull_TestMiddleOfFlat()
        {
            var p0 = new Vector2(-10.0f, 0.0f);
            var p1 = new Vector2(0.0f, 0.0f);
            var p2 = new Vector2(10.0f, 0.0f);
            var p3 = new Vector2(20.0f, 0.0f);

            var p = CatMullRom.CatmullRomSpline(p0, p1, p2, p3, 0.5f);

            Assert.Equal(5.0f, p.X, 5);
            Assert.Equal(0.0f, p.Y, 5);
        }

        [Fact]
        public void Catmull_ToLargeT()
        {
            var p0 = new Vector2(-10.0f, -10.0f);
            var p1 = new Vector2(10.0f, -10.0f);
            var p2 = new Vector2(30.0f, -20.0f);
            var p3 = new Vector2(50.0f, 0.0f);

            var p = CatMullRom.CatmullRomSpline(p0, p1, p2, p3, 2.0f);

            Assert.Equal(30.0f, p.X);
            Assert.Equal(-20.0f, p.Y);
        }
    }
}
