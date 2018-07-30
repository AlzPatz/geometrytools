using System.Numerics;

namespace GeometryTools
{
    public class AABB
    {
        public static bool CheckOverlap(AABB A, AABB B)
        {
            Vector2 ext = A.Extents + B.Extents;
            Vector2 delta = A.Centre - B.Centre;
            if (delta.X < 0.0f)
                delta.X = -delta.X;
            if (delta.Y < 0.0f)
                delta.Y = -delta.Y;
            return delta.X <= ext.X && delta.Y <= ext.Y;
        }

        public static bool CheckLineIntersect(AABB aabb, Vector2 p0, Vector2 p1)
        {
            if (aabb.Contains(p0))
                return true;

            if (aabb.Contains(p1))
                return true;

            var points = new Vector2[]
                {
                    new Vector2(aabb.Left, aabb.Top),
                    new Vector2(aabb.Right, aabb.Top),
                    new Vector2(aabb.Right, aabb.Bottom),
                    new Vector2(aabb.Left, aabb.Bottom)
                };

            for (var n = 0; n < 4; n++)
            {
                var m = n + 1;
                if (m == 4)
                    m = 0;

                if (LineIntersection.RobustLineIntersect(points[n], points[m], p0, p1))
                    return true;
            }

            return false;
        }

        public static bool Equality(AABB A, AABB B)
        {
            return A.Top == B.Top && A.Bottom == B.Bottom && A.Left == B.Left && A.Right == B.Right;
        }

        public Vector2 Centre { get; private set; }
        public Vector2 Extents { get; private set; }
        public Vector2 Min { get; private set; }
        public Vector2 Max { get; private set; }
        public float Top { get; private set; }
        public float Bottom { get; private set; }
        public float Left { get; private set; }
        public float Right { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }

        public AABB(AABB aabb)
        {
            Centre = aabb.Centre;
            Extents = aabb.Extents;
            Min = aabb.Min;
            Max = aabb.Max;
            Top = aabb.Top;
            Bottom = aabb.Bottom;
            Left = aabb.Left;
            Right = aabb.Right;
            Width = aabb.Width;
            Height = aabb.Height;
        }

        public AABB(Vector2 centre, Vector2 extents, bool validate = false)
        {
            if (validate)
            {
                if (extents.X < 0)
                    extents.X = -extents.X;
                if (extents.X == 0.0f)
                    extents.X = 1.0f;
                if (extents.Y < 0)
                    extents.Y = -extents.Y;
                if (extents.Y == 0.0f)
                    extents.Y = 1.0f;
            }

            Centre = centre;
            Extents = extents;
            Min = centre - extents;
            Max = centre + extents;
            Top = Max.Y;
            Bottom = Min.Y;
            Left = Min.X;
            Right = Max.X;
            Width = Right - Left;
            Height = Top - Bottom;
        }

        public AABB(Vector2 min, Vector2 max, int unused, bool validate = false) //Dirt!
        {
            if (validate)
            {
                if (Vector2Ext.Equality(min, max))
                {
                    max.X += 1.0f;
                    max.Y += 1.0f;
                }

                if (max.X < min.X)
                {
                    var s = max.X;
                    max.X = min.X;
                    min.X = s;
                }

                if (max.Y < min.Y)
                {
                    var s = max.Y;
                    max.Y = min.Y;
                    min.Y = s;
                }
            }

            Min = min;
            Max = max;
            Centre = 0.5f * (Min + Max);
            Extents = 0.5f * (Max - Min);
            Top = Max.Y;
            Bottom = Min.Y;
            Left = Min.X;
            Right = Max.X;
            Width = Right - Left;
            Height = Top - Bottom;
        }

        public AABB(float top, float bottom, float left, float right, bool validate = false)
        {
            if (validate)
            {
                if (top < bottom)
                {
                    var s = top;
                    top = bottom;
                    bottom = s;
                }

                if (right < left)
                {
                    var s = left;
                    left = right;
                    right = s;
                }
            }

            Top = top;
            Bottom = bottom;
            Left = left;
            Right = right;
            Min = new Vector2(Left, Bottom);
            Max = new Vector2(Right, Top);
            Centre = 0.5f * (Min + Max);
            Extents = 0.5f * (Max - Min);
            Width = Right - Left;
            Height = Top - Bottom;
        }

        public bool CheckOverlap(AABB B)
        {
            Vector2 ext = this.Extents + B.Extents;
            Vector2 delta = this.Centre - B.Centre;
            if (delta.X < 0.0f)
                delta.X = -delta.X;
            if (delta.Y < 0.0f)
                delta.Y = -delta.Y;
            return delta.X <= ext.X && delta.Y <= ext.Y;
        }

        public bool Contains(Vector2 point)
        {
            return point.X >= Min.X && point.X <= Max.X && point.Y >= Min.Y && point.Y <= Max.Y;
        }

        public bool Contains(AABB aabb)
        {
            return aabb.Min.X >= this.Min.X && aabb.Min.X <= this.Max.X && aabb.Max.X >= this.Min.X && aabb.Max.X <= this.Max.X &&
                aabb.Min.Y >= this.Min.Y && aabb.Min.Y <= this.Max.Y && aabb.Max.Y >= this.Min.Y && aabb.Max.Y <= this.Max.Y;
        }

        public AABB Q1
        {
            get { return new AABB(Max.Y, Centre.Y, Min.X, Centre.X); }
        }

        public AABB Q2
        {
            get { return new AABB(Max.Y, Centre.Y, Centre.X, Max.X); }
        }

        public AABB Q3
        {
            get { return new AABB(Centre.Y, Min.Y, Min.X, Centre.X); }
        }

        public AABB Q4
        {
            get { return new AABB(Centre.Y, Min.Y, Centre.X, Max.X); }
        }
    }
}