using UnityEngine;
using System.Collections.Generic;


namespace AdvancedAI.BehaviourTree
{
    public class BehaviourSelector : BehaviourTreeNodes
    {
        protected List<BehaviourTreeNodes> bNodes = new List<BehaviourTreeNodes>();

        public BehaviourSelector(List<BehaviourTreeNodes> bNodes)
        {
            this.bNodes = bNodes;
        }

        public override NodeState Evaluate()
        {
            foreach (BehaviourTreeNodes nodes in bNodes)
            {
                switch (nodes.Evaluate())
                {
                    case NodeState.Failure:
                        continue;
                    case NodeState.Success:
                        nodeState = NodeState.Success;
                        return nodeState;
                    case NodeState.Running:
                        nodeState = NodeState.Running;
                        return nodeState;
                    default:
                        continue;
                }
            }
            nodeState = NodeState.Failure;
            return nodeState;
        }
    }
}
