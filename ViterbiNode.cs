using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_Tagging
{
    class ViterbiNode
    {
        public int wordPositionInPhrase;
        public string partOfSpeech;
        public double probability;
        public List<LastNode> lastNodes;

        public ViterbiNode(int wordPos, string pos, double prob)
        {
            probability = prob;
            partOfSpeech = pos;
            wordPositionInPhrase = wordPos;
            lastNodes = new List<LastNode>();
        }

        public ViterbiNode(int wordPos, string pos, double prob, List<LastNode> lastNodes)
        {
            probability = prob;
            partOfSpeech = pos;
            wordPositionInPhrase = wordPos;
            this.lastNodes = lastNodes;
        }
    }
}
