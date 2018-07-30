using Xunit;
using GeometryTools;
using System.Numerics;

namespace GeometryToolsTest
{
    public class QuadTreeBranchTest
    {
        [Fact]
        public void TestCreation_Correct()
        {
            var aabb = new AABB(Vector2.Zero, new Vector2(50.0f, 50.0f));

            var branch = new QuadBranch<Polygon>(1, 2, aabb);

            Assert.False(branch.isLeaf);
            Assert.Equal(0, branch.numItems);
            Assert.NotNull(branch.Items);
            Assert.Equal(aabb.Left, branch.Aabb.Left);

            var q3 = branch.Branches[2];

            Assert.Equal(-50.0f, q3.Aabb.Left, 5);
            Assert.Equal(0.0f, q3.Aabb.Right, 5);
            Assert.Equal(0.0f, q3.Aabb.Top, 5);
            Assert.Equal(-50.0f, q3.Aabb.Bottom, 5);
        }
    }
}