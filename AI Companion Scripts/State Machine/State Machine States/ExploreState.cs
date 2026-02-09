using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PartyAISystem
{
    public class ExploreState : InitialState
    {
        public ExploreBehaviourTree exploreBehaviourTree; // The explore behaviour tree for the companion

        public ExploreState(CompanionController companionController, Animator animator, ExploreBehaviourTree exploreBehaviourTree) : base(companionController, animator)
        {
            this.exploreBehaviourTree = exploreBehaviourTree;

        }

        public override void OnEnter()
        {
            Debug.Log("State Machine Explore State: Entering Explore State & Explore Behaviour");

            if (exploreBehaviourTree == null)
            {
                Debug.LogError("ExploreState: exploreBehaviourTree is null!");
                return;
            }

            exploreBehaviourTree.StartExplore(); // Start the exploration behaviour tree
            UIManager.Instance.UpdateFiniteStateText("Exploring");


        }

        public override void FixedUpdate()
        {

        }

        public override void Update()
        {

        }

        public override void OnExit()
        {
            Debug.Log("State Machine Explore State: Exiting Explore State & Explore Behaviour");
            UIManager.Instance.UpdateBehaviourExploreActionText("Inactive");
            exploreBehaviourTree.StopExplore(); // Stop the exploration behaviour tree

        }
    }

}
