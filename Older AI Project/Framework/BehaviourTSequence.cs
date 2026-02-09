using UnityEngine;
using System.Collections.Generic;


namespace AdvancedAI.BehaviourTree
{
    public class BehaviourTSequence : BehaviourTreeNodes
    {
        protected List<BehaviourTreeNodes> bNodes = new List<BehaviourTreeNodes>();
        public BehaviourTSequence(List<BehaviourTreeNodes> bNodes)
        {
            this.bNodes = bNodes;
        }
        public override NodeState Evaluate()
        {
            bool areNodesRunning = false;
            foreach (BehaviourTreeNodes nodes in bNodes)
            {
                switch (nodes.Evaluate())
                {
                    case NodeState.Failure:
                        nodeState = NodeState.Failure;
                        return nodeState;
                    case NodeState.Success:
                        continue;
                    case NodeState.Running:
                        areNodesRunning = true;
                        continue;
                    default:
                        nodeState = NodeState.Success;
                        return nodeState;
                }
            }
            nodeState = areNodesRunning ? NodeState.Running : NodeState.Success;
            return nodeState;
        }
    }
}
