using UnityEngine;

namespace AdvancedAI.BehaviourTree
{
    public class CheckInvestigation : BehaviourTreeNodes
    {
        private HunterAI hunterAI;
        public CheckInvestigation(HunterAI hunterAI)
        {
            this.hunterAI = hunterAI;
        }
        public override NodeState Evaluate()
        {
            if (hunterAI.Perception.IsNoiseHeard())
            {
                Debug.Log("CheckInvestigation: Noise detected");
                return NodeState.Success;
            }
            return NodeState.Failure;
        }
    }
}
