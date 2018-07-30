using System;
using System.Numerics;

namespace GeometryTools
{
    /*
     * Check special cases in general
     * Negative d? doesn't appear to work
     * > < im special case
     */

    public class Arc
    {
        public Vector2 Centre;             //Centre of Circle
        public Vector2 Axis;               //From Centre to End Point
        public float Radius;              //Radius of Circle 
        public float Angle;               //Angle to rotate from Axis
        public float ArcLength;           //Distance along the arch

        //These two need to be collapsed or used in the  Curve below or, added later from Kerbing inTrack. TO DO TO DO 
        public Vector2 ReturnRightUnitVectorOnArc(bool traverseForward, float fraction)
        {
            Vector2 result = Vector2.Zero;

            if (traverseForward)
            {
                if (Radius == 0.0f)
                {
                    result = Axis.RotateClockwiseRight();
                }
                else
                {
                    float angle = Angle * fraction;
                    var multiplier = Angle >= 0.0f ? +1.0f : -1.0f;
                    result = multiplier * Axis.RotateClockwiseRadians(-angle).Normal();
                }
            }
            else
            {
                if (Radius == 0.0f)
                {
                    result = Axis.RotateClockwiseRight();
                }
                else
                {
                    float angle = Angle * (1.0f - fraction);
                    var multiplier = Angle >= 0.0f ? -1.0f : +1.0f;
                    result = multiplier * Axis.RotateClockwiseRadians(-angle).Normal();
                }
            }

            return result;
        }

        public Vector2 PointAlongArc(bool traverseForward, float fraction)
        {
            fraction = Functions.Clamp0To1(fraction);

            Vector2 result = Vector2.Zero;

            if (traverseForward)
            {
                if (Radius == 0.0f)
                {
                    //Interpolate along line
                    result = Centre + (Axis * (1.0f - 2.0f * fraction));

                }
                else
                {
                    //Interpolate along arc
                    float angle = Angle * fraction;
                    result = Centre + Axis.RotateClockwiseRadians(-angle);
                }
            }
            else
            {
                if (Radius == 0.0f)
                {
                    //Interpolate along line
                    result = Centre + (Axis * (-1.0f + 2.0f * fraction));

                }
                else
                {
                    //Interpolate along arc
                    float angle = Angle * (1.0f - fraction);
                    result = Centre + Axis.RotateClockwiseRadians(-angle);
                }
            }

            return result;
        }
    }

    public class Biarc
    {
        private bool _valid;

        private Vector2 _p0;
        private Vector2 _t0;
        private Vector2 _p1;
        private Vector2 _t1;

        public Arc Arc0 { get; private set; }
        public Arc Arc1 { get; private set; }

        public Biarc()
        {
            _valid = false;
        }

        public Biarc(Vector2 point0, Vector2 tangent0, Vector2 point1, Vector2 tangent1)
        {
            SetParams(point0, tangent0, point1, tangent1);
        }

        public void UpdateParamaters(Vector2 point0, Vector2 tangent0, Vector2 point1, Vector2 tangent1)
        {
            if (_p0 != null && _p1 != null && _t0 != null && _t1 != null)
            {
                //Only recompute arc if any of the paramaters have changed
                if (Vector2Ext.Equality(_p0, point0) && Vector2Ext.Equality(_t0, tangent0) && Vector2Ext.Equality(_p1, point1) && Vector2Ext.Equality(_t1, tangent1))
                    return;
            }

            SetParams(point0, tangent0, point1, tangent1);
        }

        private void SetParams(Vector2 point0, Vector2 tangent0, Vector2 point1, Vector2 tangent1)
        {
            _p0 = point0;
            _p1 = point1;
            _t0 = tangent0;
            _t1 = tangent1;

            ComputeArcs();
        }

        private void ComputeArcs()
        {
            //Using Approach as detailed in: http://www.ryanjuckett.com/programming/biarc-interpolation/

            _valid = false;

            Arc0 = new Arc();
            Arc1 = new Arc();

            //Need to ensure tangents are normalised. Technique here is to renormalise and then check for if equal zero length (which happens if the normalise fails). Can replace with more robust / direct method later, if this becomes a concern
            _t0 = _t0.Normal();
            _t1 = _t1.Normal();
            if (_t0.LengthSquared() == 0.0f || _t1.LengthSquared() == 0.0f)
                return;

            Vector2 v = _p1 - _p0;

            //If points equal then no interpolation required
            if (Vector2Ext.Equality(v, Vector2.Zero))
            {
                Arc0.Centre = _p0;
                Arc0.Radius = 0.0f;
                Arc0.Axis = v;
                Arc0.Angle = 0.0f;
                Arc0.ArcLength = 0.0f;

                Arc1.Centre = _p1;
                Arc1.Radius = 0.0f;
                Arc1.Axis = v;
                Arc1.Angle = 0.0f;
                Arc1.ArcLength = 0.0f;

                _valid = true;
                return;
            }

            float pi = (float)Math.PI;

            float vDotV = Vector2.Dot(v, v);

            //We are using the approach that automatically picks the values of d, where d1 = d2
            //The formula for deriving d2 is a quadratic
            /*
             d2 = [ -(v.t) +/- sqrt((v.t)^2 + 2.(1 + t1.t2)(v.v)) ] / (2* (1 - t1.t2))
             */
            //Compute denominator of the quadratic formula as it tells us if it is solvable or not (i.e. is it = 0)

            Vector2 t = _t0 + _t1;
            float vDotT = Vector2.Dot(v, t);
            float t0DotT1 = Vector2.Dot(_t0, _t1);
            float denominator = 2.0f * (1.0f - t0DotT1);

            float d; //Set for those cases that do not return straight away with solution

            //If quadratic formula denominator is zero. There are 2 cases here. In both cases the tangents are equal. In one the vector between the two curve points are perpendicular to them
            if (denominator == 0.0f)
            {
                float vDotT1 = Vector2.Dot(v, _t1);

                if ((float)Math.Abs(vDotT1) == 0.0f)
                {
                    //Tangents are equal to each other AND perpendicular to p0 and p1. The solution is two equal semi-circles
                    float vMag = v.Length();
                    float radius = 0.25f * vMag;
                    Vector2 quarterV = 0.25f * v;

                    Vector2 pm = _p0 + (2.0f * quarterV);

                    float vCrossT1 = (v.X * _t1.Y) - (v.Y * _t1.X);

                    Arc0.Centre = _p0 + quarterV;
                    Arc0.Radius = radius;
                    Arc0.Axis = _p0 - Arc0.Centre;
                    Arc0.Angle = vCrossT1 > 0.0f ? pi : -pi; //Need to check this > or <. standard is < then > but doesn't seem to work...
                    Arc0.ArcLength = pi * radius;

                    Arc1.Centre = _p0 + (3.0f * quarterV);
                    Arc1.Radius = radius;
                    Arc1.Axis = _p1 - Arc1.Centre;
                    Arc1.Angle = vCrossT1 > 0.0f ? pi : -pi;
                    Arc1.ArcLength = pi * radius;

                    _valid = true;
                    return;
                }
                else
                {
                    //The Tangents are equal to each other (this is the only route through where d can be negative)
                    d = vDotV / (4.0f * vDotT1);
                }
            }
            else
            {
                //Standard result, we use the positive version of the quadratic formula to calculate d
                float discriminant = (vDotT * vDotT) + (denominator * vDotV);
                d = (-vDotT + (float)Math.Sqrt(discriminant)) / denominator;
            }

            //At this stage we have either calculated the value of d (d1 = d2), or we have set the two arcs appropriately for special cases

            //Calculate the mid point
            //pm=p1+p2+d2(t1−t2)/2

            Vector2 mid = 0.5f * (_p0 + _p1 + (d * (_t0 - _t1)));

            //Caclulate Vectors from the curve points to the mid point
            Vector2 p0ToMid = mid - _p0;
            Vector2 p1ToMid = mid - _p1;

            //Calculate Arc Centre, Radius and Angle values
            Arc0 = ComputeSingleArc(_p0, _t0, p0ToMid);
            Arc1 = ComputeSingleArc(_p1, _t1, p1ToMid);


            if (d < 0.0f)
            {
                //Use longer path around if d is negative (doesn't seem to work right yet)
                /*
                float arc0mul = 2.0f * pi;
                arc0mul *= _arc0.Angle >= 0.0f ? 1.0f : -1.0f;
                _arc0.Angle = arc0mul - _arc0.Angle;

                float arc1mul = 2.0f * pi;
                arc1mul *= _arc1.Angle >= 0.0f ? 1.0f : -1.0f;
                _arc1.Angle = arc1mul - _arc1.Angle;
                */
            }

            //Ignoring the code to check for negative d in the example, as surely should always be positive (i.e. we are taking the shorter parth around the circles)

            Arc0.Axis = _p0 - Arc0.Centre;
            Arc1.Axis = _p1 - Arc1.Centre;

            Arc0.ArcLength = Arc0.Radius == 0.0f ? p0ToMid.Length() : (float)Math.Abs(Arc0.Radius * Arc0.Angle);
            Arc1.ArcLength = Arc1.Radius == 0.0f ? p1ToMid.Length() : (float)Math.Abs(Arc1.Radius * Arc1.Angle);

            _valid = true;
        }

        private Arc ComputeSingleArc(Vector2 p, Vector2 t, Vector2 pToMid)
        {
            //Only calculates the Arc Centre, Radius and Angle values
            t = t.Normal();
            Vector2 perp = new Vector2(-t.Y, t.X);

            float denominator = 2.0f * Vector2.Dot(perp, pToMid);

            if (Math.Abs(denominator) == 0.0f)
            {
                //Infinite Radius = Straight Line
                return new Arc
                {
                    Centre = p + (0.5f * pToMid),
                    Radius = 0.0f,
                    Angle = 0.0f
                };
            }

            //Distance along perp axis, then calc circle centre
            float dis = Vector2.Dot(pToMid, pToMid) / denominator;
            Vector2 mid = p + pToMid;
            Vector2 centre = p + (dis * perp);
            float radius = (float)Math.Abs(dis);
            float angle = 0.0f;

            if (radius > 0.0f)
            {
                //See link for why this formula is selected. note d from earlier counts is always positive
                Vector2 Op = (p - centre) / radius;
                Vector2 Om = (mid - centre) / radius;

                float cross = (Op.X * Om.Y) - (Op.Y * Om.X);
                angle = (float)Math.Acos(Vector2.Dot(Op, Om));
                angle *= cross > 0.0f ? 1.0f : -1.0f;
            }

            return new Arc
            {
                Centre = centre,
                Radius = radius,
                Angle = angle
            };
        }

        public Vector2 ReturnPointOnCurve(float fraction)
        {
            if (!_valid)
                return Vector2.Zero;

            fraction = Functions.Clamp0To1(fraction);

            float totalDistanceAlongCurve = Arc0.ArcLength + Arc1.ArcLength;
            float distanceAlongCurve = fraction * totalDistanceAlongCurve;

            Vector2 result = Vector2.Zero;

            if (distanceAlongCurve < Arc0.ArcLength)
            {
                //Arc0
                float arcFrac = distanceAlongCurve / Arc0.ArcLength;
                if (Arc0.Radius == 0.0f)
                {
                    //Interpolate along line
                    result = Arc0.Centre + (Arc0.Axis * (1.0f - 2.0f * arcFrac));

                }
                else
                {
                    //Interpolate along arc
                    float angle = Arc0.Angle * arcFrac;
                    result = Arc0.Centre + Arc0.Axis.RotateClockwiseRadians(-angle);
                }
            }
            else
            {
                //Arc1
                float arcFrac = (distanceAlongCurve - Arc0.ArcLength) / Arc1.ArcLength;
                if (Arc0.Radius == 0.0f)
                {
                    //Interpolate along line
                    result = Arc1.Centre + (Arc1.Axis * (-1.0f + 2.0f * arcFrac));

                }
                else
                {
                    //Interpolate along arc
                    float angle = Arc1.Angle * (1.0f - arcFrac);
                    result = Arc1.Centre + Arc1.Axis.RotateClockwiseRadians(-angle);
                }
            }

            return result;
        }

        public Vector2 ReturnRightUnitVectorOnCurve(float fraction)
        {
            if (!_valid)
                return Vector2.Zero;

            fraction = Functions.Clamp0To1(fraction);

            float totalDistanceAlongCurve = Arc0.ArcLength + Arc1.ArcLength;
            float distanceAlongCurve = fraction * totalDistanceAlongCurve;

            Vector2 result = Vector2.Zero;

            if (distanceAlongCurve < Arc0.ArcLength)
            {
                //Arc0
                float arcFrac = distanceAlongCurve / Arc0.ArcLength;
                if (Arc0.Radius == 0.0f)
                {
                    result = Arc0.Axis.RotateClockwiseRight();
                }
                else
                {
                    float angle = Arc0.Angle * arcFrac;
                    var multiplier = Arc0.Angle >= 0.0f ? +1.0f : -1.0f;
                    result = multiplier * Arc0.Axis.RotateClockwiseRadians(-angle).Normal();
                }
            }
            else
            {
                //Arc1
                float arcFrac = (distanceAlongCurve - Arc0.ArcLength) / Arc1.ArcLength;
                if (Arc0.Radius == 0.0f)
                {
                    result = Arc1.Axis.RotateClockwiseRight();
                }
                else
                {
                    float angle = Arc1.Angle * (1.0f - arcFrac);
                    var multiplier = Arc1.Angle >= 0.0f ? -1.0f : +1.0f;
                    result = multiplier * Arc1.Axis.RotateClockwiseRadians(-angle).Normal();
                }
            }

            return result;
        }
    }
}