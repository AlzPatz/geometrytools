using System;
using System.Numerics;

namespace GeometryTools
{
    public class LineIntersection
    {
        private const float OneMinusEps = 1.0f - Constants.Epsilon;

        public static bool IsFuzzyZero(float x)
        {
            return Math.Abs(x) < Constants.Epsilon;
        }

        /// <summary>
        /// Will not detect parralell overlapping lines
        /// </summary>
        public static bool FastLineIntersect(Vector2 a0, Vector2 a1, Vector2 b0, Vector2 b1)
        {
            var dA = (a1 - a0).Normal();
            var dB = (b1 - b0).Normal();

            if (Math.Abs(Vector2.Dot(dA, dB)) == 1.0f)
            {
                return false;
            }

            return (CcwWinding(a0, b0, b1) != CcwWinding(a1, b0, b1)) && (CcwWinding(a0, a1, b0) != CcwWinding(a0, a1, b1));
        }

        private static bool CcwWinding(Vector2 a, Vector2 b, Vector2 c)
        {
            return (c.Y - a.Y) * (b.X - a.X) > (b.Y - a.Y) * (c.X - a.X);
        }

        public static bool RobustLineIntersect(Vector2 a0, Vector2 a1, Vector2 b0, Vector2 b1, bool fireOnOverlap = true)
        {
            return RobustLineIntersectAndResult(a0, a1, b0, b1, fireOnOverlap).Intersecting;
        }

        public static LineIntersectionResult RobustLineIntersectAndResult(Vector2 a0, Vector2 a1, Vector2 b0, Vector2 b1, bool fireOnOverlap = true)
        {
            //Fail on zero length lines
            if (Vector2Ext.Equality(a0, a1) || Vector2Ext.Equality(b0, b1))
                return new LineIntersectionResult()
                {
                    Intersecting = false,
                    CoLinear = false,
                    IntersectPoint = Vector2.Zero
                };

            var r = a1 - a0;
            var s = b1 - b0;
            var qMp = b0 - a0;
            float rXs = Vector2Ext.Cross(r, s);
            float qMpXr = Vector2Ext.Cross(qMp, r);

            //1. Colinear Check
            if (IsFuzzyZero(rXs) && IsFuzzyZero(qMpXr))
            {
                //They are colinear 
                if (fireOnOverlap)
                {
                    float qMpDOTr = Vector2Ext.Dot(qMp, r);
                    float rDOTr = Vector2Ext.Dot(r, r);
                    var pMq = a0 - b0;
                    float pMqDOTs = Vector2Ext.Dot(pMq, s);
                    float sDOTs = Vector2Ext.Dot(s, s);
                    if ((qMpDOTr >= 0.0f && qMpDOTr <= rDOTr) || (pMqDOTs >= 0.0f && pMqDOTs <= sDOTs))
                    {
                        //You do not get provided a valid intersect point as the lines lie on top of each other
                        return new LineIntersectionResult()
                        {
                            Intersecting = true,
                            CoLinear = true,
                            IntersectPoint = Vector2.Zero
                        };
                    }
                }
                //Not classifying overlap as intersection
                return new LineIntersectionResult()
                {
                    Intersecting = false,
                    CoLinear = true,
                    IntersectPoint = Vector2.Zero
                };
            }

            //2. Paralell - Do not intersect
            if (IsFuzzyZero(rXs) && !IsFuzzyZero(qMpXr))
            {
                return new LineIntersectionResult()
                {
                    Intersecting = false,
                    CoLinear = false,
                    IntersectPoint = Vector2.Zero
                };
            }

            //3.Intersect at Single Point
            float t = Vector2Ext.Cross(qMp, s) / rXs;
            float u = Vector2Ext.Cross(qMp, r) / rXs;
            if (!IsFuzzyZero(rXs) && t >= 0.0f && t <= 1.0f && u >= 0.0f && u <= 1.0f)
            {
                return new LineIntersectionResult()
                {
                    Intersecting = true,
                    CoLinear = false,
                    IntersectPoint = a0 + (r * t)
                };
            }

            //4. Do not intersect 
            return new LineIntersectionResult()
            {
                Intersecting = false,
                CoLinear = false,
                IntersectPoint = Vector2.Zero
            };
        }

        public static bool IsPointOnLineSegment(Vector2 p, Vector2 v0, Vector2 v1)
        {
            var vp = p - v0;
            var l = v1 - v0;

            if (Vector2Ext.Equality(vp, Vector2Ext.Zero))
            {
                return true; //p == v0
            }

            var vpNormal = vp.Normal();
            var lNormal = l.Normal();

            if ((float)Math.Abs((double)Vector2Ext.Dot(vpNormal, lNormal)) < OneMinusEps)
                return false;

            //Lines are the same, are they within the length
            if (vp.Length() <= l.Length())
                return true;
            return false;
        }

        public static bool IsPointProjectedWithinLineSegment(Vector2 p, Vector2 l0, Vector2 l1)
        {
            var unit = (l1 - l0).Normal();

            var pDIs = Vector2.Dot(p, unit);
            var l0Dis = Vector2.Dot(l0, unit);
            var l1Dis = Vector2.Dot(l1, unit);

            var min = l0Dis < l1Dis ? l0Dis : l1Dis;
            var max = l0Dis > l1Dis ? l0Dis : l1Dis;

            return pDIs >= min && pDIs <= max;
        }

        public static float PerpendicularDistanceOfPointFromInfiniteLineCWNormalIsPositiveDirection(Vector2 p, Vector2 direction, Vector2 pOnRay)
        {
            var normal = direction.RotateClockwiseRight();

            var rayDis = Vector2.Dot(pOnRay, normal);
            var pDis = Vector2.Dot(p, normal);

            return pDis - rayDis;
        }
    }
}
