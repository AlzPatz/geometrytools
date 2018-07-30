using System;
using System.Numerics;
using Xunit;
using GeometryTools;

namespace GeometryToolsTest
{
    public class BezierSplineTest
    {
        [Fact]
        public void BezierSpline_TestClosed()
        {
            var points = new Vector2[]
            {
                new Vector2(-50.0f, 0.0f),
                new Vector2(0.0f, 86.60254037844f),
                new Vector2(50.0f, 0.0f)
            };

            var spline = BezierSpline.Create(points, 0.5f, true, 100);

            var start = spline.PointOnSpline(0.0f);
            var third = spline.PointOnSpline(1.0f / 3.0f);
            var twothirds = spline.PointOnSpline(2.0f / 3.0f);
            var end = spline.PointOnSpline(1.0f);

            Assert.Equal(-50.0f, start.X, 4);
            Assert.Equal(0.0f, start.Y, 4);
            Assert.Equal(-50.0f, end.X, 4);
            Assert.Equal(0.0f, end.Y, 4);
            Assert.Equal(0.0f, third.X, 4);
            Assert.Equal(86.60254037844f, third.Y, 4);
            Assert.Equal(50.0f, twothirds.X, 4);
            Assert.Equal(0.0f, twothirds.Y, 4);
        }

        [Fact]
        public void BezierSpline_TestClosed_ZeroSmooth()
        {
            var height = 86.60254037844f;
            var points = new Vector2[]
            {
                new Vector2(-50.0f, 0.0f),
                new Vector2(0.0f, height),
                new Vector2(50.0f, 0.0f)
            };

            var spline = BezierSpline.Create(points, 0.0f, true, 100);

            var halfwayUpFirstSide = spline.PointOnSpline(0.5f * (1.0f / 3.0f));

            Assert.Equal(-25.0f, halfwayUpFirstSide.X, 4);
            Assert.Equal(0.5f * height, halfwayUpFirstSide.Y, 4);
        }

        [Fact]
        public void BezierSpline_TestOpen()
        {
            var height = 86.60254037844f;
            var points = new Vector2[]
            {
                new Vector2(-50.0f, 0.0f),
                new Vector2(0.0f, height),
                new Vector2(50.0f, 0.0f)
            };

            var spline = BezierSpline.Create(points, 0.5f, false, 100);

            var start = spline.PointOnSpline(0.0f);
            var half = spline.PointOnSpline(0.5f);
            var end = spline.PointOnSpline(1.0f);

            Assert.Equal(-50.0f, start.X, 4);
            Assert.Equal(0.0f, start.Y, 4);
            Assert.Equal(50.0f, end.X, 4);
            Assert.Equal(0.0f, end.Y, 4);
            Assert.Equal(0.0f, half.X, 4);
            Assert.Equal(height, half.Y, 4);
        }

        [Fact]
        public void BezierSpline_TestOpen_ZeroSmooth()
        {
            var height = 86.60254037844f;
            var points = new Vector2[]
            {
                new Vector2(-50.0f, 0.0f),
                new Vector2(0.0f, height),
                new Vector2(50.0f, 0.0f)
            };

            var spline = BezierSpline.Create(points, 0.0f, false, 100);

            var point = spline.PointOnSpline(0.5f);
            Assert.Equal(0.0f, point.X, 4);
            Assert.Equal(height, point.Y, 4);
        }
    }
}