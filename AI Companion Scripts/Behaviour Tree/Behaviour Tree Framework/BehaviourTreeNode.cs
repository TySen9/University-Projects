using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;

namespace PartyAISystem 
{
    //The InterfacePolicy interface defines a policy for the behaviour tree to determine how to process the nodes and run. (a defualt policy)
    public interface InterfacePolicy
    {
        bool ShouldReturn(BehaviourTreeNode.NodeStatus nodeStatus);
    }

    // policies for the behaviour tree to determine how to process the nodes and run
    public static class Policies
    {
        public static readonly InterfacePolicy RunForever = new RunForeverPolicy();
        public static readonly InterfacePolicy RunUntilSuccess = new RunUntilSuccessPolicy();
        public static readonly InterfacePolicy RunUntilFailure = new RunUntilFailurePolicy();

        class RunForeverPolicy : InterfacePolicy
        {
            public bool ShouldReturn(BehaviourTreeNode.NodeStatus nodeStatus) => false;
        }
        class RunUntilSuccessPolicy : InterfacePolicy
        {
            public bool ShouldReturn(BehaviourTreeNode.NodeStatus nodeStatus) => nodeStatus == BehaviourTreeNode.NodeStatus.Success;
        }
        class RunUntilFailurePolicy : InterfacePolicy
        {
            public bool ShouldReturn(BehaviourTreeNode.NodeStatus nodeStatus) => nodeStatus == BehaviourTreeNode.NodeStatus.Failure;
        }
    }

    // The BehaviourTreeCore class is the root node of the behaviour tree.
    public class BehaviourTreeCore : BehaviourTreeNode
    {
        readonly InterfacePolicy policy;
        public BehaviourTreeCore(string stateName, InterfacePolicy policy = null) : base(stateName) {
            this.policy = policy ?? Policies.RunForever; // Set the policy to run forever by default
        } // Constructor for BehaviourTreeCore


        public override NodeStatus Process()
        {
            // Check if the current node child is within the bounds of the node children list
            NodeStatus nodeStatus = nodeChildren[currentNodeChild].Process();
            if (policy.ShouldReturn(nodeStatus))
            {
                //Reset(); // Reset the behaviour tree if the policy condition is met
                return nodeStatus; // Return the status of the child node
            }

            currentNodeChild = (currentNodeChild + 1) % nodeChildren.Count; // Move to the next child node in a circular manner
            return NodeStatus.Running; // Return running status if the policy condition is not met
        }

        //prints the behaviour tree to the console
        public void PrintTree()
        {
            StringBuilder sb = new StringBuilder(); // Create a StringBuilder to build the tree string
            PrintNode(this, 0, sb); // Recursively print the nodes
            Debug.Log(sb.ToString()); // Print the tree string to the console
        }

        // Recursively prints the behaviour tree nodes with indentation
        static void PrintNode(BehaviourTreeNode behaviourTreeNode, int indentLevel, StringBuilder sb)
        {
            sb.Append(' ', indentLevel * 2).AppendLine(behaviourTreeNode.stateName); // Add indentation and node name to the StringBuilder
            foreach (BehaviourTreeNode nodeChild in behaviourTreeNode.nodeChildren) // Iterate through the child nodes
            {
                PrintNode(nodeChild, indentLevel + 1, sb); // Recursively print the child nodes including indentation
            }
        }

    }

    // The BehaviourRandomSelector class is a random selector node in the behaviour tree that processes its child nodes in a random order.
    public class BehaviourRandomSelector : BehaviourPrioritySelector
    {
        protected override List<BehaviourTreeNode> SortNodeChildren() => nodeChildren.Shuffle().ToList(); // Shuffle the child nodes randomly

        public BehaviourRandomSelector(string stateName, int priority = 0) : base(stateName, priority) { } // Constructor for BehaviourRandomSelector
    }

    // This is the same as a standard selector node, but it sorts the child nodes by priority before processing them.
    public class BehaviourPrioritySelector : BehaviourSelector
    {
        List<BehaviourTreeNode> sortedNodeChildren; // Store the sorted child nodes
        List<BehaviourTreeNode> SortedNodeChildren => sortedNodeChildren ??= SortNodeChildren(); // Initialize the sorted child nodes if they are null

        protected virtual List<BehaviourTreeNode> SortNodeChildren() => nodeChildren.OrderByDescending(nodeChild => nodeChild.priority).ToList(); // Sort the child nodes by priority

        public BehaviourPrioritySelector(string stateName, int priority = 0) : base(stateName, priority) { } // Constructor for BehaviourPrioritySelector

        public override void Reset()
        {
            base.Reset();
            sortedNodeChildren = null; // Reset the sorted node children
        }

        public override NodeStatus Process()
        {
            foreach (var nodeChild in SortedNodeChildren) // Iterate through the sorted child nodes
            {
                switch (nodeChild.Process()) // Process each child node
                {
                    case NodeStatus.Running:
                        return NodeStatus.Running; // Priority selector is still running
                    case NodeStatus.Success:
                        return NodeStatus.Success; // Priority selector succeeded
                    default:
                        continue; // Move to the next child node
                }
            }

            return NodeStatus.Failure; // All child nodes processed and none succeeded
        }
    }

    // The UntilFail class is a node in the behaviour tree that continues processing its child node until it fails.
    // Discovered this till later and would solve my problem of the AI following while explore is active.
    // but I'm keeping it the way it is for now due to not wanting to break anything. (Good for future reference)
    public class UntilFail : BehaviourTreeNode 
    {
        public UntilFail(string stateName) : base(stateName) { } // Constructor for UntilFail
        public override NodeStatus Process()
        {
            if (nodeChildren[0].Process() == NodeStatus.Failure)
            {
                Reset(); // Reset the node if it fails
                return NodeStatus.Failure; // Node failed
            }

            return NodeStatus.Running; // Node is still running
        }
    }

    // The BehaviourSelector class is a selector node in the behaviour tree that processes
    // its child nodes in an order depending on the which gets completed first.
    public class BehaviourSelector : BehaviourTreeNode
    {
        public BehaviourSelector(string stateName, int priority = 0) : base(stateName, priority) { } // Constructor for BehaviourSelector
        public override NodeStatus Process()
        {
            if (currentNodeChild < nodeChildren.Count) // Check if the current node child is still within the bounds of the node children list
            {
                switch (nodeChildren[currentNodeChild].Process()) {
                    case NodeStatus.Running:
                        return NodeStatus.Running; // Selector is still running
                    case NodeStatus.Success:
                        Reset(); // Reset the selector
                        return NodeStatus.Success; // Selector succeeded
                    default:
                        currentNodeChild++;
                        return NodeStatus.Running; // Move to the next child node
                }
            }

            Reset(); // Reset the selector if all child nodes have been processed
            return NodeStatus.Failure; // All child nodes processed and none succeeded
        }
    }

    // The BehaviourSequence class is a sequence node in the behaviour tree that processes its child nodes in an order.
    public class BehaviourSequence : BehaviourTreeNode
    {
        public BehaviourSequence(string stateName, int priority = 0) : base(stateName, priority) { } // Constructor for BehaviourSequence

        public override NodeStatus Process()
        {
            if (currentNodeChild < nodeChildren.Count)
            {
                switch (nodeChildren[currentNodeChild].Process()) { 
                    case NodeStatus.Running:
                        return NodeStatus.Running; // Sequence is still running
                    case NodeStatus.Failure:
                        Reset(); // Reset the sequence
                        return NodeStatus.Failure; // Sequence failed
                    default:
                        currentNodeChild++;
                        return currentNodeChild == nodeChildren.Count ? NodeStatus.Success : NodeStatus.Running; // Move to the next child node
                }
            }

            Reset(); // Reset the sequence if all child nodes have been processed
            return NodeStatus.Success; // All child nodes processed successfully
        }
    }

    // The BehaviourInverter class is an inverter node in the behaviour tree that inverts the result of a child node.
    public class  BehaviourInverter : BehaviourTreeNode
    {
        public BehaviourInverter(string name) : base(name) { }

        public override NodeStatus Process()
        {
            switch (nodeChildren[0].Process())
            {
                case NodeStatus.Running:
                    return NodeStatus.Running; // Inverter is still running
                case NodeStatus.Failure:
                    return NodeStatus.Success; // Inverter succeeded
                default:
                    return NodeStatus.Failure;
            }
        }
    }

    // The BehaviourLeaf class is a leaf node in the behaviour tree that executes a specific action or condition.
    public class  BehaviourLeaf : BehaviourTreeNode
    {
        readonly InterfaceStrategy strategy;

        public BehaviourLeaf(string stateName, InterfaceStrategy strategy, int priority = 0) : base(stateName, priority)
        {
            this.strategy = strategy;
        }

        public override NodeStatus Process() => strategy.Process();

        public override void Reset() => strategy.Reset();
    }

    // The BehaviourTreeNode class is the base class for all nodes in the behaviour tree.
    public class BehaviourTreeNode
    {
        public enum NodeStatus{ Running, Success, Failure }

        public readonly string stateName;
        public readonly int priority;

        public readonly List<BehaviourTreeNode> nodeChildren = new();
        protected int currentNodeChild;

        public BehaviourTreeNode(string stateName = "Behaviour Node", int priority = 0)
        {
            this.stateName = stateName;
            this.priority = priority;
        }

        public void AddNodeChild(BehaviourTreeNode nodeChild) => nodeChildren.Add(nodeChild); // Add a child node to the behaviour tree

        public virtual NodeStatus Process() => nodeChildren[currentNodeChild].Process(); // Process the current child node

        public virtual void Reset() //Reset the behaviour tree node
        {
            currentNodeChild = 0;
            foreach (var nodeChild in nodeChildren)
            {
                nodeChild.Reset();
            }
        }
    }

}
