using Xunit;
using GeometryTools;

using System.Numerics;
using System.Linq;

namespace GeometryToolsTest
{
    public class QuadTreeTest
    {
        [Fact]
        public void TestQuadTreeCreation_BoundsOfRoot()
        {
            var tree = new QuadTree<Polygon>(new System.Numerics.Vector2(100.0f, 100.0f), 7, 14, 20.0f);

            //Expect 16x16 of 20.0 per side, centred at 100,100

            var tAABB = tree.Root.Aabb;

            Assert.Equal(260.0f, tAABB.Top, 5);
            Assert.Equal(-60.0f, tAABB.Bottom, 5);
            Assert.Equal(-60.0f, tAABB.Left, 5);
            Assert.Equal(260.0f, tAABB.Right, 5);
        }

        [Fact]
        public void TestQuadTreeCreation_CatchesSomeErroneousInput()
        {
            var tree = new QuadTree<Polygon>(new System.Numerics.Vector2(0.0f, 0.0f), 0, -14, -20.0f);

            //Expect to catch and make 1x1 tree of 1 per side, centred at 0,0

            var tAABB = tree.Root.Aabb;

            Assert.Equal(1.0f, tAABB.Top, 5);
            Assert.Equal(-1.0f, tAABB.Bottom, 5);
            Assert.Equal(-1.0f, tAABB.Left, 5);
            Assert.Equal(1.0f, tAABB.Right, 5);
        }

        [Fact]
        public void TestInsert_Interface()
        {
            var tree = new QuadTree<Polygon>(new System.Numerics.Vector2(100.0f, 100.0f), 7, 14, 20.0f);

            var poly = Polygon.CreateRegularNSidedPolygon(Vector2.Zero, 6, 10.0f);

            tree.Insert((ITreeItem)poly);

            var index = poly.Index;

            var result = tree.ReturnAll().Select(x => x.Index == index).Count() > 0;

            Assert.True(result);
        }

        [Fact]
        public void TestInsert_Type()
        {
            var tree = new QuadTree<Polygon>(new System.Numerics.Vector2(100.0f, 100.0f), 7, 14, 20.0f);

            var poly = Polygon.CreateRegularNSidedPolygon(Vector2.Zero, 6, 10.0f);

            tree.Insert(poly);

            var index = poly.Index;

            var result = tree.ReturnAll().Select(x => x.Index == index).Count() > 0;

            Assert.True(result);
        }


        [Fact]
        public void TestRemove()
        {
            var tree = new QuadTree<Polygon>(new System.Numerics.Vector2(100.0f, 100.0f), 7, 14, 20.0f);

            var poly = Polygon.CreateRegularNSidedPolygon(Vector2.Zero, 6, 10.0f);

            tree.Insert(poly);

            var index = poly.Index;

            tree.Remove(poly);

            var result = tree.ReturnAll().Select(x => x.Index == index).Count() > 0;

            Assert.False(result);
        }

        [Fact]
        public void TestRemoveAll()
        {
            var tree = new QuadTree<Polygon>(new System.Numerics.Vector2(100.0f, 100.0f), 7, 14, 20.0f);

            var poly0 = Polygon.CreateRegularNSidedPolygon(Vector2.Zero, 6, 10.0f);
            var poly1 = Polygon.CreateRegularNSidedPolygon(Vector2.Zero, 6, 10.0f);
            var poly2 = Polygon.CreateRegularNSidedPolygon(Vector2.Zero, 6, 10.0f);

            tree.Insert(poly0);
            tree.Insert(poly1);
            tree.Insert(poly2);

            tree.RemoveAll();

            var result = tree.ReturnAll().Count() > 0;

            Assert.False(result);
        }

        [Fact]
        public void CheckFirstItemAdded()
        {
            var tree = new QuadTree<Polygon>(new System.Numerics.Vector2(100.0f, 100.0f), 7, 14, 20.0f);

            var poly0 = Polygon.CreateRegularNSidedPolygon(Vector2.Zero, 6, 10.0f);
            var poly1 = Polygon.CreateRegularNSidedPolygon(Vector2.Zero, 6, 10.0f);
            var poly2 = Polygon.CreateRegularNSidedPolygon(Vector2.Zero, 6, 10.0f);

            tree.Insert(poly0);
            tree.Insert(poly1);
            tree.Insert(poly2);

            var first = tree.FirstItemAdded();

            var result = first.Index == poly0.Index;

            Assert.True(result);
        }

        [Fact]
        public void CheckFirstItemAdded_IfRemoveItExpectNull()
        {
            var tree = new QuadTree<Polygon>(new System.Numerics.Vector2(100.0f, 100.0f), 7, 14, 20.0f);

            var poly0 = Polygon.CreateRegularNSidedPolygon(Vector2.Zero, 6, 10.0f);
            var poly1 = Polygon.CreateRegularNSidedPolygon(Vector2.Zero, 6, 10.0f);
            var poly2 = Polygon.CreateRegularNSidedPolygon(Vector2.Zero, 6, 10.0f);

            tree.Insert(poly0);
            tree.Insert(poly1);
            tree.Insert(poly2);

            tree.Remove(poly0);

            var first = tree.FirstItemAdded();

            Assert.Null(first);
        }

        [Fact]
        public void CheckLastItemAdded()
        {
            var tree = new QuadTree<Polygon>(new System.Numerics.Vector2(100.0f, 100.0f), 7, 14, 20.0f);

            var poly0 = Polygon.CreateRegularNSidedPolygon(Vector2.Zero, 6, 10.0f);
            var poly1 = Polygon.CreateRegularNSidedPolygon(Vector2.Zero, 6, 10.0f);
            var poly2 = Polygon.CreateRegularNSidedPolygon(Vector2.Zero, 6, 10.0f);

            tree.Insert(poly0);
            tree.Insert(poly1);
            tree.Insert(poly2);

            var last = tree.LastItemAdded();

            var result = last.Index == poly2.Index;

            Assert.True(result);
        }

        [Fact]
        public void CheckLastItemAdded_IfRemoveItExpectNull()
        {
            var tree = new QuadTree<Polygon>(new System.Numerics.Vector2(100.0f, 100.0f), 7, 14, 20.0f);

            var poly0 = Polygon.CreateRegularNSidedPolygon(Vector2.Zero, 6, 10.0f);
            var poly1 = Polygon.CreateRegularNSidedPolygon(Vector2.Zero, 6, 10.0f);
            var poly2 = Polygon.CreateRegularNSidedPolygon(Vector2.Zero, 6, 10.0f);

            tree.Insert(poly0);
            tree.Insert(poly1);
            tree.Insert(poly2);

            tree.Remove(poly2);

            var last = tree.LastItemAdded();

            Assert.Null(last);
        }

        [Fact]
        public void CheckReturnAll()
        {
            var tree = new QuadTree<Polygon>(new System.Numerics.Vector2(100.0f, 100.0f), 7, 14, 20.0f);

            var poly0 = Polygon.CreateRegularNSidedPolygon(Vector2.Zero, 6, 10.0f);
            var poly1 = Polygon.CreateRegularNSidedPolygon(Vector2.Zero, 6, 10.0f);
            var poly2 = Polygon.CreateRegularNSidedPolygon(Vector2.Zero, 6, 10.0f);

            tree.Insert(poly0);
            tree.Insert(poly1);
            tree.Insert(poly2);

            var all = tree.ReturnAll();

            var result = all.Count == 3;

            Assert.True(result);
        }

        [Fact]
        public void CheckOverlap_MultipleLevelAllHit()
        {
            var tree = new QuadTree<Polygon>(new System.Numerics.Vector2(0.0f, 0.0f), 16, 16, 10.0f);

            var poly0 = Polygon.CreateRegularNSidedPolygon(Vector2.Zero, 6, 10.0f);
            var poly1 = Polygon.CreateRegularNSidedPolygon(Vector2.Zero, 6, 30.0f);
            var poly2 = Polygon.CreateRegularNSidedPolygon(Vector2.Zero, 6, 70.0f);

            tree.Insert(poly0);
            tree.Insert(poly1);
            tree.Insert(poly2);

            var allAABB = tree.Aabb;

            var result = tree.ReturnAllOverlap(allAABB);

            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void CheckOverlap_MultipleLevelHitTwoOutOfFourPartialOverlap()
        {
            var tree = new QuadTree<Polygon>(new System.Numerics.Vector2(0.0f, 0.0f), 16, 16, 10.0f);

            var poly0 = Polygon.CreateRegularNSidedPolygon(new Vector2(-40.0f, +40.0f), 5, 10.0f);
            var poly1 = Polygon.CreateRegularNSidedPolygon(new Vector2(-40.0f, -40.0f), 5, 10.0f);
            var poly2 = Polygon.CreateRegularNSidedPolygon(new Vector2(+40.0f, +40.0f), 5, 10.0f);
            var poly3 = Polygon.CreateRegularNSidedPolygon(new Vector2(+40.0f, +40.0f), 5, 10.0f);

            tree.Insert(poly0);
            tree.Insert(poly1);
            tree.Insert(poly2);
            tree.Insert(poly3);

            var aabb = new AABB(35.0f, -35.0f, -35.0f, -25.0f);

            var result = tree.ReturnAllOverlap(aabb);

            Assert.Equal(2, result.Count);

            var numEqual = result.Select(x => x.Index == poly0.Index || x.Index == poly1.Index).Count();

            Assert.Equal(2, numEqual);
        }

        [Fact]
        public void CheckOverlap_CheckNoOverlap()
        {
            var tree = new QuadTree<Polygon>(new System.Numerics.Vector2(0.0f, 0.0f), 16, 16, 10.0f);

            var poly0 = Polygon.CreateRegularNSidedPolygon(new Vector2(-40.0f, +40.0f), 5, 10.0f);
            var poly1 = Polygon.CreateRegularNSidedPolygon(new Vector2(-40.0f, -40.0f), 5, 10.0f);
          
            tree.Insert(poly0);
            tree.Insert(poly1);

            var aabb = new AABB(35.0f, -35.0f, 25.0f, 35.0f);

            var result = tree.ReturnAllOverlap(aabb);

            Assert.Empty(result);
        }
    }
}