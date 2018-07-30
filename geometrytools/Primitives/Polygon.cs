using System;
using System.Numerics;

namespace GeometryTools
{
    public enum AngleType { Degrees, Radians }

    public class Polygon : ITreeItem
    {
        public AABB Aabb { get; set; }
        public Guid? Index { get; set; }
        public ITree Tree { get; set; }
        public bool IsInTree { get; set; }

        public int NumVertices { get; private set; }
        public Vector2[] Vertices { get; private set; }
        public Vector2[] EdgeNormals { get; private set; }
        public Vector2[] EdgeTangents { get; private set; }
        public float[] EdgeLengths { get; private set; }
        public Vector2 Centre { get; private set; }

        public static Polygon CreateRegularNSidedPolygon(Vector2 centre, int numSides, float radius)
        {
            if (numSides < 2)
                return null;

            Vector2[] verts = new Vector2[numSides];
            float dAngle = (2.0f * (float)Math.PI / numSides);
            for (var n = 0; n < numSides; n++)
            {
                Vector2 dir = Vector2Ext.Up.RotateClockwiseRadians(1.0f * n * dAngle);
                verts[n] = centre + (radius * dir);
            }

            return new Polygon(verts);
        }

        public Polygon(Vector2[] vertices, ITree tree = null)
        {
            //Need to deal with unordered / incorrectly wrapped polygons?

            if (tree == null)
            {
                IsInTree = false;
            }
            else
            {
                IsInTree = true;
                Tree = tree;
            }

            NumVertices = vertices.Length;
            Vertices = new Vector2[NumVertices];
            EdgeNormals = new Vector2[NumVertices];
            EdgeTangents = new Vector2[NumVertices];
            EdgeLengths = new float[NumVertices];

            Centre = new Vector2(0.0f, 0.0f);
            for (var n = 0; n < NumVertices; n++)
            {
                Vertices[n] = vertices[n];
                Centre = Centre + Vertices[n];
            }
            Centre = Centre / (1.0f * NumVertices);

            CalculateUnits();
            CalculateAABB();

            if (IsInTree)
                InsertInTree(Tree);
        }

        public void InsertInTree(ITree tree)
        {
            Tree = tree;
            IsInTree = Tree.Insert(this);
        }

        public void RemoveFromTree(ITree tree = null)
        {
            if (tree == null && Tree == null)
                return;

            if (tree == null)
                Tree.Remove(this);
            else
                tree.Remove(this);

            IsInTree = false;
        }

        private void ReInsertInTree()
        {
            Tree.Remove(this);
            Tree.Insert(this);
        }

        private void CalculateUnits()
        {
            for (var n = 0; n < NumVertices; n++)
            {
                int next = n == NumVertices - 1 ? 0 : n + 1;
                EdgeTangents[n] = Vertices[next] - Vertices[n];
                EdgeLengths[n] = EdgeTangents[n].Length();
                EdgeTangents[n] = EdgeTangents[n] / EdgeLengths[n];
                EdgeNormals[n] = -EdgeTangents[n].RotateClockwiseRight();
            }
        }

        private void CalculateAABB()
        {
            float top = float.MinValue;
            float bottom = float.MaxValue;
            float left = float.MaxValue;
            float right = float.MinValue;

            for (var n = 0; n < NumVertices; n++)
            {
                if (Vertices[n].X < left)
                    left = Vertices[n].X;
                if (Vertices[n].X > right)
                    right = Vertices[n].X;
                if (Vertices[n].Y < bottom)
                    bottom = Vertices[n].Y;
                if (Vertices[n].Y > top)
                    top = Vertices[n].Y;
            }

            Aabb = new AABB(top, bottom, left, right);
        }

        public void RotateClockwise(float angle, AngleType angleType = AngleType.Degrees)
        {
            for (var n = 0; n < NumVertices; n++)
            {
                Vertices[n] = Vertices[n] - Centre;
                if (angleType == AngleType.Degrees)
                {
                    Vertices[n] = Vertices[n].RotateClockwiseDegrees(angle);
                }
                else
                {
                    Vertices[n] = Vertices[n].RotateClockwiseRadians(angle);
                }
                Vertices[n] = Vertices[n] + Centre;
            }
            CalculateCentre();
            CalculateUnits();
            CalculateAABB();
            if (IsInTree)
                ReInsertInTree();
        }

        public void Translate(Vector2 translation)
        {
            for (var n = 0; n < NumVertices; n++)
            {
                Vertices[n] = Vertices[n] + translation;
            }
            CalculateCentre();
            CalculateAABB();
            if (IsInTree)
                ReInsertInTree();
        }

        public void SetPosition(Vector2 position)
        {
            for (var n = 0; n < NumVertices; n++)
            {
                Vertices[n] = Vertices[n] - Centre;
                Vertices[n] = Vertices[n] + position;
            }
            CalculateCentre();
            CalculateAABB();
            if (IsInTree)
                ReInsertInTree();
        }

        private void CalculateCentre()
        {
            var sum = Vector2.Zero;
            for (var n = 0; n < NumVertices; n++)
            {
                sum = sum + Vertices[n];
            }
            sum = sum / (1.0f * NumVertices);
            Centre = sum;
        }
    }
}
