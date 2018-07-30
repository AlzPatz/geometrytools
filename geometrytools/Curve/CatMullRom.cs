using System;
using System.Numerics;

namespace GeometryTools
{
    public class CatMullRom
    {
        public static Vector2 CatmullRomSpline(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            t = Functions.Clamp0To1(t);
            return 0.5f *
                    ((2f * p1) + ((p2 - p0) * t) +
                    ((2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t) +
                    ((-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t));
        }
    }
}