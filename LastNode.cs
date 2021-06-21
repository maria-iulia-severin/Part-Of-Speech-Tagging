using System.Collections.Generic;

namespace POS_Tagging
{
    class LastNode
    {
        //public string partOfSpeech;
        public ViterbiNode node;
        public double transitionEdge;

        public LastNode(string pos, double transition, List<ViterbiNode> prevNodes)
        {
            //partOfSpeech = pos;
            transitionEdge = transition;
            node = prevNodes.FindLast(x => x.partOfSpeech == pos);
        }
    }
}