using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryTools
{
    public interface ITreeItem
    {
        AABB Aabb { get; set; }
        Guid? Index { get; set; }
        ITree Tree { get; set; }
    }
}