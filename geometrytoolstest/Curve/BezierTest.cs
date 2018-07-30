using Xunit;
using System.Numerics;
using GeometryTools;

namespace GeometryToolsTest
{
    public class BezierTest
    {
        [Fact]
        public void TestBezier_Quadratic_StartFinish()
        {
            var curve = Bezier.CreateBezierQuadratic(new Vector2(0.0f, 0.0f), new Vector2(150.0f, 100.0f), new Vector2(100.0f, 100.0f));

            var point = Bezier.PointOnCurve(0.0f, curve);

            Assert.Equal(0.0f, point.X, 5);
            Assert.Equal(0.0f, point.Y, 5);

            point = Bezier.PointOnCurve(1.0f, curve);

            Assert.Equal(100.0f, point.X, 5);
            Assert.Equal(100.0f, point.Y, 5);
        }

        [Fact]
        public void TestBezier_Quadratic_StartFinishOutOfRange()
        {
            var curve = Bezier.CreateBezierQuadratic(new Vector2(0.0f, 0.0f), new Vector2(150.0f, 100.0f), new Vector2(100.0f, 100.0f));

            var point = Bezier.PointOnCurve(-100.0f, curve);

            Assert.Equal(0.0f, point.X, 5);
            Assert.Equal(0.0f, point.Y, 5);

            point = Bezier.PointOnCurve(100.0f, curve);

            Assert.Equal(100.0f, point.X, 5);
            Assert.Equal(100.0f, point.Y, 5);
        }

        [Fact]
        public void TestBezier_Quadratic_MiddleOfStraightLine()
        {
            var curve = Bezier.CreateBezierQuadratic(new Vector2(0.0f, 0.0f), new Vector2(50.0f, 0.0f), new Vector2(100.0f, 0.0f));

            var point = Bezier.PointOnCurve(0.5f, curve);

            Assert.Equal(50.0f, point.X, 5);
            Assert.Equal(0.0f, point.Y, 5);
        }

        [Fact]
        public void TestBezier_Cubic_StartFinish()
        {
            var curve = Bezier.CreateBezierCubic(new Vector2(0.0f, 0.0f), new Vector2(-100.0f, -100.0f), new Vector2(100.0f, 100.0f), new Vector2(150.0f, 100.0f));

            var point = Bezier.PointOnCurve(0.0f, curve);

            Assert.Equal(0.0f, point.X, 5);
            Assert.Equal(0.0f, point.Y, 5);

            point = Bezier.PointOnCurve(1.0f, curve);

            Assert.Equal(100.0f, point.X, 5);
            Assert.Equal(100.0f, point.Y, 5);
        }

        [Fact]
        public void TestBezier_Cubic_StartFinishOutOfRange()
        {
            var curve = Bezier.CreateBezierCubic( new Vector2(0.0f, 0.0f), new Vector2(-100.0f, -100.0f), new Vector2(100.0f, 100.0f), new Vector2(150.0f, 100.0f));

            var point = Bezier.PointOnCurve(-100.0f, curve);

            Assert.Equal(0.0f, point.X, 5);
            Assert.Equal(0.0f, point.Y, 5);

            point = Bezier.PointOnCurve(100.0f, curve);

            Assert.Equal(100.0f, point.X, 5);
            Assert.Equal(100.0f, point.Y, 5);
        }

        [Fact]
        public void TestBezier_Cubic_MiddleOfStraightLine()
        {
            var curve = Bezier.CreateBezierCubic(new Vector2(0.0f, 0.0f), new Vector2(10.0f, 0.0f), new Vector2(100.0f, 0.0f), new Vector2(90.0f, 0.0f));

            var point = Bezier.PointOnCurve(0.5f, curve);

            Assert.Equal(50.0f, point.X, 5);
            Assert.Equal(0.0f, point.Y, 5);
        }
    }
}