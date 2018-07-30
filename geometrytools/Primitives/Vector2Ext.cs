using System;
using System.Numerics;

namespace GeometryTools
{
    public static class Vector2Ext
    {
        //Vector2 is struct and has SIMD support. Hence I ditched my won Vector Implementation
        public static Vector2 Up { get { return Vector2.UnitY; } }
        public static Vector2 Down { get { return -Vector2.UnitY; } }
        public static Vector2 Left { get { return -Vector2.UnitX; } }
        public static Vector2 Right { get { return Vector2.UnitX; } }
        public static Vector2 Zero { get { return Vector2.Zero; } }

        public static bool Equality(Vector2 v0, Vector2 v1, float epsilon = Constants.Epsilon)
        {
            return Math.Abs(v1.X - v0.X) <= epsilon && Math.Abs(v1.Y - v0.Y) <= epsilon;
        }

        public static Vector2 Normal(this Vector2 vec)
        {
            if (vec.Length() == 0.0f)
                return vec;

            return Vector2.Normalize(vec);
        }

        public static float DotWith(this Vector2 v0, Vector2 v1)
        {
            return Vector2Ext.Dot(v0, v1);
        }

        public static float Dot(Vector2 v0, Vector2 v1)
        {
            return Vector2.Dot(v0, v1);
        }

        public static float CrossWith(this Vector2 v0, Vector2 v1)
        {
            return Cross(v0, v1);
        }

        public static float Cross(Vector2 v0, Vector2 v1)
        {
            return (v0.X * v1.Y) - (v0.Y * v1.X);
        }

        public static Vector2 UnitInDirection(Vector2 from, Vector2 too)
        {
            if (Vector2Ext.Equality(from, too))
                return Vector2.Zero;
            return (too - from).Normal();
        }

        public static float DistanceBetweenTwoPoints(Vector2 v0, Vector2 v1)
        {
            return Vector2.Distance(v0, v1);
        }

        public static float RotationClockwiseFromUpInDegrees(this Vector2 vec)
        {
            return vec.RotationClockwiseFromUpInRadians() / Constants.DegToRads;
        }

        public static float RotationClockwiseFromUpInRadians(this Vector2 vec)
        {
            var angleFromHori = (float)Math.Atan2(vec.Y, vec.X);
            var angleFromVert = angleFromHori - (0.5f * (float)Math.PI);
            return -angleFromVert;
        }

        public static Vector2 RotateClockwiseRight(this Vector2 vec)
        {
            return new Vector2(vec.Y, -vec.X);
        }

        public static Vector2 RotateClockwiseDegrees(this Vector2 vec, float angle)
        {
            return vec.RotateClockwiseRadians(angle * Constants.DegToRads);
        }

        public static Vector2 RotateClockwiseRadians(this Vector2 vec, float angle)
        {
            return Vector2.Transform(vec, Quaternion.CreateFromAxisAngle(Vector3.UnitZ, -angle));
        }

        public static Vector2 ReflectAcrossAxis(this Vector2 vec, Vector2 axis)
        {
            Vector2 normal = axis.Normal().RotateClockwiseDegrees(90.0f);//Negative Direction unimportant as checked in next method
            return vec.ReflectByNormal(normal);
        }

        public static Vector2 ReflectByNormal(this Vector2 vec, Vector2 normal)
        {
            //Reflect = Vector - 2(Normal.Vector)*Normal
            return Vector2.Reflect(vec, normal);
        }
    }
}