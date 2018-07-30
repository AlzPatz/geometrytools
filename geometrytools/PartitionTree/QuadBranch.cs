using System.Collections.Generic;

namespace GeometryTools
{
    public class QuadBranch<T> where T : ITreeItem
    {
        public AABB Aabb;
        public bool isLeaf { get; set; }
        public QuadBranch<T>[] Branches { get; set; }
        public List<T> Items { get; set; }
        public int numItems { get; set; }

        public QuadBranch(int level, int levelOfLeaf, AABB aabb)
        {
            //No validation of level or levelOfLeaf. Rely on correct pass

            Aabb = aabb;
            Items = new List<T>();
            numItems = 0;
            isLeaf = level == levelOfLeaf;
            if (!isLeaf)
            {
                int nextlevel = level + 1;
                Branches = new QuadBranch<T>[4];
                Branches[0] = new QuadBranch<T>(nextlevel, levelOfLeaf, Aabb.Q1);
                Branches[1] = new QuadBranch<T>(nextlevel, levelOfLeaf, Aabb.Q2);
                Branches[2] = new QuadBranch<T>(nextlevel, levelOfLeaf, Aabb.Q3);
                Branches[3] = new QuadBranch<T>(nextlevel, levelOfLeaf, Aabb.Q4);
            }
        }
    }
}
