using Xunit;
using GeometryTools;

using System.Numerics;

namespace GeometryToolsTest
{
    public class LineIntersectionTest
    {
        [Fact]
        public void FuzzyZeroCheck()
        {
            var value = 0.1f * Constants.Epsilon;

            var result = LineIntersection.IsFuzzyZero(value);

            Assert.True(result);
        }

        [Fact]
        public void FastIntersect_Intersecting()
        {
            var a0 = new Vector2(0.0f, 0.0f);
            var a1 = new Vector2(1.0f, 0.0f);

            var b0 = new Vector2(0.5f, -1.0f);
            var b1 = new Vector2(0.5f, 1.0f);

            var result = LineIntersection.FastLineIntersect(a0, a1, b0, b1);

            Assert.True(result);
        }

        [Fact]
        public void FastIntersect_NotIntersecting()
        {
            var a0 = new Vector2(0.0f, 0.0f);
            var a1 = new Vector2(1.0f, 0.0f);

            var b0 = new Vector2(1.5f, -1.0f);
            var b1 = new Vector2(1.5f, 1.0f);

            var result = LineIntersection.FastLineIntersect(a0, a1, b0, b1);

            Assert.False(result);
        }

        [Fact]
        public void FastIntersect_ConfirmParralellOverlapingNotDetected()
        {
            var a0 = new Vector2(0.0f, 2.0f);
            var a1 = new Vector2(1.0f, 2.0f);

            var b0 = new Vector2(0.1f, 2.0f);
            var b1 = new Vector2(1.2f, 2.0f);

            var result = LineIntersection.FastLineIntersect(a0, a1, b0, b1);

            Assert.False(result);
        }

        [Fact]
        public void RobustIntersect_IntersectingBasic()
        {
            var a0 = new Vector2(0.0f, 0.0f);
            var a1 = new Vector2(1.0f, 0.0f);

            var b0 = new Vector2(0.5f, -1.0f);
            var b1 = new Vector2(0.5f, 1.0f);

            var result = LineIntersection.RobustLineIntersectAndResult(a0, a1, b0, b1);

            Assert.True(result.Intersecting);
            Assert.False(result.CoLinear);
            Assert.Equal(0.5f, result.IntersectPoint.X);
            Assert.Equal(0.0f, result.IntersectPoint.Y);
        }

        [Fact]
        public void RobustIntersect_NotIntersectingBasic()
        {
            var a0 = new Vector2(0.0f, 0.0f);
            var a1 = new Vector2(1.0f, 0.0f);

            var b0 = new Vector2(1.5f, -1.0f);
            var b1 = new Vector2(1.5f, 1.0f);

            var result = LineIntersection.RobustLineIntersectAndResult(a0, a1, b0, b1);

            Assert.False(result.Intersecting);
            Assert.False(result.CoLinear);
            Assert.Equal(0.0f, result.IntersectPoint.X);
            Assert.Equal(0.0f, result.IntersectPoint.Y);
        }

        [Fact]
        public void RobustIntersect_FailOnZeroLengthLines()
        {
            var a0 = new Vector2(0.0f, 0.0f);
            var a1 = new Vector2(0.0f, 0.0f);

            var b0 = new Vector2(1.0f, 0.0f);
            var b1 = new Vector2(1.0f, 0.0f);

            var result = LineIntersection.RobustLineIntersectAndResult(a0, a1, b0, b1);

            Assert.False(result.Intersecting);
            Assert.False(result.CoLinear);
            Assert.Equal(0.0f, result.IntersectPoint.X);
            Assert.Equal(0.0f, result.IntersectPoint.Y);
        }

        [Fact]
        public void RobustIntersect_CoLinear_Overlapping_FireOnTrue()
        {
            var a0 = new Vector2(0.0f, 0.0f);
            var a1 = new Vector2(1.0f, 0.0f);

            var b0 = new Vector2(0.5f, 0.0f);
            var b1 = new Vector2(1.5f, 0.0f);

            var result = LineIntersection.RobustLineIntersectAndResult(a0, a1, b0, b1, true);

            Assert.True(result.Intersecting);
            Assert.True(result.CoLinear);
            Assert.Equal(0.0f, result.IntersectPoint.X);
            Assert.Equal(0.0f, result.IntersectPoint.Y);
        }

        [Fact]
        public void RobustIntersect_CoLinear_Overlapping_FireOnFalse()
        {
            var a0 = new Vector2(0.0f, 0.0f);
            var a1 = new Vector2(1.0f, 0.0f);

            var b0 = new Vector2(0.5f, 0.0f);
            var b1 = new Vector2(1.5f, 0.0f);

            var result = LineIntersection.RobustLineIntersectAndResult(a0, a1, b0, b1, false);

            Assert.False(result.Intersecting);
            Assert.True(result.CoLinear);
            Assert.Equal(0.0f, result.IntersectPoint.X);
            Assert.Equal(0.0f, result.IntersectPoint.Y);
        }

        [Fact]
        public void RobustIntersect_CoLinear_NotOverlapping_FireOnTrue()
        {
            var a0 = new Vector2(0.0f, 0.0f);
            var a1 = new Vector2(1.0f, 0.0f);

            var b0 = new Vector2(1.5f, 0.0f);
            var b1 = new Vector2(2.5f, 0.0f);

            var result = LineIntersection.RobustLineIntersectAndResult(a0, a1, b0, b1, true);

            Assert.False(result.Intersecting);
            Assert.True(result.CoLinear);
            Assert.Equal(0.0f, result.IntersectPoint.X);
            Assert.Equal(0.0f, result.IntersectPoint.Y);
        }

        [Fact]
        public void RobustIntersect_Parralell_EnsureNoIntersect()
        {
            var a0 = new Vector2(0.0f, 1.0f);
            var a1 = new Vector2(1.0f, 1.0f);

            var b0 = new Vector2(1.5f, 0.0f);
            var b1 = new Vector2(2.5f, 0.0f);

            var result = LineIntersection.RobustLineIntersectAndResult(a0, a1, b0, b1);

            Assert.False(result.Intersecting);
            Assert.False(result.CoLinear);
            Assert.Equal(0.0f, result.IntersectPoint.X);
            Assert.Equal(0.0f, result.IntersectPoint.Y);
        }

        [Fact]
        public void RobustIntersect_TestingCollisionIfSharePoints()
        {
            var a0 = new Vector2(0.0f, 1.0f);
            var a1 = new Vector2(1.0f, 1.0f);

            var b0 = new Vector2(1.0f, 1.0f);
            var b1 = new Vector2(2.5f, 0.0f);

            var result = LineIntersection.RobustLineIntersectAndResult(a0, a1, b0, b1);

            Assert.True(result.Intersecting);
            Assert.False(result.CoLinear);
            Assert.Equal(1.0f, result.IntersectPoint.X);
            Assert.Equal(1.0f, result.IntersectPoint.Y);
        }

        [Fact]
        public void IsPointOnLineSegment_Confirm()
        {
            var point = new Vector2(1.0f, 1.0f);
            var v0 = new Vector2(-10.0f, 1.0f);
            var v1 = new Vector2(12.0f, 1.0f);

            var result = LineIntersection.IsPointOnLineSegment(point, v0, v1);

            Assert.True(result);
        }

        [Fact]
        public void IsPointOnLineSegment_ConfirmFailsOK()
        {
            var point = new Vector2(1.0f, 10.0f);
            var v0 = new Vector2(-10.0f, 1.0f);
            var v1 = new Vector2(12.0f, 1.0f);

            var result = LineIntersection.IsPointOnLineSegment(point, v0, v1);

            Assert.False(result);
        }

        [Fact]
        public void IsPointProjectedWithinLineSegment_Correct()
        {
            var p = new Vector2(0.0f, 0.0f);
            var p1 = new Vector2(-100.0f, 10.0f);
            var p2 = new Vector2(100.0f, 10.0f);

            var result = LineIntersection.IsPointProjectedWithinLineSegment(p, p1, p2);

            Assert.True(result);
        }

        [Fact]
        public void IsPointProjectedWithinLineSegment_False()
        {
            var p = new Vector2(101.0f, 0.0f);
            var p1 = new Vector2(-100.0f, 10.0f);
            var p2 = new Vector2(100.0f, 10.0f);

            var result = LineIntersection.IsPointProjectedWithinLineSegment(p, p1, p2);

            Assert.False(result);
        }

        [Fact]
        public void TestPerpDistanceFromLine_PositiveRightSide()
        {
            var p = new Vector2(50.0f, 50.0f);
            var dir = new Vector2(0.0f, 1.0f);
            var pointOnRay = new Vector2(0.0f, 0.0f);

            var result = LineIntersection.PerpendicularDistanceOfPointFromInfiniteLineCWNormalIsPositiveDirection(p, dir, pointOnRay);

            Assert.Equal(50.0f, result);
        }

        [Fact]
        public void TestPerpDistanceFromLine_NegativeLeftSide()
        {
            var p = new Vector2(-50.0f, 50.0f);
            var dir = new Vector2(0.0f, 1.0f);
            var pointOnRay = new Vector2(0.0f, 0.0f);

            var result = LineIntersection.PerpendicularDistanceOfPointFromInfiniteLineCWNormalIsPositiveDirection(p, dir, pointOnRay);

            Assert.Equal(-50.0f, result);
        }
    }
}