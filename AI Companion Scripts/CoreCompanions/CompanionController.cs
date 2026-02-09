using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System;

namespace PartyAISystem
{

    public class CompanionController : MonoBehaviour
    {
        [Header("Reference points")]
        StateMachine stateMachine;
        private Statistics statistics; // The statistics component for the companion
        [SerializeField] Animator animator; // The animator component for animations
        [SerializeField] GameObject companion; // The companion transform in order to check the tag of the companion
        [SerializeField] public bool isInBattle; // A boolean to check if the companion is in battle or not
        [SerializeField] public bool isInIdleZone; // A boolean to check if the companion is in idle zone or not
        [SerializeField] public bool hasFainted; // A boolean to check if the companion has fainted or not

        [Header("Behaviour Tree")]
        [SerializeField] ExploreBehaviourTree exploreBehaviourTree; // The exploration behaviour tree
        [SerializeField] BattleBehaviourTree battleBehaviourTree; // The battle behaviour tree
        [SerializeField] private UIManager uiManager; // The UI manager for the companion


        [Header("Follow Targets")]
        [SerializeField] public GameObject idleStandPoint; //  The point which the companions will go to when in idle zones



        [Header("Movement")]
        private NavMeshAgent navMeshAgent; // The NavMeshAgent component for pathfinding

        


        private void Awake()
        {
            animator = GetComponent<Animator>(); // Get the animator component
            navMeshAgent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component
            statistics = GetComponent<Statistics>(); // Get the statistics component
            exploreBehaviourTree = GetComponent<ExploreBehaviourTree>(); // Get the exploration behaviour tree
            battleBehaviourTree = GetComponent<BattleBehaviourTree>(); // Get the battle behaviour tree
            uiManager = FindAnyObjectByType<UIManager>();

            // Initialize the state machine
            stateMachine = new StateMachine();

        }

        private void Start()
        {
            // Initialize the states
            var idleState = new IdleState(this, animator, navMeshAgent, idleStandPoint);
            var battleState = new BattleState(this, animator, GetComponent<BattleBehaviourTree>());
            var exploreState = new ExploreState(this, animator, exploreBehaviourTree);
            var faintState = new FaintState(this, animator, navMeshAgent);

            //Defining the transitions of the state machine.
            At(idleState, exploreState, new FuncPredicate(() => !ReachIdleZone(isInIdleZone) && !BattleEngage(isInBattle)));
            At(idleState, battleState, new FuncPredicate(() => BattleEngage(isInBattle) && ReachIdleZone(isInIdleZone)));
            At(exploreState, idleState, new FuncPredicate(() => ReachIdleZone(isInIdleZone) && !BattleEngage(isInBattle)));
            At(exploreState, battleState, new FuncPredicate(() => BattleEngage(isInBattle) && !ReachIdleZone(isInIdleZone)));
            At(battleState, exploreState, new FuncPredicate(() => !BattleEngage(isInBattle) && !ReachIdleZone(isInIdleZone)));
            At(battleState, idleState, new FuncPredicate(() => !BattleEngage(isInBattle) && ReachIdleZone(isInIdleZone)));
            At(battleState, faintState, new FuncPredicate(() => FellinBattle(hasFainted)));
            At(faintState, battleState, new FuncPredicate(() => !FellinBattle(hasFainted)));

            ////Finds the location tags.
            //GameObject companion = GameObject.FindGameObjectWithTag("Companion A Point");
            //if (companion != null)
            //{
            //    companionPoint = companion.transform;
            //}
            //else
            //{
            //    Debug.LogError("CompanionController: Companion A Point not found in the scene.");
            //}

            stateMachine.SetState(exploreState);
        }

        //Creates the format the transitions will run in and allows for the transitions to be created here.
        void At(InterfaceState previous, InterfaceState nextState, InterfacePredicate reqCondition) => stateMachine.AddTransition(previous, nextState, reqCondition);
        void AtAny(InterfaceState nextState, InterfacePredicate reqCondition) => stateMachine.AddAnyTransition(nextState, reqCondition);

        // Update is called once per frame
        void Update()
        {
            stateMachine.Update();

            //Debug: Press J to deal damage to the companion
            if (Input.GetKeyDown(KeyCode.J))
            {
                statistics.TakeDamage(50); // Deal 50 damage to the companion
                Debug.Log($"{name}: Took 50 damage and health is now {statistics.GetCurrentHealth()}/{statistics.GetMaxHealth()}");
            }

            //Debug: Press H to heal the companion and fill mana
            if (Input.GetKeyDown(KeyCode.H))
            {
                statistics.FullHeal(); // Heal the companion
                statistics.FillMana(); // Fill the mana of the companion
                Debug.Log($"{name}: Healed to full health and health is now {statistics.GetCurrentHealth()}/{statistics.GetMaxHealth()}. Also Mana is set to max.");
            }

            //Debug: Press K to End or start Battle
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (isInBattle)
                {
                    isInBattle = false; // End the battle
                    Debug.Log($"{name}: Ended battle");
                }
                else
                {
                    isInBattle = true; // Start the battle
                    Debug.Log($"{name}: Started battle");
                }
            }

            //Debug: Press R to restart the game
            if (Input.GetKeyDown(KeyCode.R))
            {
                // Restart the game
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                Debug.Log($"{name}: Restarted the game");
            }
        }

        //
        private void FixedUpdate()
        {
            stateMachine.FixedUpdate(); // Update the state machine
        }

        //Is used to control if the companion is in the idle zone or not.
        public bool ReachIdleZone(bool isInIdleZone)
        {
            return isInIdleZone;
        }

        //Is used to control if the companion is in battle or not.
        public bool BattleEngage(bool isInBattle)
        {
            return isInBattle;
        }
        //Is used to control if the companion has fainted or not.
        public bool FellinBattle(bool hasFainted)
        {
            return hasFainted;
        }
    }
}
