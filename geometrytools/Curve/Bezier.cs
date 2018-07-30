using System.Numerics;

namespace GeometryTools
{
    public enum BezierType { Quadratic, Cubic }

    public class Bezier
    {
        public BezierType Type { get; set; }
        public Vector2 P1 { get; set; }
        public Vector2 P2 { get; set; }
        public Vector2 C1 { get; set; }
        public Vector2 C2 { get; set; }

        public static Vector2 PointOnCurve(float t, Bezier curve)
        {
            t = Functions.Clamp0To1(t); //must pull this out as in other curves too

            //Delta is between 0 and 1, signifying how far along the path we are analysing
            float s = 1.0f - t;
            float ss = s * s;
            float tt = t * t;

            switch (curve.Type)
            {
                case BezierType.Quadratic:
                    return (ss * curve.P1) +
                           (2.0f * s * t * curve.C1) +
                           (tt * curve.P2);
                case BezierType.Cubic:
                    float sss = ss * s;
                    float ttt = tt * t;
                    return (sss * curve.P1) +
                           (3.0f * ss * t * curve.C1) +
                           (3.0f * s * tt * curve.C2) +
                           (ttt * curve.P2);
            }
            return Vector2.Zero; //Should never get here
        }

        public static Bezier CreateBezierQuadratic(Vector2 point1, Vector2 control1, Vector2 point2)
        {
            return new Bezier
            {
                P1 = point1,
                P2 = point2,
                C1 = control1,
                C2 = Vector2.Zero,
                Type = BezierType.Quadratic
            };
        }

        public static Bezier CreateBezierCubic(Vector2 point1, Vector2 control1, Vector2 point2, Vector2 control2)
        {
            return new Bezier
            {
                P1 = point1,
                P2 = point2,
                C1 = control1,
                C2 = control2,
                Type = BezierType.Cubic
            };
        }
    }
}