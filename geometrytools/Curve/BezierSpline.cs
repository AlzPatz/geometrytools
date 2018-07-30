using System;
using System.Numerics;

namespace GeometryTools
{
    public class BezierSpline
    {
        internal class BezierWrap
        {
            public Bezier Curve { get; set; }
            public bool TraverseInReverse { get; set; }
            public float ApproxLength { get; set; }
            public float FractionalLength { get; set; }
            public float FractionStart { get; set; }
            public float FractionEnd { get; set; }
        }

        public static BezierSpline Create(Vector2[] pathPoints, float smoothness, bool closed, int numberOfDivisionsPerCurveForLengthApproximation)
        {
            var numPoints = pathPoints.Length;

            var controlPoints = CreateControlPoints(numPoints, pathPoints, smoothness);

            var curves = CreateBezierCurves(numPoints, pathPoints, controlPoints, closed);

            return new BezierSpline(curves, numberOfDivisionsPerCurveForLengthApproximation);
        }

        private static Tuple<Vector2, Vector2>[] CreateControlPoints(int numPoints, Vector2[] pathPoints, float smoothness)
        {
            var controlPoints = new Tuple<Vector2, Vector2>[numPoints];

            for (var current = 0; current < numPoints; current++)
            {
                var prior = PriorPoint(current, numPoints);
                var next = NextPoint(current, numPoints);

                controlPoints[current] = CreateControlPointFromPathSegment(pathPoints[prior], pathPoints[current], pathPoints[next], smoothness);
            }

            return controlPoints;
        }

        private static int PriorPoint(int current, int numPoints)
        {
            var prior = current - 1;
            if (prior < 0)
                prior = numPoints - 1;
            return prior;
        }

        private static int NextPoint(int current, int numPoints)
        {
            var next = current + 1;
            if (next == numPoints)
                next = 0;
            return next;
        }

        private static Tuple<Vector2, Vector2> CreateControlPointFromPathSegment(Vector2 v0, Vector2 v1, Vector2 v2, float t)
        {
            var d01 = Vector2.Distance(v0, v1);
            var d12 = Vector2.Distance(v1, v2);

            var f1 = (t * d01) / (d01 + d12);
            var f2 = (t * d12) / (d01 + d12);

            var dv = v2 - v0;

            var p1 = v1 - (f1 * dv);
            var p2 = v1 + (f2 * dv);

            return new Tuple<Vector2, Vector2>(p1, p2);
        }

        private static BezierWrap[] CreateBezierCurves(int numPoints, Vector2[] pathPoints, Tuple<Vector2, Vector2>[] controlPoints, bool closed)
        {
            var numCurves = closed ? numPoints : numPoints - 1;
            var curves = new BezierWrap[numCurves];

            for (var n = 0; n < numCurves; n++)
            {
                var m = NextPoint(n, numPoints);

                var pointCurrent = pathPoints[n];
                var pointNext = pathPoints[m];

                var CpsCurrent = controlPoints[n];
                var CpsNext = controlPoints[m];

                if (!closed)
                {
                    if (n == 0)
                    {
                        curves[n] = new BezierWrap
                        {
                            Curve = Bezier.CreateBezierQuadratic(pointCurrent, CpsNext.Item1, pointNext),
                            TraverseInReverse = false
                        };
                        continue;
                    }
                    else if (n == numCurves - 1)
                    {
                        curves[n] = new BezierWrap
                        {
                            Curve = Bezier.CreateBezierQuadratic(pointNext, CpsCurrent.Item2, pointCurrent),
                            TraverseInReverse = true
                        };
                        continue;
                    }
                }
                curves[n] = new BezierWrap
                {
                    Curve = Bezier.CreateBezierCubic(pointCurrent, CpsCurrent.Item2, pointNext, CpsNext.Item1),
                    TraverseInReverse = false
                };
            }
            return curves;
        }

        private int _numCurves;
        private BezierWrap[] _curves;
        private float _approxLengthOfSpline;

        private BezierSpline(BezierWrap[] curves, int numberOfDivisionsPerCurveForLengthApproximation)
        {
            _numCurves = curves.Length;
            _curves = curves;

            _approxLengthOfSpline = SumAndCalculateCurveLength(numberOfDivisionsPerCurveForLengthApproximation);
            CalculateFractionalLengths();
        }

        private float SumAndCalculateCurveLength(int numDivisionsPerCurve)
        {
            var sum = 0.0f;
            var frac = 1.0f / (1.0f * numDivisionsPerCurve);

            for (var n = 0; n < _numCurves; n++)
            {
                var len = MeasureCurveApproximateLength(_curves[n], frac, numDivisionsPerCurve);
                _curves[n].ApproxLength = len;
                sum += len;
            }

            return sum;
        }

        private float MeasureCurveApproximateLength(BezierWrap curve, float frac, int numDivs)
        {
            var len = 0.0f;
            for (var n = 0; n < numDivs; n++)
            {
                var m = n + 1;

                var fracStart = n * frac;
                var fracEnd = m * frac;

                if (curve.TraverseInReverse)
                {
                    fracStart = 1.0f - fracStart;
                    fracEnd = 1.0f - fracEnd;
                }

                var p0 = Bezier.PointOnCurve(fracStart, curve.Curve);
                var p1 = Bezier.PointOnCurve(fracEnd, curve.Curve);

                len += Vector2.Distance(p0, p1);
            }
            return len;
        }

        private void CalculateFractionalLengths()
        {
            var aggregateFraction = 0.0f;
            for (var n = 0; n < _numCurves; n++)
            {
                var curve = _curves[n];
                var fracOfWhole = curve.ApproxLength / _approxLengthOfSpline;

                curve.FractionalLength = fracOfWhole;
                curve.FractionStart = aggregateFraction;
                curve.FractionEnd = aggregateFraction + fracOfWhole;

                if (n == _numCurves - 1)
                    curve.FractionEnd = 1.0f;

                aggregateFraction = curve.FractionEnd;
            }
        }

        public Vector2 PointOnSpline(float fraction)
        {
            fraction = Functions.Clamp0To1(fraction);

            var curve = FindCurveFromFractionAlongSpline(fraction);

            var fractionOnCurve = FindFractionAlongCurve(fraction, curve);

            if (curve.TraverseInReverse)
                fractionOnCurve = 1.0f - fractionOnCurve;

            return Bezier.PointOnCurve(fractionOnCurve, curve.Curve);
        }

        private BezierWrap FindCurveFromFractionAlongSpline(float fraction)
        {
            BezierWrap result = null;
            for (var n = 0; n < _numCurves; n++)
            {
                if (fraction >= _curves[n].FractionStart)
                {
                    if (fraction <= _curves[n].FractionEnd)
                    {
                        result = _curves[n];
                    }
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        private float FindFractionAlongCurve(float fraction, BezierWrap curve)
        {
            return (fraction - curve.FractionStart) / curve.FractionalLength;
        }

        public float ApproximateDistanceAroundSpline(float fraction)
        {
            fraction = Functions.Clamp0To1(fraction);
            return fraction * _approxLengthOfSpline;
        }
    }
}