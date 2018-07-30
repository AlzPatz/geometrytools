using System;
using System.Numerics;

namespace GeometryTools
{
    //The Epsilon Fuzzy rounding seems to void the concept of fireOnEdge, so remove either. TO DO 
    public class PolygonIntersection
    {
        public static PolygonIntersectionResult CircleCircleIntersect(Circle c0, Circle c1, bool trueOnEdge = true)
        {
            return PolygonIntersection.CircleCircleIntersect(c0.Position, c0.Radius, c1.Position, c1.Radius, trueOnEdge);
        }

        public static bool CircleCircleIntersect(Vector2 centre0, float radius0, Vector2 centre1, float radius1)
        {
            return PolygonIntersection.CircleCircleIntersect(centre0, radius0, centre1, radius1, true).Intersecting;
        }

        public static PolygonIntersectionResult CircleCircleIntersect(Vector2 centre0, float radius0, Vector2 centre1, float radius1, bool trueOnEdge)
        {
            float rSquared = radius0 + radius1;
            rSquared *= rSquared;
            Vector2 delta = centre1 - centre0;
            var doesIntersect = trueOnEdge ? delta.LengthSquared() <= rSquared : delta.LengthSquared() < rSquared;

            var point = Vector2.Zero;
            var depth = 0.0f;
            var normal0to1 = Vector2.Zero;

            if (doesIntersect)
            {
                var r = (float)Math.Sqrt(rSquared);
                depth = r - delta.Length();
                normal0to1 = delta.Normal();
                point = centre0 + (normal0to1 * (radius0 - depth));
            }

            return new PolygonIntersectionResult()
            {
                Point = point,
                Depth = depth,
                Intersecting = doesIntersect,
                Normal0To1 = normal0to1
            };
        }

        public static bool PolygonPolygonIntersect(Polygon p0, Polygon p1)
        {
            return PolygonIntersection.PolygonPolygonIntersect(p0, p1, true).Intersecting;
        }

        public static PolygonIntersectionResult PolygonPolygonIntersect(Polygon p0, Polygon p1, bool trueOnEdge)
        {
            //Seperating Axis Theorem
            var seperationDepth = float.MaxValue;
            var seperationDirection0To1 = Vector2.Zero;

            float minP0 = 0.0f, maxP0 = 0.0f, minP1 = 0.0f, maxP1 = 0.0f;

            //Iterate through Poly 0 edge normals as set of axis
            for (var n = 0; n < p0.NumVertices; n++)
            {
                ProjectPolygonOnAxis(ref p0.EdgeNormals[n], p0, ref minP0, ref maxP0);
                ProjectPolygonOnAxis(ref p0.EdgeNormals[n], p1, ref minP1, ref maxP1);

                if (trueOnEdge)
                {
                    if (IntervalDistance(ref minP0, ref maxP0, ref minP1, ref maxP1) >= -Constants.Epsilon)
                        return new PolygonIntersectionResult()
                        {
                            Intersecting = false,
                            Depth = 0.0f,
                            Normal0To1 = seperationDirection0To1,
                            Point = Vector2.Zero
                        };
                }
                else
                {
                    if (IntervalDistance(ref minP0, ref maxP0, ref minP1, ref maxP1) > -Constants.Epsilon)
                        return new PolygonIntersectionResult()
                        {
                            Intersecting = false,
                            Depth = 0.0f,
                            Normal0To1 = seperationDirection0To1,
                            Point = Vector2.Zero
                        };
                }

                //Overlap is now confirmed. Drop out if one projection contains the other (i.e. sep distance doesn't make sense)
                if ((maxP0 <= maxP1 && maxP0 >= minP1 & minP0 >= minP1 && minP0 <= maxP1) ||
                   (maxP1 <= maxP0 && maxP1 >= minP0 && minP1 >= minP0 && minP1 <= maxP0))
                {
                    continue;
                }

                //Calculate overlap amount (and store if is current smallest)
                float sep = maxP0 > maxP1 ? maxP1 - minP0 : maxP0 - minP1;
                if (sep < seperationDepth)
                {
                    seperationDepth = sep;
                    seperationDirection0To1 = maxP1 > maxP0 ? p0.EdgeNormals[n] : -p0.EdgeNormals[n];
                }
            }

            //Iterate through Poly 1 edge normals as set of axis
            for (var n = 0; n < p1.NumVertices; n++)
            {
                ProjectPolygonOnAxis(ref p1.EdgeNormals[n], p0, ref minP0, ref maxP0);
                ProjectPolygonOnAxis(ref p1.EdgeNormals[n], p1, ref minP1, ref maxP1);

                if (trueOnEdge)
                {
                    if (IntervalDistance(ref minP0, ref maxP0, ref minP1, ref maxP1) >= -Constants.Epsilon)
                        return new PolygonIntersectionResult()
                        {
                            Intersecting = false,
                            Depth = 0.0f,
                            Normal0To1 = seperationDirection0To1,
                            Point = Vector2.Zero
                        };
                }
                else
                {
                    if (IntervalDistance(ref minP0, ref maxP0, ref minP1, ref maxP1) > -Constants.Epsilon)
                        return new PolygonIntersectionResult()
                        {
                            Intersecting = false,
                            Depth = 0.0f,
                            Normal0To1 = seperationDirection0To1,
                            Point = Vector2.Zero
                        };
                }

                //Overlap is now confirmed. Drop out if one projection contains the other (i.e. sep distance doesn't make sense)
                if ((maxP0 <= maxP1 && maxP0 >= minP1 & minP0 >= minP1 && minP0 <= maxP1) ||
                   (maxP1 <= maxP0 && maxP1 >= minP0 && minP1 >= minP0 && minP1 <= maxP0))
                {
                    continue;
                }

                //Calculate overlap amount (and store if is current smallest)
                float sep = maxP0 > maxP1 ? maxP1 - minP0 : maxP0 - minP1;
                if (sep < seperationDepth)
                {
                    seperationDepth = sep;
                    seperationDirection0To1 = maxP1 > maxP0 ? p1.EdgeNormals[n] : -p1.EdgeNormals[n];
                }
            }

            return new PolygonIntersectionResult()
            {
                Intersecting = true,
                Depth = seperationDepth,
                Normal0To1 = seperationDirection0To1,
                Point = Vector2.Zero
            };
        }

        private static void ProjectPolygonOnAxis(ref Vector2 axis, Polygon poly, ref float min, ref float max)
        {
            min = float.MaxValue;
            max = float.MinValue;
            for (int n = 0; n < poly.NumVertices; n++)
            {
                float dot = Vector2.Dot(poly.Vertices[n], axis);
                if (dot < min)
                    min = dot;
                if (dot > max)
                    max = dot;
            }
        }

        private static float IntervalDistance(ref float minP, ref float maxP, ref float minR, ref float maxR)
        {
            if (minP < minR)
                return minR - maxP;
            else
                return minP - maxR;
        }


        public static PolygonIntersectionResult CirclePolygonIntersect(Circle c, Polygon p, bool trueOnEdge = true)
        {
            return PolygonIntersection.CirclePolygonIntersect(c.Position, c.Radius, p, trueOnEdge);
        }

        public static bool CirclePolygonIntersect(Vector2 centre, float radius, Polygon p)
        {
            return PolygonIntersection.CirclePolygonIntersect(centre, radius, p, true).Intersecting;
        }

        public static PolygonIntersectionResult CirclePolygonIntersect(Vector2 centre, float radius, Polygon p, bool trueOnEdge)
        {
            //Seperating Axis Theorem
            var seperationDepth = float.MaxValue;
            var seperationDirectionCircleToPoly = new Vector2(0.0f, 0.0f);

            float minPoly = 0.0f, maxPoly = 0.0f, minCircle = 0.0f, maxCircle = -0.0f;
            //Iterate through Poly 0 edge normals as set of axis
            for (var n = 0; n < p.NumVertices; n++)
            {
                ProjectPolygonOnAxis(ref p.EdgeNormals[n], p, ref minPoly, ref maxPoly);
                ProjectCircleOnAxis(ref p.EdgeNormals[n], centre, radius, ref minCircle, ref maxCircle);

                if (trueOnEdge)
                {
                    if (IntervalDistance(ref minPoly, ref maxPoly, ref minCircle, ref maxCircle) >= -Constants.Epsilon)
                        return new PolygonIntersectionResult()
                        {
                            Intersecting = false,
                            Depth = 0.0f,
                            Normal0To1 = seperationDirectionCircleToPoly,
                            Point = Vector2.Zero
                        };
                }
                else
                {
                    if (IntervalDistance(ref minPoly, ref maxPoly, ref minCircle, ref maxCircle) > -Constants.Epsilon)
                        return new PolygonIntersectionResult()
                        {
                            Intersecting = false,
                            Depth = 0.0f,
                            Normal0To1 = seperationDirectionCircleToPoly,
                            Point = Vector2.Zero
                        };
                }

                //Overlap is now confirmed. Drop out if one projection contains the other (i.e. sep distance doesn't make sense)
                if ((maxPoly <= maxCircle && maxPoly >= minCircle & minPoly >= minCircle && minPoly <= maxCircle) ||
                   (maxCircle <= minPoly && maxCircle >= minPoly && minCircle >= minPoly && minCircle <= maxPoly))
                {
                    continue;
                }

                //Calculate overlap amount (and store if is current smallest)
                float sep = maxPoly > maxCircle ? maxCircle - minPoly : maxPoly - minCircle;
                if (sep < seperationDepth)
                {
                    seperationDepth = sep;
                    seperationDirectionCircleToPoly = maxCircle > maxPoly ? -p.EdgeNormals[n] : p.EdgeNormals[n];
                }
            }

            //Iterate through axis made by the poly edge vertices and circle centre
            for (var n = 0; n < p.NumVertices; n++)
            {
                Vector2 axis = (p.Vertices[n] - centre).Normal();
                ProjectPolygonOnAxis(ref axis, p, ref minPoly, ref maxPoly);
                ProjectCircleOnAxis(ref axis, centre, radius, ref minCircle, ref maxCircle);

                if (trueOnEdge)
                {
                    if (IntervalDistance(ref minPoly, ref maxPoly, ref minCircle, ref maxCircle) >= -Constants.Epsilon)
                        return new PolygonIntersectionResult()
                        {
                            Intersecting = false,
                            Depth = 0.0f,
                            Normal0To1 = seperationDirectionCircleToPoly,
                            Point = Vector2.Zero
                        };
                }
                else
                {
                    if (IntervalDistance(ref minPoly, ref maxPoly, ref minCircle, ref maxCircle) > -Constants.Epsilon)
                        return new PolygonIntersectionResult()
                        {
                            Intersecting = false,
                            Depth = 0.0f,
                            Normal0To1 = seperationDirectionCircleToPoly,
                            Point = Vector2.Zero
                        };
                }

                //Overlap is now confirmed. Drop out if one projection contains the other (i.e. sep distance doesn't make sense)
                if ((maxPoly <= maxCircle && maxPoly >= minCircle & minPoly >= minCircle && minPoly <= maxCircle) ||
                   (maxCircle <= minPoly && maxCircle >= minPoly && minCircle >= minPoly && minCircle <= maxPoly))
                {
                    continue;
                }

                //Calculate overlap amount (and store if is current smallest)
                float sep = maxPoly > maxCircle ? maxCircle - minPoly : maxPoly - minCircle;
                if (sep < seperationDepth)
                {
                    seperationDepth = sep;
                    seperationDirectionCircleToPoly = maxCircle > maxPoly ? -axis : axis;
                }
            }

            return new PolygonIntersectionResult()
            {
                Intersecting = true,
                Depth = seperationDepth,
                Normal0To1 = seperationDirectionCircleToPoly,
                Point = Vector2.Zero
            };
        }

        private static void ProjectCircleOnAxis(ref Vector2 axis, Vector2 centre, float radius, ref float min, ref float max)
        {
            float c = Vector2.Dot(centre, axis);
            min = c - radius;
            max = c + radius;
        }

        public static bool IsPointInCircle(Vector2 point, Circle circle, bool trueOnEdge = true)
        {
            return PolygonIntersection.IsPointInCircle(point, circle.Position, circle.Radius, trueOnEdge);
        }

        public static bool IsPointInCircle(Vector2 point, Vector2 circleCentre, float circleRadius, bool trueOnEdge = true)
        {
            if (trueOnEdge)
            {
                return Vector2Ext.DistanceBetweenTwoPoints(point, circleCentre) <= circleRadius;
            }
            else
            {
                return Vector2Ext.DistanceBetweenTwoPoints(point, circleCentre) < circleRadius;
            }
        }

        public static bool IsPointInPolygon(Vector2 point, Polygon poly, bool trueOnEdge = true, bool useWindingMethod = true)
        {
            if (trueOnEdge)
            {
                bool onEdge = false;
                for (var n = 0; n < poly.NumVertices; n++)
                {
                    var next = n == poly.NumVertices - 1 ? 0 : n + 1;
                    if (LineIntersection.IsPointOnLineSegment(point, poly.Vertices[n], poly.Vertices[next]))
                    {
                        onEdge = true;
                    }
                    if (onEdge)
                        return true;
                }
            }

            if (useWindingMethod)
                return CalculatePolygonWinding(poly, point) != 0;
            else
                return CalculatePolyCrossing(poly, point);
        }

        private static int CalculatePolygonWinding(Polygon poly, Vector2 point)
        {
            int winding = 0;

            for (var n = 0; n < poly.NumVertices; n++)
            {
                var next = n == poly.NumVertices - 1 ? 0 : n + 1;

                if (poly.Vertices[n].Y <= point.Y)
                {
                    if (poly.Vertices[next].Y > point.Y)
                    {
                        if (isLeft(ref poly.Vertices[n], ref poly.Vertices[next], ref point) > 0.0f)
                            winding++;
                    }
                }
                else
                {
                    if (poly.Vertices[next].Y <= point.Y)
                    {
                        if (isLeft(ref poly.Vertices[n], ref poly.Vertices[next], ref point) < 0.0f)
                            winding--;
                    }
                }
            }
            return winding;
        }

        private static float isLeft(ref Vector2 p0, ref Vector2 p1, ref Vector2 p2)
        {
            return (((p1.X - p0.X) * (p2.Y - p0.Y)) - ((p2.X - p0.X) * (p1.Y - p0.Y)));
        }

        private static bool CalculatePolyCrossing(Polygon poly, Vector2 point)
        {
            bool insideToggle = false;
            int i, j = 0;
            for (i = 0, j = poly.NumVertices - 1; i < poly.NumVertices; j = i++)
            {
                if (((poly.Vertices[i].Y > point.Y) != (poly.Vertices[j].Y > point.Y)) &&
                                           (point.X < (poly.Vertices[j].X - poly.Vertices[i].X) *
                                           (point.Y - poly.Vertices[i].Y) /
                                           (poly.Vertices[j].Y - poly.Vertices[i].Y) +
                                           poly.Vertices[i].X))
                    insideToggle = !insideToggle;
            }

            return insideToggle;
        }
    }
}

