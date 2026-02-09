using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

namespace AdvancedAI.BehaviourTree
{
    public class Patrol : BehaviourTreeNodes
    {
        private HunterAI hunterAI;
        private NavMeshAgent navAgent;
        private List<Vector3> patrolPositions;
        private Vector3 currentDestination;

        private float patrolSpeed = 3.5f; // Slower speed for patrolling.

        public Patrol(HunterAI hunterAI, List<Vector3> patrolPoints)
        {
            this.hunterAI = hunterAI;
            this.navAgent = hunterAI.GetComponent<NavMeshAgent>();
            this.patrolPositions = patrolPoints;
            ChooseRandomPatrolPoint();
        }
        public override NodeState Evaluate()
        {
            if (patrolPositions.Count == 0)
            {
                Debug.LogError("Patrol: No patrol points set.");
                return NodeState.Failure;
            }

            // Set patrol speed
            navAgent.speed = patrolSpeed;

            if (!navAgent.pathPending && navAgent.remainingDistance < 0.5f)
            {
                ChooseRandomPatrolPoint();
                return NodeState.Success;
            }
            Debug.Log("Patrol: Patrolling the Area");
            return NodeState.Running;
        }

        private void ChooseRandomPatrolPoint()
        {
            if (navAgent == null) return; //stops issues from happening if the navAgent is null.

            int randomIndex = Random.Range(0, patrolPositions.Count);
            currentDestination = patrolPositions[randomIndex];

            if (navAgent.isOnNavMesh) //Checks if the navAgent is on the navmesh.
            {
                navAgent.SetDestination(currentDestination);
            }
        }
    }
}
