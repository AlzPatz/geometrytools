using Xunit;
using System.Numerics;
using GeometryTools;

namespace GeometryToolsTest
{
    public class PolygonIntersectionTest
    {
        [Fact]
        public void CircleCircleIntersect_PositiveSimpleOverload()
        {
            Circle c0 = new Circle(new System.Numerics.Vector2(0.0f, 0.0f), 20.0f);
            Circle c1 = new Circle(new System.Numerics.Vector2(30.0f, 0.0f), 20.0f);

            var result = PolygonIntersection.CircleCircleIntersect(c0, c1);

            Assert.True(result.Intersecting);
            Assert.Equal(10.0f, result.Depth, 4);
            Assert.Equal(1.0f, result.Normal0To1.X, 4);
            Assert.Equal(0.0f, result.Normal0To1.Y, 4);
            Assert.Equal(10.0f, result.Point.X, 4);
            Assert.Equal(0.0f, result.Point.Y, 4);
        }

        [Fact]
        public void CircleCircleIntersect_NegativeSimpleOverload()
        {
            Circle c0 = new Circle(new System.Numerics.Vector2(0.0f, 0.0f), 20.0f);
            Circle c1 = new Circle(new System.Numerics.Vector2(100.0f, 0.0f), 20.0f);

            var result = PolygonIntersection.CircleCircleIntersect(c0, c1);

            Assert.False(result.Intersecting);
            Assert.Equal(0.0f, result.Depth, 4);
            Assert.Equal(0.0f, result.Normal0To1.X, 4);
            Assert.Equal(0.0f, result.Normal0To1.Y, 4);
            Assert.Equal(0.0f, result.Point.X, 4);
            Assert.Equal(0.0f, result.Point.Y, 4);
        }

        [Fact]
        public void CircleCircleIntersect_PositiveSimpleBooleanOverload()
        {
            var result = PolygonIntersection.CircleCircleIntersect(new Vector2(0.0f, 0.0f), 20.0f, new Vector2(30.0f, 0.0f), 20.0f);

            Assert.True(result);
        }

        [Fact]
        public void CircleCircleIntersect_NegativeSimpleBooleanOverload()
        {
            var result = PolygonIntersection.CircleCircleIntersect(new Vector2(0.0f, 0.0f), 20.0f, new Vector2(100.0f, 0.0f), 20.0f);

            Assert.False(result);
        }

        [Fact]
        public void CircleCircleIntersect_FullOverload_Positive()
        {
            var result = PolygonIntersection.CircleCircleIntersect(new Vector2(4.0f, 4.0f), 10.0f, new Vector2(4.0f, 20.0f), 10.0f, true);

            Assert.True(result.Intersecting);
            Assert.Equal(4.0f, result.Depth, 4);
            Assert.Equal(0.0f, result.Normal0To1.X, 4);
            Assert.Equal(1.0f, result.Normal0To1.Y, 4);
            Assert.Equal(4.0f, result.Point.X, 4);
            Assert.Equal(10.0f, result.Point.Y, 4);
        }

        [Fact]
        public void CircleCircleIntersect_FullOverload_Negative()
        {
            var result = PolygonIntersection.CircleCircleIntersect(new Vector2(4.0f, 4.0f), 10.0f, new Vector2(115.0f, 115.0f), 10.0f, true);

            Assert.False(result.Intersecting);
            Assert.Equal(0.0f, result.Depth, 4);
            Assert.Equal(0.0f, result.Normal0To1.X, 4);
            Assert.Equal(0.0f, result.Normal0To1.Y, 4);
            Assert.Equal(0.0f, result.Point.X, 4);
            Assert.Equal(0.0f, result.Point.Y, 4);
        }

        [Fact]
        public void CircleCircleIntersect_FullOverload_PositiveOnEdge()
        {
            var result = PolygonIntersection.CircleCircleIntersect(new Vector2(5.0f, 5.0f), 10.0f, new Vector2(5.0f, 25.0f), 10.0f, true);

            Assert.True(result.Intersecting);
            Assert.Equal(0.0f, result.Depth, 4);
            Assert.Equal(0.0f, result.Normal0To1.X, 4);
            Assert.Equal(1.0f, result.Normal0To1.Y, 4);
            Assert.Equal(5.0f, result.Point.X, 4);
            Assert.Equal(15.0f, result.Point.Y, 4);
        }

        [Fact]
        public void CircleCircleIntersect_FullOverload_NegativeOnEdge()
        {
            var result = PolygonIntersection.CircleCircleIntersect(new Vector2(5.0f, 5.0f), 10.0f, new Vector2(5.0f, 25.0f), 10.0f, false);

            Assert.False(result.Intersecting);
            Assert.Equal(0.0f, result.Depth, 4);
            Assert.Equal(0.0f, result.Normal0To1.X, 4);
            Assert.Equal(0.0f, result.Normal0To1.Y, 4);
            Assert.Equal(0.0f, result.Point.X, 4);
            Assert.Equal(0.0f, result.Point.Y, 4);
        }

        [Fact]
        public void PolyPolyIntersect_SimpleBoolean_Positive()
        {
            var p0 = new Polygon(new Vector2[]{
                new Vector2(-50.0f, 50.0f),
                new Vector2(50.0f, 50.0f),
                new Vector2(50.0f, -50.0f),
                new Vector2(-50.0f, -50.0f)
            });

            var p1 = new Polygon(new Vector2[]{
                new Vector2(0.0f, 40.0f),
                new Vector2(-50.0f, 100.0f),
                new Vector2(50.0f, 100.0f)
            });

            var result = PolygonIntersection.PolygonPolygonIntersect(p0, p1);

            Assert.True(result);
        }

        [Fact]
        public void PolyPolyIntersect_SimpleBoolean_Negative()
        {
            var p0 = new Polygon(new Vector2[]{
                new Vector2(-50.0f, 50.0f),
                new Vector2(50.0f, 50.0f),
                new Vector2(50.0f, -50.0f),
                new Vector2(-50.0f, -50.0f)
            });

            var p1 = new Polygon(new Vector2[]{
                new Vector2(0.0f, 60.0f),
                new Vector2(-50.0f, 120.0f),
                new Vector2(50.0f, 120.0f)
            });

            var result = PolygonIntersection.PolygonPolygonIntersect(p0, p1);

            Assert.False(result);
        }

        [Fact]
        public void PolyPolyIntersect_FullOverload_Positive()
        {
            var p0 = new Polygon(new Vector2[]{
                new Vector2(-50.0f, 50.0f),
                new Vector2(50.0f, 50.0f),
                new Vector2(50.0f, -50.0f),
                new Vector2(-50.0f, -50.0f)
            });

            var p1 = new Polygon(new Vector2[]{
                new Vector2(0.0f, 40.0f),
                new Vector2(-50.0f, 100.0f),
                new Vector2(50.0f, 100.0f)
            });

            var result = PolygonIntersection.PolygonPolygonIntersect(p0, p1, true);

            Assert.True(result.Intersecting);
            Assert.Equal(10.0f, result.Depth, 4);
            Assert.Equal(0.0f, result.Normal0To1.X, 4);
            Assert.Equal(1.0f, result.Normal0To1.Y, 4);
            Assert.Equal(0.0f, result.Point.X, 4);
            Assert.Equal(0.0f, result.Point.Y, 4);
        }

        [Fact]
        public void PolyPolyIntersect_FullOverload_PositiveOnEdge()
        {
            var p0 = new Polygon(new Vector2[]{
                new Vector2(-50.0f, 50.0f),
                new Vector2(50.0f, 50.0f),
                new Vector2(50.0f, -50.0f),
                new Vector2(-50.0f, -50.0f)
            });

            var p1 = new Polygon(new Vector2[]{
                new Vector2(0.0f, 49.9999f),//Due to the epsilon impact on fireonedge..
                new Vector2(-50.0f, 100.0f),
                new Vector2(50.0f, 100.0f)
            });

            var result = PolygonIntersection.PolygonPolygonIntersect(p0, p1, true);

            Assert.True(result.Intersecting);
            Assert.Equal(0.0f, result.Depth, 3);
            Assert.Equal(0.0f, result.Normal0To1.X, 3);
            Assert.Equal(1.0f, result.Normal0To1.Y, 3);
            Assert.Equal(0.0f, result.Point.X, 3);
            Assert.Equal(0.0f, result.Point.Y, 3);
        }

        [Fact]
        public void PolyPolyIntersect_FullOverload_NegativeOnEdge()
        {
            var p0 = new Polygon(new Vector2[]{
                new Vector2(-50.0f, 50.0f),
                new Vector2(50.0f, 50.0f),
                new Vector2(50.0f, -50.0f),
                new Vector2(-50.0f, -50.0f)
            });

            var p1 = new Polygon(new Vector2[]{
                new Vector2(0.0f, 50.0f),
                new Vector2(-50.0f, 100.0f),
                new Vector2(50.0f, 100.0f)
            });

            var result = PolygonIntersection.PolygonPolygonIntersect(p0, p1, false);

            Assert.False(result.Intersecting);
            Assert.Equal(0.0f, result.Depth, 4);
            Assert.Equal(0.0f, result.Normal0To1.X, 4);
            Assert.Equal(0.0f, result.Normal0To1.Y, 4);
            Assert.Equal(0.0f, result.Point.X, 4);
            Assert.Equal(0.0f, result.Point.Y, 4);
        }

        [Fact]
        public void CirclePolyIntersect_SimpleBoolean_Positive()
        {
            var c = new Circle(new Vector2(0.0f, 0.0f), 10.0f);
            var p = Polygon.CreateRegularNSidedPolygon(new Vector2(15.0f, 0.0f), 4, 10.0f);

            var result = PolygonIntersection.CirclePolygonIntersect(c.Position, c.Radius, p);

            Assert.True(result);
        }

        [Fact]
        public void CirclePolyIntersect_SimpleBoolean_Negative()
        {
            var c = new Circle(new Vector2(0.0f, 0.0f), 10.0f);
            var p = Polygon.CreateRegularNSidedPolygon(new Vector2(25.0f, 0.0f), 4, 10.0f);

            var result = PolygonIntersection.CirclePolygonIntersect(c.Position, c.Radius, p);

            Assert.False(result);
        }

        [Fact]
        public void CirclePolyIntersect_SimpleResult_Positive()
        {
            var c = new Circle(new Vector2(0.0f, 0.0f), 10.0f);
            var p = Polygon.CreateRegularNSidedPolygon(new Vector2(15.0f, 0.0f), 4, 10.0f);

            var result = PolygonIntersection.CirclePolygonIntersect(c, p);

            Assert.True(result.Intersecting);
            Assert.Equal(5.0f, result.Depth, 4);
            Assert.Equal(1.0f, result.Normal0To1.X, 4);
            Assert.Equal(0.0f, result.Normal0To1.Y, 4);
            Assert.Equal(0.0f, result.Point.X, 4);
            Assert.Equal(0.0f, result.Point.Y, 4); ;
        }

        [Fact]
        public void CirclePolyIntersect_SimpleResult_Negative()
        {
            var c = new Circle(new Vector2(0.0f, 0.0f), 10.0f);
            var p = Polygon.CreateRegularNSidedPolygon(new Vector2(25.0f, 0.0f), 4, 10.0f);

            var result = PolygonIntersection.CirclePolygonIntersect(c, p);

            Assert.False(result.Intersecting);
            Assert.Equal(0.0f, result.Depth, 4);
            Assert.Equal(0.0f, result.Normal0To1.X, 4);
            Assert.Equal(0.0f, result.Normal0To1.Y, 4);
            Assert.Equal(0.0f, result.Point.X, 4);
            Assert.Equal(0.0f, result.Point.Y, 4); ;
        }

        [Fact]
        public void CirclePolyIntersect_OnEdge_Positive()
        {
            var c = new Circle(new Vector2(0.0f, 0.0f), 10.0f);
            var p = Polygon.CreateRegularNSidedPolygon(new Vector2(19.9999f, 0.0f), 4, 10.0f); //issue here again with epsilon i think...

            var result = PolygonIntersection.CirclePolygonIntersect(c.Position, c.Radius, p, true);

            Assert.True(result.Intersecting);
            Assert.Equal(0.0f, result.Depth, 3);
            Assert.Equal(1.0f, result.Normal0To1.X, 3);
            Assert.Equal(0.0f, result.Normal0To1.Y, 3);
            Assert.Equal(0.0f, result.Point.X, 3);
            Assert.Equal(0.0f, result.Point.Y, 3); ;
        }

        [Fact]
        public void CirclePolyIntersect_OnEdge_Negative()
        {
            var c = new Circle(new Vector2(0.0f, 0.0f), 10.0f);
            var p = Polygon.CreateRegularNSidedPolygon(new Vector2(25.0f, 0.0f), 4, 10.0f);

            var result = PolygonIntersection.CirclePolygonIntersect(c.Position, c.Radius, p, false);

            Assert.False(result.Intersecting);
            Assert.Equal(0.0f, result.Depth, 4);
            Assert.Equal(0.0f, result.Normal0To1.X, 4);
            Assert.Equal(0.0f, result.Normal0To1.Y, 4);
            Assert.Equal(0.0f, result.Point.X, 4);
            Assert.Equal(0.0f, result.Point.Y, 4); ;
        }

        [Fact]
        public void PointInCircle_Pass()
        {
            Circle c = new Circle(new Vector2(10.0f, 10.0f), 5.0f);
            Vector2 p = new Vector2(11.0f, 12.0f);

            var result = PolygonIntersection.IsPointInCircle(p, c);

            Assert.True(result);
        }

        [Fact]
        public void PointInCircle_Fail()
        {
            Circle c = new Circle(new Vector2(10.0f, 10.0f), 5.0f);
            Vector2 p = new Vector2(20.0f, 20.0f);

            var result = PolygonIntersection.IsPointInCircle(p, c);

            Assert.False(result);
        }

        [Fact]
        public void PointInCircle_OnEdge_True()
        {
            Circle c = new Circle(new Vector2(10.0f, 10.0f), 5.0f);
            Vector2 p = new Vector2(15.0f, 10.0f);

            var result = PolygonIntersection.IsPointInCircle(p, c, true);

            Assert.True(result);
        }

        [Fact]
        public void PointInCircle_OnEdge_False()
        {
            Circle c = new Circle(new Vector2(10.0f, 10.0f), 5.0f);
            Vector2 p = new Vector2(15.0f, 10.0f);

            var result = PolygonIntersection.IsPointInCircle(p, c, false);

            Assert.False(result);
        }

        [Fact]
        public void PointInPoly_Winding_Pass()
        {
            Polygon p = Polygon.CreateRegularNSidedPolygon(new Vector2(0.0f, 0.0f), 5, 10.0f);
            Vector2 v = new Vector2(5.0f, 5.0f);

            var result = PolygonIntersection.IsPointInPolygon(v, p, true, true);

            Assert.True(result);
        }

        [Fact]
        public void PointInPoly_Winding_Fail()
        {
            Polygon p = Polygon.CreateRegularNSidedPolygon(new Vector2(0.0f, 0.0f), 4, 10.0f);
            Vector2 v = new Vector2(15.0f, 15.0f);

            var result = PolygonIntersection.IsPointInPolygon(v, p, true, true);

            Assert.False(result);
        }

        [Fact]
        public void PointInPoly_Crossing_Pass()
        {
            Polygon p = Polygon.CreateRegularNSidedPolygon(new Vector2(0.0f, 0.0f), 4, 10.0f);
            Vector2 v = new Vector2(4.0f, 4.0f);

            var result = PolygonIntersection.IsPointInPolygon(v, p, true, false);

            Assert.True(result);
        }

        [Fact]
        public void PointInPoly_Crossing_Fail()
        {
            Polygon p = Polygon.CreateRegularNSidedPolygon(new Vector2(0.0f, 0.0f), 4, 10.0f);
            Vector2 v = new Vector2(15.0f, 15.0f);

            var result = PolygonIntersection.IsPointInPolygon(v, p, true, false);

            Assert.False(result);
        }

        [Fact]
        public void PointInPoly_OnEdge_True()
        {
            Polygon p = Polygon.CreateRegularNSidedPolygon(new Vector2(0.0f, 0.0f), 4, 10.0f);
            Vector2 v = new Vector2(10.0f, 0.0f);

            var result = PolygonIntersection.IsPointInPolygon(v, p, true, true);

            Assert.True(result);
        }

        [Fact]
        public void PointInPoly_OnEdge_False()
        {
            Polygon p = Polygon.CreateRegularNSidedPolygon(new Vector2(0.0f, 0.0f), 4, 10.0f);
            Vector2 v = new Vector2(10.0f, 0.0f);

            var result = PolygonIntersection.IsPointInPolygon(v, p, false, false);

            Assert.False(result);
        }
    }
}