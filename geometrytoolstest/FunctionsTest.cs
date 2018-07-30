using System;
using Xunit;
using GeometryTools;

namespace GeometryToolsTest
{
    public class FunctionsTest
    {
       //Humouring TDD extremists

       [Fact]
        public void ClampInRange()
        {
            var result = Functions.Clamp0To1(0.5f);

            Assert.True(0.5f == result);
        }

       [Fact]
        public void ClampLow()
        {
            var result = Functions.Clamp0To1(-0.5f);

            Assert.True(0.0f == result);
        }

       [Fact]
        public void ClampHigh()
        {
            var result = Functions.Clamp0To1(1.5f);

            Assert.True(1.0f == result);
        }
    }
}