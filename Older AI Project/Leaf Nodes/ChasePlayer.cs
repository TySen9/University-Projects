using UnityEngine;
using UnityEngine.AI;

namespace AdvancedAI.BehaviourTree
{
    public class ChasePlayer : BehaviourTreeNodes
    {
        private HunterAI hunterAI;
        private NavMeshAgent navAgent;
        private Transform player;

        private float chaseSpeed = 5.5f;
        public ChasePlayer(HunterAI hunterAI)
        {
            this.hunterAI = hunterAI;
            this.navAgent = hunterAI.GetComponent<NavMeshAgent>();
            this.player = hunterAI.GetPlayerTransform();
        }
        public override NodeState Evaluate()
        {
            if (player == null || navAgent == null)
            {
                Debug.LogError("ChasePlayer: Player or NavMeshAgent is missing.");
                return NodeState.Failure;
            }

            //Set chase speed
            navAgent.speed = chaseSpeed;

            if (!navAgent.pathPending && navAgent.remainingDistance < navAgent.stoppingDistance)
            {
                Debug.Log("ChasePlayer: Reached Player and now in attack range.");
                return NodeState.Success;
            }


            if (navAgent.isOnNavMesh)
            {
                navAgent.SetDestination(player.position);
            }

            Debug.Log("ChasePlayer: Chasing the player.");
            return NodeState.Running;
        }
    }
}
