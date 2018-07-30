using Xunit;
using GeometryTools;
using System.Numerics;

namespace GeometryToolsTest
{
    public class AabbTest
    {
        [Fact]
        public void TestContructor_CentreExtents()
        {
            var centre = new Vector2(20.0f, -20.0f);
            var extents = new Vector2(10.0f, 20.0f);

            var aabb = new AABB(centre, extents, true);

            Assert.Equal(20.0f, aabb.Centre.X, 5);
            Assert.Equal(-20.0f, aabb.Centre.Y, 5);
            Assert.Equal(10.0f, aabb.Extents.X, 5);
            Assert.Equal(20.0f, aabb.Extents.Y, 5);
            Assert.Equal(10.0f, aabb.Min.X, 5);
            Assert.Equal(-40.0f, aabb.Min.Y, 5);
            Assert.Equal(30.0f, aabb.Max.X, 5);
            Assert.Equal(0.0f, aabb.Max.Y, 5);
            Assert.Equal(0.0f, aabb.Top, 5);
            Assert.Equal(-40.0f, aabb.Bottom, 5);
            Assert.Equal(10.0f, aabb.Left, 5);
            Assert.Equal(30.0f, aabb.Right, 5);
            Assert.Equal(20.0f, aabb.Width, 5);
            Assert.Equal(40.0f, aabb.Height, 5);
        }

        [Fact]
        public void TestContructor_Aabb()
        {
            //Naughty as using same set up as test above and assuming it passess
            var centre = new Vector2(20.0f, -20.0f);
            var extents = new Vector2(10.0f, 20.0f);

            var aabbConfirmedInLastTest = new AABB(centre, extents, true);
            var aabb = new AABB(aabbConfirmedInLastTest);

            Assert.Equal(20.0f, aabb.Centre.X, 5);
            Assert.Equal(-20.0f, aabb.Centre.Y, 5);
            Assert.Equal(10.0f, aabb.Extents.X, 5);
            Assert.Equal(20.0f, aabb.Extents.Y, 5);
            Assert.Equal(10.0f, aabb.Min.X, 5);
            Assert.Equal(-40.0f, aabb.Min.Y, 5);
            Assert.Equal(30.0f, aabb.Max.X, 5);
            Assert.Equal(0.0f, aabb.Max.Y, 5);
            Assert.Equal(0.0f, aabb.Top, 5);
            Assert.Equal(-40.0f, aabb.Bottom, 5);
            Assert.Equal(10.0f, aabb.Left, 5);
            Assert.Equal(30.0f, aabb.Right, 5);
            Assert.Equal(20.0f, aabb.Width, 5);
            Assert.Equal(40.0f, aabb.Height, 5);
        }

        [Fact]
        public void TestContructor_MinMax()
        {
            var min = new Vector2(10.0f, -40.0f);
            var max = new Vector2(30.0f, 0.0f);

            var aabb = new AABB(min, max, 0, true);

            Assert.Equal(20.0f, aabb.Centre.X, 5);
            Assert.Equal(-20.0f, aabb.Centre.Y, 5);
            Assert.Equal(10.0f, aabb.Extents.X, 5);
            Assert.Equal(20.0f, aabb.Extents.Y, 5);
            Assert.Equal(10.0f, aabb.Min.X, 5);
            Assert.Equal(-40.0f, aabb.Min.Y, 5);
            Assert.Equal(30.0f, aabb.Max.X, 5);
            Assert.Equal(0.0f, aabb.Max.Y, 5);
            Assert.Equal(0.0f, aabb.Top, 5);
            Assert.Equal(-40.0f, aabb.Bottom, 5);
            Assert.Equal(10.0f, aabb.Left, 5);
            Assert.Equal(30.0f, aabb.Right, 5);
            Assert.Equal(20.0f, aabb.Width, 5);
            Assert.Equal(40.0f, aabb.Height, 5);
        }

        [Fact]
        public void TestContructor_TopBottomLeftRight()
        {
            var aabb = new AABB(0.0f, -40.0f, 10.0f, 30.0f, true);

            Assert.Equal(20.0f, aabb.Centre.X, 5);
            Assert.Equal(-20.0f, aabb.Centre.Y, 5);
            Assert.Equal(10.0f, aabb.Extents.X, 5);
            Assert.Equal(20.0f, aabb.Extents.Y, 5);
            Assert.Equal(10.0f, aabb.Min.X, 5);
            Assert.Equal(-40.0f, aabb.Min.Y, 5);
            Assert.Equal(30.0f, aabb.Max.X, 5);
            Assert.Equal(0.0f, aabb.Max.Y, 5);
            Assert.Equal(0.0f, aabb.Top, 5);
            Assert.Equal(-40.0f, aabb.Bottom, 5);
            Assert.Equal(10.0f, aabb.Left, 5);
            Assert.Equal(30.0f, aabb.Right, 5);
            Assert.Equal(20.0f, aabb.Width, 5);
            Assert.Equal(40.0f, aabb.Height, 5);
        }

        [Fact]
        public void TestContructor_CentreExtents_ValidateCatch()
        {
            var centre = new Vector2(20.0f, -20.0f);
            var extents = new Vector2(0.0f, 0.0f);

            var aabb = new AABB(centre, extents, true);

            Assert.Equal(20.0f, aabb.Centre.X, 5);
            Assert.Equal(-20.0f, aabb.Centre.Y, 5);
            Assert.Equal(1.0f, aabb.Extents.X, 5);
            Assert.Equal(1.0f, aabb.Extents.Y, 5);
            Assert.Equal(19.0f, aabb.Min.X, 5);
            Assert.Equal(-21.0f, aabb.Min.Y, 5);
            Assert.Equal(21.0f, aabb.Max.X, 5);
            Assert.Equal(-19.0f, aabb.Max.Y, 5);
            Assert.Equal(-19.0f, aabb.Top, 5);
            Assert.Equal(-21.0f, aabb.Bottom, 5);
            Assert.Equal(19.0f, aabb.Left, 5);
            Assert.Equal(21.0f, aabb.Right, 5);
            Assert.Equal(2.0f, aabb.Width, 5);
            Assert.Equal(2.0f, aabb.Height, 5);
        }

        [Fact]
        public void TestContructor_MinMax_ValidateCatch()
        {
            var max = new Vector2(10.0f, -40.0f);
            var min = new Vector2(30.0f, 0.0f);

            var aabb = new AABB(min, max, 0, true);

            Assert.Equal(20.0f, aabb.Centre.X, 5);
            Assert.Equal(-20.0f, aabb.Centre.Y, 5);
            Assert.Equal(10.0f, aabb.Extents.X, 5);
            Assert.Equal(20.0f, aabb.Extents.Y, 5);
            Assert.Equal(10.0f, aabb.Min.X, 5);
            Assert.Equal(-40.0f, aabb.Min.Y, 5);
            Assert.Equal(30.0f, aabb.Max.X, 5);
            Assert.Equal(0.0f, aabb.Max.Y, 5);
            Assert.Equal(0.0f, aabb.Top, 5);
            Assert.Equal(-40.0f, aabb.Bottom, 5);
            Assert.Equal(10.0f, aabb.Left, 5);
            Assert.Equal(30.0f, aabb.Right, 5);
            Assert.Equal(20.0f, aabb.Width, 5);
            Assert.Equal(40.0f, aabb.Height, 5);
        }

        [Fact]
        public void TestContructor_TopBottomLeftRight_ValidateCatch()
        {
            var aabb = new AABB(-40.0f, 0.0f, 30.0f, 10.0f, true);

            Assert.Equal(20.0f, aabb.Centre.X, 5);
            Assert.Equal(-20.0f, aabb.Centre.Y, 5);
            Assert.Equal(10.0f, aabb.Extents.X, 5);
            Assert.Equal(20.0f, aabb.Extents.Y, 5);
            Assert.Equal(10.0f, aabb.Min.X, 5);
            Assert.Equal(-40.0f, aabb.Min.Y, 5);
            Assert.Equal(30.0f, aabb.Max.X, 5);
            Assert.Equal(0.0f, aabb.Max.Y, 5);
            Assert.Equal(0.0f, aabb.Top, 5);
            Assert.Equal(-40.0f, aabb.Bottom, 5);
            Assert.Equal(10.0f, aabb.Left, 5);
            Assert.Equal(30.0f, aabb.Right, 5);
            Assert.Equal(20.0f, aabb.Width, 5);
            Assert.Equal(40.0f, aabb.Height, 5);
        }

        [Fact]
        public void CheckOverlapStatic_Positive()
        {
            var a = new AABB(new Vector2(0.0f, 0.0f), new Vector2(10.0f, 10.0f));
            var b = new AABB(new Vector2(20.0f, 20.0f), new Vector2(15.0f, 15.0f));

            var result = AABB.CheckOverlap(a, b);

            Assert.True(result);
        }

        [Fact]
        public void CheckOverlapStatic_Negative()
        {
            var a = new AABB(new Vector2(0.0f, 0.0f), new Vector2(10.0f, 10.0f));
            var b = new AABB(new Vector2(30.0f, 30.0f), new Vector2(15.0f, 15.0f));

            var result = AABB.CheckOverlap(a, b);

            Assert.False(result);
        }

        [Fact]
        public void CheckOverlapMember_Positive()
        {
            var a = new AABB(new Vector2(0.0f, 0.0f), new Vector2(10.0f, 10.0f));
            var b = new AABB(new Vector2(20.0f, 20.0f), new Vector2(15.0f, 15.0f));

            var result = a.CheckOverlap(b);

            Assert.True(result);
        }

        [Fact]
        public void CheckOverlapMember_Negative()
        {
            var a = new AABB(new Vector2(0.0f, 0.0f), new Vector2(10.0f, 10.0f));
            var b = new AABB(new Vector2(30.0f, 30.0f), new Vector2(15.0f, 15.0f));

            var result = a.CheckOverlap(b);

            Assert.False(result);
        }

        [Fact]
        public void CheckLineIntersect_Negative()
        {
            var aabb = new AABB(new Vector2(10.0f, 10.0f), new Vector2(10.0f, 10.0f));

            var p0 = new Vector2(-10.0f, -10.0f);
            var p1 = new Vector2(-20.0f, -10.0f);

            var result = AABB.CheckLineIntersect(aabb, p0, p1);

            Assert.False(result);
        }

        [Fact]
        public void CheckLineIntersect_PositiveOnePointInside()
        {
            var aabb = new AABB(new Vector2(10.0f, 10.0f), new Vector2(10.0f, 10.0f));

            var p0 = new Vector2(10.0f, 12.0f);
            var p1 = new Vector2(-20.0f, -10.0f);

            var result = AABB.CheckLineIntersect(aabb, p0, p1);

            Assert.True(result);
        }

        [Fact]
        public void CheckLineIntersect_PositiveTwoPointsInside()
        {
            var aabb = new AABB(new Vector2(10.0f, 10.0f), new Vector2(10.0f, 10.0f));

            var p0 = new Vector2(5.0f, 5.0f);
            var p1 = new Vector2(15.0f, 15.0f);

            var result = AABB.CheckLineIntersect(aabb, p0, p1);

            Assert.True(result);
        }

        [Fact]
        public void CheckLineIntersect_PositiveNoPointsInside()
        {
            var aabb = new AABB(new Vector2(10.0f, 10.0f), new Vector2(10.0f, 10.0f));

            var p0 = new Vector2(10.0f, -10.0f);
            var p1 = new Vector2(10.0f, 100.0f);

            var result = AABB.CheckLineIntersect(aabb, p0, p1);

            Assert.True(result);
        }

        [Fact]
        public void Equality_Positive()
        {
            var a = new AABB(new Vector2(0.0f, 0.0f), new Vector2(10.0f, 10.0f));
            var b = new AABB(new Vector2(0.0f, 0.0f), new Vector2(10.0f, 10.0f));

            var result = AABB.Equality(a, b);

            Assert.True(result);
        }

        [Fact]
        public void Equality_Negative()
        {
            var a = new AABB(new Vector2(0.0f, 0.0f), new Vector2(10.0f, 10.0f));
            var b = new AABB(new Vector2(1.0f, 0.0f), new Vector2(10.0f, 10.0f));

            var result = AABB.Equality(a, b);

            Assert.False(result);
        }

        [Fact]
        public void ContainsVector_Positive()
        {
            var aabb = new AABB(new Vector2(0.0f, 0.0f), new Vector2(10.0f, 10.0f));
            var point = new Vector2(5.0f, 5.0f);

            var result = aabb.Contains(point);

            Assert.True(result);
        }

        [Fact]
        public void ContainsVector_Negative()
        {
            var aabb = new AABB(new Vector2(0.0f, 0.0f), new Vector2(10.0f, 10.0f));
            var point = new Vector2(15.0f, 15.0f);

            var result = aabb.Contains(point);

            Assert.False(result);
        }

        [Fact]
        public void ContainsAABB_Positive()
        {
            var aabb = new AABB(new Vector2(0.0f, 0.0f), new Vector2(10.0f, 10.0f));
            var aabb2 = new AABB(new Vector2(5.0f, 5.0f), new Vector2(2.0f, 2.0f));

            var result = aabb.Contains(aabb2);

            Assert.True(result);
        }

        [Fact]
        public void ContainsAABB_Negative()
        {
            var aabb = new AABB(new Vector2(0.0f, 0.0f), new Vector2(10.0f, 10.0f));
            var aabb2 = new AABB(new Vector2(8.0f, 8.0f), new Vector2(3.0f, 3.0f));

            var result = aabb.Contains(aabb2);

            Assert.False(result);
        }

        private AABB CreateAabbToCheckQs()
        {
            return new AABB(new Vector2(0.0f, 0.0f), new Vector2(50.0f, 50.0f));
        }

        [Fact]
        public void CheckQ1()
        {
            var result = CreateAabbToCheckQs().Q1;

            Assert.Equal(-50.0f, result.Left);
            Assert.Equal(50.0f, result.Top);
            Assert.Equal(0.0f, result.Right);
            Assert.Equal(0.0f, result.Bottom);
        }

        [Fact]
        public void CheckQ2()
        {
            var result = CreateAabbToCheckQs().Q2;

            Assert.Equal(0.0f, result.Left);
            Assert.Equal(50.0f, result.Top);
            Assert.Equal(50.0f, result.Right);
            Assert.Equal(0.0f, result.Bottom);
        }

        [Fact]
        public void CheckQ3()
        {
            var result = CreateAabbToCheckQs().Q3;

            Assert.Equal(-50.0f, result.Left);
            Assert.Equal(0.0f, result.Top);
            Assert.Equal(0.0f, result.Right);
            Assert.Equal(-50.0f, result.Bottom);
        }

        [Fact]
        public void CheckQ4()
        {
            var result = CreateAabbToCheckQs().Q4;

            Assert.Equal(0.0f, result.Left);
            Assert.Equal(0.0f, result.Top);
            Assert.Equal(50.0f, result.Right);
            Assert.Equal(-50.0f, result.Bottom);
        }
    }
}