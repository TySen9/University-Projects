using UnityEngine;

namespace AdvancedAI.BehaviourTree
{
    public class AttackPlayer : BehaviourTreeNodes
    {
        private HunterAI hunterAI;

        public AttackPlayer(HunterAI hunterAI)
        {
            this.hunterAI = hunterAI;
        }

        public override NodeState Evaluate()
        {
            hunterAI.Attack();
            Debug.Log("AttackPlayer: Attacking the Player.");
            return NodeState.Success;
        }

    }
}
