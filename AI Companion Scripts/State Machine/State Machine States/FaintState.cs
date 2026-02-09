using UnityEngine;
using UnityEngine.AI;

namespace PartyAISystem
{
    public class FaintState : InitialState
    {
        readonly NavMeshAgent navMeshAgent; // The navmesh agent for the companion
        public FaintState(CompanionController companionController, Animator animator, NavMeshAgent navMeshAgent) : base(companionController, animator)
        {
            this.navMeshAgent = navMeshAgent;
        }

        public override void OnEnter()
        {
            Debug.Log("State Machine Faint State: Entering Faint State");
            Debug.Log("State Machine Faint State: Companion has fainted!");
            animator.CrossFade(FaintHash, TransitionDuration); // Crossfade to the faint animation
            navMeshAgent.isStopped = true; // Stop the navmesh agent
            UIManager.Instance.UpdateFiniteStateText("Fainted");

        }

        public override void FixedUpdate()
        {

        }

        public override void Update()
        {

        }

        public override void OnExit()
        {
            Debug.Log("State Machine Faint State: Exiting Faint State");
            Debug.Log("State Machine Faint State: Companion has recovered!");
            navMeshAgent.isStopped = false; // Resume the navmesh agent

        }
    }

}
