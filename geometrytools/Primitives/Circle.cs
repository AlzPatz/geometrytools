using System.Numerics;

namespace GeometryTools
{
    public class Circle
    {
        public float Radius { get; set; }
        public Vector2 Position { get; set; }

        public Circle(Vector2 position, float radius)
        {
            Position = position;
            Radius = radius;
        }

        public void Translate(Vector2 translation)
        {
            Position += translation;
        }
    }
}
