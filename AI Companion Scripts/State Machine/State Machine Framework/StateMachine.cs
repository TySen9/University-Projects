using System;
using System.Collections.Generic;

namespace PartyAISystem
{
    public class StateMachine
    {
       StateNode currentNode; // The current state node
        Dictionary<Type, StateNode> nodes = new(); // A dictionary to store the state nodes
        HashSet<InterfaceTransition> anyTransitions = new(); // A hashset to store the any transitions

        public void Update()
       {
            // Update the current state
            currentNode.States?.Update();
            var transitions = GetTransitions(); // Get the transitions for the current state
            if (transitions != null)
               ChangeState(transitions.NextState); // Change the state if there are any transitions

        }

       public void FixedUpdate()
       {
           currentNode.States?.FixedUpdate(); // Update the current state
        }

        // / Set the state of the state machine
        public void SetState(InterfaceState states)
       {
            currentNode = nodes[states.GetType()]; // Set the current state node
            currentNode.States?.OnEnter(); // Call the OnEnter method of the current state
       }

        // Change the state of the state machine
        void ChangeState(InterfaceState states)
       {
           if (states == currentNode.States) return; // Check if the state is the same as the current state

            var previousState = currentNode.States; // Get the previous state
            var toState = nodes[states.GetType()].States; // Get the new state

            previousState?.OnExit(); // Call the OnExit method of the previous state
            toState?.OnEnter(); // Call the OnEnter method of the new state
            currentNode = nodes[states.GetType()]; // Set the current state node
        }

        /// Get the transitions for the current state
        InterfaceTransition GetTransitions()
       {
           foreach (var transition in anyTransitions) // Check for any transitions
                if (transition.ReqCondition.Evaluate()) // Check if the condition is met
                    return transition; // Return the transition

            foreach (var transition in currentNode.Transitions) // Check for the current state transitions
                // Check if the condition is met
                // If so, return the transition
                if (transition.ReqCondition.Evaluate())
                   return transition;
           
           return null;
       }

        // Adds a transition to the state machine requiring a previous state and a next state
        public void AddTransition(InterfaceState previous, InterfaceState nextState, InterfacePredicate reqCondition)
       {
           GetOrAddNode(previous).AddTransition(GetOrAddNode(nextState).States, reqCondition); //
       }

        // Adds an any transition to the state machine regardless of the current state
        public void AddAnyTransition(InterfaceState nextState, InterfacePredicate reqCondition)
       {
           anyTransitions.Add(new Transition(GetOrAddNode(nextState).States, reqCondition));
       }

        /// Gets or adds a state to the state machine
        StateNode GetOrAddNode(InterfaceState state)
       {
           var node = nodes.GetValueOrDefault(state.GetType()); // Check if the state is already in the dictionary

            if (node == null)
           {
               node = new StateNode(state); // Create a new state node
                nodes.Add(state.GetType(), node); // Add the state node to the dictionary
            }
           
           return node;
       }

        // The State Node class represents a state in the state machine and the details of the transitions to create a state.
        class StateNode
       {
           public InterfaceState States { get; }
           public HashSet<InterfaceTransition> Transitions { get; }

           public StateNode(InterfaceState states)
           {
               this.States = states;
               Transitions = new HashSet<InterfaceTransition>();
           }

           public void AddTransition(InterfaceState nextState, InterfacePredicate reqCondition)
           {
               Transitions.Add(new Transition(nextState, reqCondition));
           }
       }
    }
}