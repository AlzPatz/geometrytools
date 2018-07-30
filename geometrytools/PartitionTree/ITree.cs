using System.Collections.Generic;

namespace GeometryTools
{
    public interface ITree
    {
        bool Remove(ITreeItem item);
        bool Insert(ITreeItem item);
        List<ITreeItem> ReturnAllOverlap(AABB aabb);
        void RemoveAll();
        List<ITreeItem> ReturnAll();
        ITreeItem FirstItemAdded();
        ITreeItem LastItemAdded();
    }
}