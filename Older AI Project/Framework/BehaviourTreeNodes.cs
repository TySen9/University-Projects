using UnityEngine;
using System.Collections.Generic;

namespace AdvancedAI.BehaviourTree
{
    public abstract class BehaviourTreeNodes
    {
        public enum NodeState
        {
            Success,
            Failure,
            Running
        }
        protected NodeState nodeState;
        public abstract NodeState Evaluate();
    }
}