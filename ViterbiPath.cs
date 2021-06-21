using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_Tagging
{
    class ViterbiPath
    {
        public double value;
        public List<ViterbiNode> nodes;

        public ViterbiPath()
        {
            value = 1;
            nodes = new List<ViterbiNode>();
        }
        public ViterbiPath(ViterbiPath path)
        {
            value = path.value;
            nodes = new List<ViterbiNode>(path.nodes);
        }
    }
}
