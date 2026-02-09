using UnityEngine;

namespace AdvancedAI.BehaviourTree
{
    public class CheckPlayerSighted : BehaviourTreeNodes
    {
        private HunterAI hunterAI;

        public CheckPlayerSighted(HunterAI hunterAI)
        {
            this.hunterAI = hunterAI;
        }

        public override NodeState Evaluate()
        {
            if (hunterAI.Perception.IsPlayerSpotted())
            {
                Debug.Log("CheckPlayerSighted: Player has been Spotted.");
                return NodeState.Success;
            }
            return NodeState.Failure;
        }
    }
}
