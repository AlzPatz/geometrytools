using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace GeometryTools
{
    public class QuadTree<T> : ITree where T : ITreeItem
    {
        public QuadBranch<T> Root;
        public AABB Aabb;
        Dictionary<Guid?, T> _items;
        ITreeItem _lastItemAdded;
        ITreeItem _firstItemAdded;

        int _size; //Number Cells in Tree Dimension at lowerst (leaf) level
        int _depth; //Number Layers in Tree Depth
        float _leafCellDimension; //Size of cell (square), lowest (leaf) level

        public QuadTree(Vector2 centre, int minInitColumns, int minInitRows, float leafCellDimension)
        {
            if (minInitColumns <= 0)
                minInitColumns = 1;

            if (minInitRows <= 0)
                minInitRows = 2;

            if (leafCellDimension <= 0.0f)
                leafCellDimension = 1.0f;

            _items = new Dictionary<Guid?, T>();
            _lastItemAdded = default(T);
            _firstItemAdded = default(T);

            _leafCellDimension = leafCellDimension;
            int largestDimension = minInitColumns;
            if (minInitRows > minInitColumns)
                largestDimension = minInitRows;

            _depth = 1;
            _size = 1;
            do
            {
                if (largestDimension <= _size)
                    break;
                _depth++;
                _size *= 2;
            } while (true);

            float halfTreeDimension = (0.5f * _size) * _leafCellDimension;

            Vector2 topLeftPositionOfTree = centre + new Vector2(-halfTreeDimension, halfTreeDimension);
            Vector2 bottomRightPositionOfTree = centre + new Vector2(halfTreeDimension, -halfTreeDimension);

            Aabb = new AABB(topLeftPositionOfTree.Y, bottomRightPositionOfTree.Y, topLeftPositionOfTree.X, bottomRightPositionOfTree.X);
            Root = new QuadBranch<T>(1, _depth, Aabb);
        }

        public bool Insert(ITreeItem item)
        {
            return Insert((T)item);
        }

        public bool Insert(T item)
        {
            UpdateInsertRecords(item);
            IncreaseTreeSizeIfRequired(item);
            InsertInTree(Root, item);
            return true; //As tree can grow, failure is not expected (logically possible? that seems a stretch of a statement to make... )
        }

        private void UpdateInsertRecords(T item)
        {
            if (_firstItemAdded == null)
            {
                _firstItemAdded = item;
            }
            _lastItemAdded = item;
        }

        private void IncreaseTreeSizeIfRequired(T item)
        {
            while (!Aabb.Contains(item.Aabb.Min))
                StepUpTreeSize();
            while (!Aabb.Contains(item.Aabb.Max))
                StepUpTreeSize();
        }

        private void StepUpTreeSize()
        {
            QuadBranch<T> currentRoot = Root;
            _depth++;
            Aabb = new AABB(Aabb.Centre, 2.0f * Aabb.Extents);
            Root = new QuadBranch<T>(1, _depth, Aabb);

            //Copy back current tree data placed in the centre of the new Quad Tree
            Root.Branches[0].Branches[3] = currentRoot.Branches[0];
            Root.Branches[1].Branches[2] = currentRoot.Branches[1];
            Root.Branches[2].Branches[1] = currentRoot.Branches[2];
            Root.Branches[3].Branches[0] = currentRoot.Branches[3];

            //Re-insert those that were at the root level before;
            for (int i = 0; i < currentRoot.numItems; i++)
                InsertInTree(Root, currentRoot.Items[i]);
        }

        private bool InsertInTree(QuadBranch<T> branch, T item)
        {
            //Insert here if cannot test any lower
            if (branch.isLeaf)
            {
                item.Index = Guid.NewGuid();
                _items.Add(item.Index, item);
                branch.Items.Add(item);
                branch.numItems++;
                return true;
            }

            //If not a leaf, see if small enough to fit into sub-branch
            int count = 0;
            int lastIndex = -1;
            for (var c = 0; c < 4; c++)
            {
                if (AABB.CheckOverlap(branch.Branches[c].Aabb, item.Aabb))
                {
                    lastIndex = c;
                    count++;
                }
            }

            if (count <= 0)
            {
                //Fail.. shouldn't get here
                return false;
            }

            if (count > 1)
            {
                //Covers more than one sub-branch, so add to existing
                item.Index = Guid.NewGuid();
                _items.Add(item.Index, item);
                branch.Items.Add(item);
                branch.numItems++;
                return true;
            }

            //If get here, then we insert into the sub branch we found
            return InsertInTree(branch.Branches[lastIndex], item);
        }

        public bool Remove(ITreeItem item)
        {
            return FindAndRemove(Root, item);
        }

        private bool FindAndRemove(QuadBranch<T> branch, ITreeItem item)
        {
            var index = item.Index;
            if (branch.Aabb.Contains(item.Aabb))
            {
                bool inThisLevel = false;

                int count;
                do
                {
                    count = 0;
                    int lastindex = -1;
                    bool found = false;
                    for (var p = 0; p < branch.numItems; p++)
                    {
                        if (index == branch.Items[p].Index)
                        {
                            inThisLevel = true;
                            found = true;
                            lastindex = p;
                        }
                    }
                    if (found)
                    {
                        if (index == _lastItemAdded.Index)
                            _lastItemAdded = null;

                        if (index == _firstItemAdded.Index)
                            _firstItemAdded = null;

                        _items.Remove(branch.Items[lastindex].Index);
                        branch.Items.RemoveAt(lastindex);
                        branch.numItems--;
                    }

                } while (count > 0);

                if (inThisLevel)
                {
                    return true;
                }
                else
                {
                    if (branch.isLeaf)
                    {
                        return false;
                    }
                    else
                    {
                        return FindAndRemove(branch.Branches[0], item) ||
                                FindAndRemove(branch.Branches[1], item) ||
                                FindAndRemove(branch.Branches[2], item) ||
                                FindAndRemove(branch.Branches[3], item);

                    }
                }
            }
            return false;
        }

        public void RemoveAll()
        {
            _lastItemAdded = null;
            _firstItemAdded = null;
            _items.Clear();
            ClearBranch(Root);
        }

        private void ClearBranch(QuadBranch<T> branch)
        {
            branch.numItems = 0;
            branch.Items.Clear();
            if (!branch.isLeaf)
            {
                ClearBranch(branch.Branches[0]);
                ClearBranch(branch.Branches[1]);
                ClearBranch(branch.Branches[2]);
                ClearBranch(branch.Branches[3]);
            }
        }

        public List<ITreeItem> ReturnAllOverlap(AABB aabb)
        {
            var found = new List<ITreeItem>();
            if (AABB.CheckOverlap(Aabb, aabb))
                CheckBranch(Root, found, aabb);
            return found;
        }

        private void CheckBranch(QuadBranch<T> branch, List<ITreeItem> found, AABB aabb)
        {
            //Check at current branch level
            if (branch.numItems > 0)
            {
                for (var p = 0; p < branch.numItems; p++)
                {
                    if (AABB.CheckOverlap(branch.Items[p].Aabb, aabb))
                        found.Add(branch.Items[p]);
                }
            }
            //If not a final leaf check the next level down
            if (!branch.isLeaf)
            {
                for (var b = 0; b < 4; b++)
                {
                    if (AABB.CheckOverlap((branch.Branches[b]).Aabb, aabb))
                        CheckBranch(branch.Branches[b], found, aabb);
                }
            }
        }

        public ITreeItem FirstItemAdded()
        {
            return _firstItemAdded;
        }

        public ITreeItem LastItemAdded()
        {
            return _lastItemAdded;
        }

        public List<ITreeItem> ReturnAll()
        {
            return _items.Values.Select(x => (ITreeItem)x).ToList();
        }
    }
}