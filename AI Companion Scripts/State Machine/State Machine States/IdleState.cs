using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PartyAISystem
{
    public class IdleState : InitialState
    {
        public readonly GameObject idleStandPoint; // The waypoints the companion will go to in idle zones
        readonly NavMeshAgent navMeshAgent; // The navmesh agent for the companion
        readonly float walkToSpeed = 6f; // The speed of the companion when patrolling

        public IdleState(CompanionController companionController, Animator animator, NavMeshAgent navMeshAgent, GameObject idleStandPoint ) : base(companionController, animator)
        {
            this.navMeshAgent = navMeshAgent;
            this.idleStandPoint = idleStandPoint;
        }

        void start()
        {
            
        }

        public override void OnEnter()
        {
            Debug.Log("State Machine Idle State: Entering Idle State");
            animator.CrossFade(WalkingHash, TransitionDuration); // Crossfade to the idle animation
            GoToIdleSpot(); // Call the function to go to the idle spot
            UIManager.Instance.UpdateFiniteStateText("Idle Mode");

        }

        public override void FixedUpdate()
        {

        }

        public override void Update()
        {
            navMeshAgent.speed = walkToSpeed; // Set the speed of the navmesh agent to the walking speed
            // Check if the companion is in the idle zone

            if (HasReachedIdleSpot())
            {
                // If the companion has reached the idle spot, stop the navmesh agent and play the idle animation
                navMeshAgent.isStopped = true; // stop the navmesh agent
                animator.CrossFade(IdleHash, TransitionDuration); // Crossfade to the idle animation

            }
        }

        private void GoToIdleSpot()
        {
            // Set the destination of the navmesh agent to the idle spot
            navMeshAgent.SetDestination(idleStandPoint.transform.position);
            navMeshAgent.isStopped = false; // allows the navmesh agent to move
        }

        bool HasReachedIdleSpot()
        {
            // Check if the companion has reached the idle spot
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                return true;
            }
            return false;
        }

        public override void OnExit()
        {
            Debug.Log("State Machine Idle State: Exiting Idle State");
            navMeshAgent.isStopped = false; // allows the navmesh agent to move
        }

    }

}
