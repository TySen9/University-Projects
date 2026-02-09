namespace PartyAISystem
{
    public interface InterfaceTransition
    {
        // Gets the next state to transition to.
        InterfaceState NextState { get; }
        // gets the required condition for the transition to happen.
        InterfacePredicate ReqCondition { get; }
    }
}