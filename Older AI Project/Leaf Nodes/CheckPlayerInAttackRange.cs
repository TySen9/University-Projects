using UnityEngine;

namespace AdvancedAI.BehaviourTree
{
    public class CheckPlayerInAttackRange : BehaviourTreeNodes
    {
        private HunterAI hunterAI;
        public CheckPlayerInAttackRange(HunterAI hunterAI)
        {
            this.hunterAI = hunterAI;
        }
        public override NodeState Evaluate()
        {
            if (hunterAI.IsPlayerInAttackRange())
            {
                Debug.Log("CheckPlayerInAttackRange: Player is in Attack Range.");
                return NodeState.Success;
            }
            return NodeState.Failure;
        }
    }
}


