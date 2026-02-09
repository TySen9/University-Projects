using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

namespace PartyAISystem
{
    public class ExploreBehaviourTree : MonoBehaviour
    {
        [Header("Explore Behaviour Tree")]
        [SerializeField] private BehaviourTreeCore exploreBehaviourTreecore; // The explore behaviour tree for the companion
        [SerializeField] private NavMeshAgent navMeshAgent; // The NavMeshAgent component for pathfinding
        [SerializeField] private Transform exploreStandPoint; //  The point which the companions will go to when in idle zones
        [SerializeField] private Transform companionEntity; // The companion transform in order to check the tag of the companion
        [SerializeField] private Transform lookPoint; // The transform of the player object
        [SerializeField] public GameObject explorePoint; // The explore point object
        [SerializeField] public GameObject lookPointObject; // The look point object
        [SerializeField] private UIManager uiManager; // The UI manager for the companion

        public bool isExplorationStateActive = false; // Flag to check if the companion is in exploration state
        public bool isActive; // Flag to check if the exploration state is active



        void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component
            uiManager = FindAnyObjectByType<UIManager>();
            GameObject exploreObject = GameObject.FindGameObjectWithTag("ExplorePoint"); // Find the player object in the scene
            if (exploreObject != null)
            {
                exploreStandPoint = exploreObject.transform; //get the transform of the explore location
            }
            else
                Debug.LogError("ExploreBehaviourTree: Explore Point not found in the scene.");

            exploreBehaviourTreecore = new BehaviourTreeCore("Explore Behaviour Tree"); // Initialize the behaviour tree

            //** Keeping the old code for reference to help understand how to use Sequencer. **
            //BehaviourLeaf isExploreFSMActive = new BehaviourLeaf("Is Explore FSM Active", new StratCondition(() => explorePoint.activeSelf)); // Create a condition node to check if the exploration state is active
            //BehaviourLeaf goToPoint = new BehaviourLeaf("Go To Explore Point", new FollowExplorePointStrategy(navMeshAgent, exploreStandPoint, lookPoint, companionEntity)); // Add the GoToExplorePoint node to the behaviour tree

            //BehaviourSequence explorationSequence = new BehaviourSequence("Exploration Sequence");// Create a sequence node for the exploration behaviour
            //explorationSequence.AddNodeChild(isExploreFSMActive); // Add the condition node to the sequence
            //explorationSequence.AddNodeChild(goToPoint); // Add the GoToExplorePoint node to the sequence

            //exploreBehaviourTree.AddNodeChild(explorationSequence); // Add the sequence node to the behaviour tree
            //** End of old code **

            BehaviourSelector exploreSelector = new BehaviourSelector("Explore Selector"); // Create a selector node for the exploration behaviour
            exploreSelector.AddNodeChild(new BehaviourLeaf("Go To Explore Point", new FollowExplorePointStrategy(navMeshAgent, exploreStandPoint, lookPoint, companionEntity, this))); // Add the GoToExplorePoint node to the behaviour tree

            exploreBehaviourTreecore.AddNodeChild(exploreSelector); // Add the selector node to the behaviour tree
        }

        // Update is called once per frame
        void Update()
        {
            if (isActive) 
            {
                Debug.Log("ExploreBehaviourTree: Processing Explore Behaviour Tree");
                exploreBehaviourTreecore.Process(); // Process the behaviour tree

            }
        }

        /// Function to check if the companion is in exploration state
        public void ToggleExploreState(bool isActive)
        {
            if (explorePoint == null)
            {
                Debug.LogError("ExploreBehaviourTree: Explore Point not assigned.");
                return;
            }
            // Toggle the exploration state
            
            explorePoint.SetActive(isActive); // Set the explore point active or inactive
            lookPointObject.SetActive(isActive); // Set the look point active or inactive
        }

        // Starts the exploration behaviour tree
        public void StartExplore()
        {
            ToggleExploreState(true); // Start the exploration state
            isActive = true; // Set the exploration state active
        }

        // Stops the exploration behaviour tree
        public void StopExplore()
        {
            ToggleExploreState(false); // Stop the exploration state
            isActive = false; // Set the exploration state inactive
        }
    }
}
