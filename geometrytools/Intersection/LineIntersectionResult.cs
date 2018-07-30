using System.Numerics;

namespace GeometryTools
{
    public class LineIntersectionResult
    {
        public bool Intersecting { get; set; }
        public bool CoLinear { get; set; }
        public Vector2 IntersectPoint { get; set; }
    }
}