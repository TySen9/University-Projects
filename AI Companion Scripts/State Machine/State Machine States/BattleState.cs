using UnityEngine;

namespace PartyAISystem
{
    public class BattleState : InitialState
    {
        public BattleBehaviourTree battlebehaviourTree; // The battle behaviour tree for the companion
        public BattleState(CompanionController companionController, Animator animator, BattleBehaviourTree battlebehaviourTree) : base(companionController, animator)
        {
            this.battlebehaviourTree = battlebehaviourTree;

        }

        public override void OnEnter()
        {
            Debug.Log("State Machine Battle State: Entering Battle State");
            animator.CrossFade(BattleHash, TransitionDuration); // Crossfade to the battle animation
            battlebehaviourTree.StartBattle(); // Start the battle behaviour tree
            UIManager.Instance.UpdateFiniteStateText("In-Battle");
        }

        public override void FixedUpdate()
        {

        }

        public override void Update()
        {

        }

        public override void OnExit()
        {
            Debug.Log("State Machine Battle State: Exiting Battle State");
            animator.CrossFade(IdleHash, TransitionDuration); // Crossfade to the idle animation
            UIManager.Instance.UpdateBehaviourBattleActionText("Inactive");
            battlebehaviourTree.StopBattle(); // Stop the battle behaviour tree
        }
    }

}
