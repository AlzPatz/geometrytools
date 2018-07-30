using Xunit;
using GeometryTools;
using System.Numerics;

namespace GeometryToolsTest
{
    public class BiarcTest
    {
        private Biarc CreateBasicCurve()
        {
            var p0 = new Vector2(-50.0f, 0.0f);
            var p1 = new Vector2(50.0f, 0.0f);

            var t0 = Vector2Ext.Up;
            var t1 = Vector2Ext.Down;

            return new Biarc(p0, t0, p1, t1);
        }

        [Fact]
        public void BiarcTestCurve_Start()
        {
            var curve = CreateBasicCurve();

            var point = curve.ReturnPointOnCurve(0.0f);

            Assert.Equal(-50.0f, point.X, 5);
            Assert.Equal(0.0f, point.Y, 5);
        }

        [Fact]
        public void BiarcTestCurve_End()
        {
            var curve = CreateBasicCurve();

            var point = curve.ReturnPointOnCurve(1.0f);

            Assert.Equal(50.0f, point.X, 5);
            Assert.Equal(0.0f, point.Y, 5);
        }

        [Fact]
        public void BiarcTestCurve_TooHighFraction()
        {
            var curve = CreateBasicCurve();

            var point = curve.ReturnPointOnCurve(1.5f);

            Assert.Equal(50.0f, point.X, 5);
            Assert.Equal(0.0f, point.Y, 5);
        }

        [Fact]
        public void BiarcTestCurve_Middle()
        {
            var curve = CreateBasicCurve();

            var point = curve.ReturnPointOnCurve(0.5f);

            Assert.Equal(0.0f, point.X, 5);
            Assert.Equal(50.0f, point.Y, 5);
        }

        [Fact]
        public void BiarcTestCurveRightAngleAtMiddle()
        {
            var curve = CreateBasicCurve();

            var point = curve.ReturnRightUnitVectorOnCurve(0.5f);

            Assert.Equal(0.0f, point.X, 5);
            Assert.Equal(-1.0f, point.Y, 5);
        }

        [Fact]
        public void BiarcTestUpdateParams()
        {
            var p0 = new Vector2(-50.0f, 0.0f);
            var p1 = new Vector2(50.0f, 0.0f);

            var t0 = Vector2Ext.Up;
            var t1 = Vector2Ext.Down;

            var curve = new Biarc(new Vector2(0.0f, 10.0f), new Vector2(0.0f, 1.0f), new Vector2(50.0f, 1.0f), new Vector2(0.0f, 1.0f));

            curve.UpdateParamaters(p0, t0, p1, t1);

            var point = curve.ReturnPointOnCurve(0.5f);

            Assert.Equal(0.0f, point.X, 5);
            Assert.Equal(50.0f, point.Y, 5);
        }

    }
}