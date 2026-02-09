using UnityEngine;

namespace PartyAISystem
{
    //This interface state works as a template for the every state that is created for the state machine.
    public interface InterfaceState
    {
        //initialises all of the methods that will be utilised in each state.
        void OnEnter();
        void OnExit();
        void Update();
        void FixedUpdate();
    }
}
