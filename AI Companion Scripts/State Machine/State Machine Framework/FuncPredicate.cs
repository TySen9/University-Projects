using System;

namespace PartyAISystem
{
    // Functional Predicate class that implements the InterfacePredicate interface and allows for the evaluation of a boolean function.
    // It can be used to create custom predicates for state transitions specifically conditions (potentially through conditions) in the state machine.
    public class FuncPredicate : InterfacePredicate
    {
        readonly Func<bool> function;

        public FuncPredicate(Func<bool> function)
        {
            this.function = function;
        }

        public bool Evaluate() => function.Invoke();
    }
}