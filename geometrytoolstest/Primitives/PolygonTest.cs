using System;
using System.Linq;
using System.Numerics;
using Xunit;
using GeometryTools;

namespace GeometryToolsTest
{
    public class PolygonTest
    {
       [Fact]
        public void TestRegularNSidedPolygonCreation()
        {
            var poly = Polygon.CreateRegularNSidedPolygon(new System.Numerics.Vector2(5.0f, 10.0f), 4, 30.0f);

            //Check a selection of properties cover most of the polygon
            Assert.Equal(4, poly.NumVertices);
            Assert.Equal(5.0f, poly.Vertices[2].X, 5);
            Assert.Equal(-20.0f, poly.Vertices[2].Y, 5);
            var rootHalf = (float)Math.Sqrt(0.5f);
            Assert.Equal(-rootHalf, poly.EdgeNormals[1].Y, 5);
            Assert.Equal(-rootHalf, poly.EdgeTangents[1].X, 5);
            Assert.Equal(rootHalf * 60.0f, poly.EdgeLengths[1], 5);
            Assert.Equal(60.0f, poly.Aabb.Width, 5);
        }

       [Fact]
        public void TestStandardPolygonConstructor()
        {
            var poly = CreateStandardBoxPolygon();

            //Check a selection of properties cover most of the polygon
            Assert.Equal(4, poly.NumVertices);
            Assert.Equal(50.0f, poly.Vertices[2].X, 5);
            Assert.Equal(-50.0f, poly.Vertices[2].Y, 5);
            Assert.Equal(1.0f, poly.EdgeNormals[1].X, 5);
            Assert.Equal(-1.0f, poly.EdgeTangents[1].Y, 5);
            Assert.Equal(100.0f, poly.EdgeLengths[1], 5);
            Assert.Equal(100.0f, poly.Aabb.Width, 5);
            Assert.Equal(0.0f, poly.Centre.X, 5);
        }

        private Polygon CreateStandardBoxPolygon()
        {
            var verts = new Vector2[]
            {
                new Vector2(-50.0f, 50.0f),
                new Vector2(50.0f, 50.0f),
                new Vector2(50.0f, -50.0f),
                new Vector2(-50.0f, -50.0f)
            };
            return new Polygon(verts);
        }

       [Fact]
        public void TestInsertInTree()
        {
            ITree tree = new QuadTree<Polygon>(new Vector2(-10.0f, -10.0f), 2, 2, 10);

            var poly = CreateStandardBoxPolygon();

            poly.InsertInTree(tree);

            var id = poly.Index;

            Assert.True(tree.ReturnAll().Select(x => x.Index).Where(x => x == id).Count() > 0);
        }

       [Fact]
        public void TestRemoveFromTree()
        {
            ITree tree = new QuadTree<Polygon>(new Vector2(-10.0f, -10.0f), 2, 2, 10);

            var poly = CreateStandardBoxPolygon();

            poly.InsertInTree(tree);

            var id = poly.Index;

            poly.RemoveFromTree();

            Assert.False(tree.ReturnAll().Select(x => x.Index).Where(x => x == id).Count() > 0);
        }

       [Fact]
        public void TestRotateClockwise()
        {
            var poly = CreateStandardBoxPolygon();

            poly.RotateClockwise(90.0f, AngleType.Degrees);

            Assert.Equal(50.0f, poly.Vertices[0].X, 1);
            Assert.Equal(50.0f, poly.Vertices[0].Y, 1);

            poly.RotateClockwise(90.0f * Constants.DegToRads, AngleType.Radians);

            Assert.Equal(50.0f, poly.Vertices[0].X, 1);
            Assert.Equal(-50.0f, poly.Vertices[0].Y, 1);
        }

       [Fact]
        public void TestTranslate()
        {
            var poly = CreateStandardBoxPolygon();

            poly.Translate(new Vector2(10.0f, 5.0f));

            Assert.Equal(10.0f, poly.Centre.X);
            Assert.Equal(55.0f, poly.Vertices[1].Y);
        }

       [Fact]
        public void TestSetPosition()
        {
            var poly = CreateStandardBoxPolygon();

            poly.SetPosition(new Vector2(100.0f, 50.0f));

            Assert.Equal(100.0f, poly.Centre.X);
            Assert.Equal(100.0f, poly.Vertices[1].Y);
        }
    }
}