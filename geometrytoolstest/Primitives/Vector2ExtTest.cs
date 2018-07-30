using System;
using System.Numerics;
using Xunit;
using GeometryTools;

namespace GeometryToolsTest
{
    public class Vector2ExtTest
    {
       [Fact]
        public void StandardEqualsTestEqual()
        {
            var v0 = new Vector2(1.0f, 1.0f);
            var v1 = new Vector2(1.0f, 1.0f);

            var result = v0 == v1;

            Assert.True(result);
        }

       [Fact]
        public void StandardEqualsTestNotEqual()
        {
            var v0 = new Vector2(1.0f, 1.0f);
            var v1 = new Vector2(2.0f, 1.0f);

            var result = v0 == v1;

            Assert.False(result);
        }

       [Fact]
        public void EqualityTestEqual()
        {
            var v0 = new Vector2(1.0f, 1.0f);
            var v1 = new Vector2(1.0f, 1.0f);

            var result = GeometryTools.Vector2Ext.Equality(v0, v1);

            Assert.True(result);
        }

       [Fact]
        public void EqualityNotEqual()
        {
            var v0 = new Vector2(1.0f, 1.0f);
            var v1 = new Vector2(2.0f, 1.0f);

            var result = GeometryTools.Vector2Ext.Equality(v0, v1);

            Assert.False(result);
        }

       [Fact]
        public void EqualityTestWithinTolerance()
        {
            var tolerance = 0.002f;

            var v0 = new Vector2(1.0f, 1.0f);
            var v1 = new Vector2(1.001f, 1.001f);

            var result = GeometryTools.Vector2Ext.Equality(v0, v1, tolerance);

            Assert.True(result);
        }

       [Fact]
        public void EqualityTestOutOfTolerance()
        {
            var tolerance = 0.00005f;

            var v0 = new Vector2(1.0f, 1.0f);
            var v1 = new Vector2(1.0001f, 1.0001f);

            var result = GeometryTools.Vector2Ext.Equality(v0, v1, tolerance);

            Assert.False(result);
        }

       [Fact]
        public void NormalWithZeroLengthVector()
        {
            Vector2 vec = new Vector2(0.0f, 0.0f);

            var normal = vec.Normal();

            Assert.Equal(0.0f, normal.X, 5);
            Assert.Equal(0.0f, normal.Y, 5);
        }

       [Fact]
        public void CheckNormalCalculationWorks()
        {
            Vector2 vec = new Vector2(-10.0f, -10.0f);

            var normal = vec.Normal();

            var expected = -(float)Math.Sqrt(0.5f);

            Assert.Equal(expected, normal.X, 5);
            Assert.Equal(expected, normal.Y, 5);
        }

       [Fact]
        public void DotProductTest()
        {
            Vector2 v0 = new Vector2(1.0f, 0.0f);
            Vector2 v1 = new Vector2(2.0f, 0.0f);

            var result = GeometryTools.Vector2Ext.Dot(v0, v1);

            Assert.Equal(2.0f, result);
        }

       [Fact]
        public void DotProductTestPerp()
        {
            Vector2 v0 = new Vector2(1.0f, 0.0f);
            Vector2 v1 = new Vector2(0.0f, 1.0f);

            var result = GeometryTools.Vector2Ext.Dot(v0, v1);

            Assert.Equal(0.0f, result);
        }

       [Fact]
        public void DotWithProductTest()
        {
            Vector2 v0 = new Vector2(1.0f, 0.0f);
            Vector2 v1 = new Vector2(2.0f, 0.0f);

            var result = v0.DotWith(v1); ;

            Assert.Equal(2.0f, result);
        }

       [Fact]
        public void DotWithProductTestPerp()
        {
            Vector2 v0 = new Vector2(1.0f, 0.0f);
            Vector2 v1 = new Vector2(0.0f, 1.0f);

            var result = v0.DotWith(v1);

            Assert.Equal(0.0f, result);
        }

       [Fact]
        public void CrossProductTest()
        {
            Vector2 v0 = new Vector2(1.0f, -2.0f);
            Vector2 v1 = new Vector2(2.0f, 5.0f);

            var result = GeometryTools.Vector2Ext.Cross(v0, v1);

            Assert.Equal(9.0f, result);
        }

       [Fact]
        public void CrossProductSelfTest()
        {
            Vector2 v0 = new Vector2(1.0f, -2.0f);
            Vector2 v1 = new Vector2(2.0f, 5.0f);

            var result = v0.CrossWith(v1);

            Assert.Equal(9.0f, result);
        }

       [Fact]
        public void CreateUnitInDirectionTest()
        {
            Vector2 v0 = new Vector2(0.0f, 0.0f);
            Vector2 v1 = new Vector2(10.0f, -10.0f);

            var result = Vector2Ext.UnitInDirection(v0, v1);

            var expected = (float)Math.Sqrt(0.5f);

            Assert.Equal(expected, result.X, 5);
            Assert.Equal(-expected, result.Y, 5);
        }

       [Fact]
        public void CreateUnitInDirectionTestEqual()
        {
            Vector2 v0 = new Vector2(10.0f, 10.0f);
            Vector2 v1 = new Vector2(10.0f, 10.0f);

            var result = Vector2Ext.UnitInDirection(v0, v1);

            Assert.Equal(0.0f, result.X);
            Assert.Equal(0.0f, result.Y);
        }

       [Fact]
        public void LengthBetweenTwoPoints()
        {
            Vector2 v0 = new Vector2(0.0f, 0.0f);
            Vector2 v1 = new Vector2(10.0f, 0.0f);

            var result = Vector2Ext.DistanceBetweenTwoPoints(v0, v1);

            Assert.Equal(10.0f, result);
        }

       [Fact]
        public void RotationCWFromUpDegrees()
        {
            Vector2 v = new Vector2(12.0f, 12.0f);

            var result = v.RotationClockwiseFromUpInDegrees();

            Assert.Equal(45.0f, result);
        }

       [Fact]
        public void RotationCWFromUpRadians()
        {
            Vector2 v = new Vector2(0.0f, -12.0f);

            var result = v.RotationClockwiseFromUpInRadians();

            Assert.Equal(Math.PI, result, 5);
        }

       [Fact]
        public void ReturnClockWiseRight()
        {
            Vector2 v = new Vector2(10.0f, 0.0f);

            var result = v.RotateClockwiseRight();

            Assert.Equal(0.0f, result.X);
            Assert.Equal(-10.0f, result.Y); ;
        }

       [Fact]
        public void ReturnRotateClockwiseDegrees()
        {
            Vector2 v = new Vector2(-20.0f, 0.0f);

            var result = v.RotateClockwiseDegrees(90.0f);

            Assert.Equal(0.0f, result.X, 5);
            Assert.Equal(20.0f, result.Y, 5);
        }

       [Fact]
        public void ReturnRotateClockwiseRadians()
        {
            Vector2 v = new Vector2(-20.0f, 0.0f);

            var result = v.RotateClockwiseRadians(0.5f * (float)Math.PI);

            Assert.Equal(0.0f, result.X, 5);
            Assert.Equal(20.0f, result.Y, 5);
        }

       [Fact]
        public void TestReflectAcrossAxis()
        {
            Vector2 v = new Vector2(0.0f, 1.0f);
            Vector2 axis = new Vector2(45.0f, 45.0f);

            var result = v.ReflectAcrossAxis(axis);

            Assert.Equal(1.0f, result.X, 5);
            Assert.Equal(0.0f, result.Y, 5);
        }

       [Fact]
        public void TestReflectByNormal()
        {
            Vector2 v = new Vector2(1.0f, 1.0f);
            Vector2 normal = new Vector2(0.0f, -1.0f);

            var result = v.ReflectByNormal(normal);

            Assert.Equal(1.0f, result.X, 5);
            Assert.Equal(-1.0f, result.Y, 5);
        }

       [Fact]
        public void TestReflectByNormalNegativeDirectionOfSurfaceFaceCheck()
        {
            Vector2 v = new Vector2(1.0f, 1.0f);
            Vector2 normal = new Vector2(0.0f, 1.0f);

            var result = v.ReflectByNormal(normal);

            Assert.Equal(1.0f, result.X, 5);
            Assert.Equal(-1.0f, result.Y, 5);
        }
    }
}