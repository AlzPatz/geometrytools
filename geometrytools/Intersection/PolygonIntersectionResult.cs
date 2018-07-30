using System.Numerics;

namespace GeometryTools
{
    public class PolygonIntersectionResult
    {
        public bool Intersecting { get; set; }
        public Vector2 Point { get; set; } //Means nothing for polys (zero) and little for circles. consider changing or removing
        public float Depth { get; set; }
        public Vector2 Normal0To1 { get; set; }
    }
}