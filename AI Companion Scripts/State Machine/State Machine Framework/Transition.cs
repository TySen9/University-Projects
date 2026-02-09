namespace PartyAISystem
{
    // Transition class that implements the InterfaceTransition interface.
    public class Transition : InterfaceTransition
    {
        /// The next state to transition to.
        public InterfaceState NextState { get; }
        public InterfacePredicate ReqCondition { get; } // The required condition for the transition to happen.

        // Constructor for the Transition class.
        public Transition(InterfaceState nextState, InterfacePredicate reqCondition)
        {
            NextState = nextState;
            ReqCondition = reqCondition;
        }
    }
}