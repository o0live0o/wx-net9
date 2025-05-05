using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wx.Infrastructure;

public class LRUCache
{
    
    private class Node
    {
        public int Key { get; set; }
        public int Value { get; set; }

        public Node Prev { get; set; }
        public Node Next { get; set; }
    }

    
}
