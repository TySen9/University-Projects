using UnityEngine;

namespace AdvancedAI.BehaviourTree
{
    public class Despawn : BehaviourTreeNodes
    {
        private HunterAI hunterAI;
        public Despawn(HunterAI hunterAI)
        {
            this.hunterAI = hunterAI;
        }
        public override NodeState Evaluate()
        {
            if (hunterAI.ShouldDespawn())
            {
                hunterAI.Despawn();
                Debug.Log("Despawn: Despawning the Hunter");
                return NodeState.Success;
            }
            return NodeState.Failure;
        }
    }
}
