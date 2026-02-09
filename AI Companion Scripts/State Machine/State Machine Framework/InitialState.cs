using UnityEngine;
using System;

namespace PartyAISystem
{
    public abstract class InitialState : InterfaceState
    {
        //common components that may need to be accessed.
        protected readonly CompanionController companionController;
        protected readonly Animator animator;

        //animation hashes to avoid using strings and also allow for easy access to the animator for the companions.
        protected static readonly int IdleHash = Animator.StringToHash("IdleAnim");
        protected static readonly int RunningHash = Animator.StringToHash("RunningAnim");
        protected static readonly int WalkingHash = Animator.StringToHash("WalkingAnim");
        protected static readonly int FaintHash = Animator.StringToHash("FaintAnim");
        protected static readonly int AttackHash = Animator.StringToHash("AttackAnim");
        protected static readonly int BattleHash = Animator.StringToHash("BattleIdleAnim");

        // animation transition duration values
        protected const float TransitionDuration = 0.1f;

        // The constructor for the InitialState which becomes the base line for all states in the state machine.
        protected InitialState(CompanionController companionController, Animator animator)
        {
            this.companionController = companionController;
            this.animator = animator;
            
        }

        public virtual void OnEnter()
        {

        }

        public virtual void OnExit()
        {

        }

        public virtual void Update()
        {
            
        }

        public virtual void FixedUpdate()
        {
            
        }
        
    }
}