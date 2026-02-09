using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System;

namespace PartyAISystem 
{

    public class BattleBehaviourTree : MonoBehaviour
    {
        [Header("Battle Behaviour Tree")]
        [SerializeField] private BehaviourTreeCore battleBehaviourTreecore; // The behaviour tree core for the battle behaviour tree
        [SerializeField] private NavMeshAgent navMeshAgent; // The NavMeshAgent component for the companion
        [SerializeField] private Statistics playerStats; // The player stats object
        [SerializeField] private Statistics companionStats; // The companion stats object
        [SerializeField] private Statistics enemyStats; // The enemy stats object
        [SerializeField] private Transform targetEnemy;// The enemy transform in order to check the tag of the enemy
        [SerializeField] private Transform companionEntity; // The companion transform
        [SerializeField] private float retargetInterval = 2f; // The interval for retargeting the explore point
        [SerializeField] private float retargetTimer; // The timer for retargeting the explore point
        [SerializeField] private UIManager uiManager; // The UI manager for the companion

        [Header("Targeting System")]
        [SerializeField] private TargetingSystem targetingSystem;

        public bool isActive; // Flag to check if the battle behaviour tree is active

        void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component
            playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Statistics>(); // Get the player stats object
            companionStats = GetComponent<Statistics>(); // Get the companion stats object
            enemyStats = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Statistics>(); // Get the enemy stats object
            targetingSystem = GetComponent<TargetingSystem>(); // Get the targeting system component
            uiManager = FindAnyObjectByType<UIManager>();

            battleBehaviourTreecore = new BehaviourTreeCore("Battle Behaviour Tree"); // Initialize the behaviour tree

            // Create the nodes for the battle behaviour tree

            //Top level Selector
            BehaviourPrioritySelector battleSelector = new BehaviourPrioritySelector("Battle Root Selector"); // Create a root selector node for the battle behaviour tree

            //<-- Support Behaviour -->
            BehaviourSequence supportSequence = new BehaviourSequence("Support Sequence", 70); // Create a sequence node for the support behaviour

            // Create a condition node to check if the player is low on health
            bool IsPlayerLowHealthCheck()
            {
                if (playerStats.CurrentHealth <= playerStats.HalfHealth)
                {
                    return true; // Return true if the player is low on health
                }

                //battleSelector.Reset(); // Reset the behaviour tree if the companion is not low on mana
                supportSequence.Reset(); // Reset the sequence if the player is not low on health
                return false;
            }

            BehaviourLeaf isPlayerLowHealth = new BehaviourLeaf("Is Player Low Health?", new StratCondition(IsPlayerLowHealthCheck)); // Create a condition node to check if the player is low on health
            supportSequence.AddNodeChild(isPlayerLowHealth); // Add the condition node to the sequence

            BehaviourPrioritySelector supportPrioritySelector = new BehaviourPrioritySelector("Support Selector"); // Create a selector node for the support behaviour
            supportSequence.AddNodeChild(supportPrioritySelector); // Add the selector node to the sequence

            BehaviourSequence supportMagicSequence = new BehaviourSequence("Support Magic Sequence", 70); // Create a sequence node for the support magic behaviour

            // Create a condition node to check if the companion has enough mana for the heal spell
            bool IsCompanionManaTooLowSupportCheck()
            {
                if (companionStats.currentMana >= companionStats.healSpellCost)
                {
                    return true; // Return true if the companion is low on mana

                }
                supportMagicSequence.Reset(); // Reset the sequence if the companion is not low on mana
                //battleSelector.Reset(); // Reset the behaviour tree if the companion is not low on mana
                return false;
            }

            supportMagicSequence.AddNodeChild(new BehaviourLeaf("Check Mana for Heal", new StratCondition(IsCompanionManaTooLowSupportCheck))); // Create a condition node to check if the companion has enough mana for the heal spell
            supportMagicSequence.AddNodeChild(new BehaviourLeaf("Cast Heal Spell", new HealPlayerMagicStrategy(playerStats, companionStats))); // Create a leaf node to cast the heal spell
            supportPrioritySelector.AddNodeChild(supportMagicSequence); // Add the support magic sequence to the support selector

            BehaviourSequence supportItemSequence = new BehaviourSequence("Support Item Sequence", 60); // Create a sequence node for the support item behaviour

            // Create a condition node to check if the companion has any heal items in stock
            bool IsCompanionHealItemStockCheck()
            {
                if (companionStats.healItemStock > 0)
                {
                    return true; // Return true if the companion has no heal items in stock
                }
                supportItemSequence.Reset(); // Reset the sequence if the companion has heal items in stock
                //battleSelector.Reset(); // Reset the behaviour tree if the companion has heal items in stock
                return false;
            }
            supportItemSequence.AddNodeChild(new BehaviourLeaf("Check Item Stock", new StratCondition(IsCompanionHealItemStockCheck))); // Create a condition node to check if the companion has any heal items in stock
            supportItemSequence.AddNodeChild(new BehaviourLeaf("Use Heal Item", new HealPlayerItemStrategy(playerStats, companionStats))); // Create a leaf node to use the heal item
            supportPrioritySelector.AddNodeChild(supportItemSequence); // Add the support item sequence to the support selector
            //<-- End of Support Behaviour -->

            //<-- Attack Behaviour -->
            BehaviourPrioritySelector offenseRandomSelector = new BehaviourRandomSelector("Offense Random Selector", 50); // Create a random selector node for the offense behaviour
            

            BehaviourSequence magicAttackSequence = new BehaviourSequence("Magic Attack Sequence", 50); // Create a sequence node for the magic attack behaviour

            // Create a condition node to check if the companion has enough mana for the attack spell
            bool IsCompanionManaTooLowAttackCheck()
            {
                bool manaCheck = companionStats.currentMana >= companionStats.attackSpellCost; // Check if the companion has enough mana for the attack spell
                bool nullTarget = targetEnemy == null; // Check if the target enemy is null
                if (manaCheck && !nullTarget)
                {
                    Debug.Log("BattleBehaviourTree: Companion has enough mana for attack spell"); // Log the companion isn't low on mana
                    return true; // Return true if the companion is low on mana
                }
                magicAttackSequence.Reset(); // Reset the sequence if the companion is low on mana
                //battleSelector.Reset(); // Reset the behaviour tree if the companion is low on mana
                return false;
            }

            BehaviourLeaf checkManaForAttack = new BehaviourLeaf("Check Mana for Attack", new StratCondition(IsCompanionManaTooLowAttackCheck)); // Create a condition node to check if the companion has enough mana for the attack spell
            BehaviourLeaf castMagicAttack = new BehaviourLeaf("Cast Magic Attack", new MagicAttackEnemyStrategy(targetingSystem.Tracker, companionStats)); // Create a leaf node to cast the magic attack spell
            BehaviourLeaf getInMagicAttackRange = new BehaviourLeaf("Get In Magic Attack Range", new GetInAttackRangeStrategy(companionEntity, navMeshAgent, targetingSystem.Tracker, 6)); // Create a leaf node to move to the enemy
            magicAttackSequence.AddNodeChild(checkManaForAttack); // Add the condition node to the sequence
            magicAttackSequence.AddNodeChild(getInMagicAttackRange); // Add the get in magic attack range node to the sequence
            magicAttackSequence.AddNodeChild(castMagicAttack); // Add the cast magic attack node to the sequence
            offenseRandomSelector.AddNodeChild(magicAttackSequence); // Add the magic attack sequence to the random selector

            BehaviourSequence attackSequence = new BehaviourSequence("Attack Sequence", 50); // Create a sequence node for the attack behaviour
            //BehaviourLeaf attackEnemy = new BehaviourLeaf("Attack Enemy", new AttackEnemyStrategy(EnemyStats)); // Create a leaf node to attack the enemy
            BehaviourLeaf getInAttackRange = new BehaviourLeaf("Get In Attack Range", new GetInAttackRangeStrategy(companionEntity, navMeshAgent, targetingSystem.Tracker, 3)); // Create a leaf node to move to the enemy
            attackSequence.AddNodeChild(new BehaviourLeaf("Get In Attack Range", new GetInAttackRangeStrategy(companionEntity, navMeshAgent, targetingSystem.Tracker, 3))); // Add the get in attack range node to the sequence
            attackSequence.AddNodeChild(new BehaviourLeaf("Attack Enemy", new AttackEnemyStrategy(targetingSystem.Tracker))); // Add the attack enemy node to the sequence
            offenseRandomSelector.AddNodeChild(attackSequence); // Add the attack sequence to the random selector
            //<-- End of Attack Behaviour -->

            //<-- Real Battle Behaviour Tree -->
            // populate the behaviour tree with the nodes to have the following structure

            battleSelector.AddNodeChild(supportSequence); // Add the support sequence to the selector
            battleSelector.AddNodeChild(offenseRandomSelector); // Add the offense random selector to the selector
            battleSelector.AddNodeChild(new BehaviourLeaf("Idle", new IdleStrategy(navMeshAgent, companionEntity), 0)); // Add the idle node to the selector

            battleBehaviourTreecore.AddNodeChild(battleSelector); // Add the root selector node to the behaviour tree
            battleBehaviourTreecore.PrintTree(); // Print the behaviour tree for debugging
            /*
              The behaviour tree will now have the following structure:
              
              Battle Root Selector
              ├── Support Sequence
              │   ├── Is Player Low Health?
              │   └── Support Selector
              │       ├── Support Magic Sequence
              │       │   ├── Check Mana for Heal
              │       │   └── Cast Heal Spell
              │       └── Support Item Sequence
              │           ├── Check Item Stock
              │           └── Use Heal Item
              ├── Offense Random Selector
              │   ├── Magic Attack Sequence
              │   │   ├── Check Mana for Attack
              │   │   ├── Get In Magic Attack Range
              │   │   └── Cast Magic Attack
              │   └── Attack Sequence
              │       ├── Get In Attack Range
              │       └── Attack Enemy
              └── Idle
            */

            //<-- End of Real Battle Behaviour Tree -->

            //<-- Debug Tree for testing -->
            //battleBehaviourTreecore = new BehaviourTreeCore("Debug Battle Tree");
            //battleBehaviourTreecore.AddNodeChild(new BehaviourLeaf("Debug Attack Enemy", new DebugAttackStrategy())); // Create a leaf node to attack the enemy
            //BehaviourSequence debugAttackSequence = new BehaviourSequence("Debug Attack Sequence", 50); // Create a sequence node for the attack behaviour
            //debugAttackSequence.AddNodeChild(getInAttackRange);
            //debugAttackSequence.AddNodeChild(new BehaviourLeaf("Attack Enemy", new AttackEnemyStrategy(enemyStats)));


            //battleBehaviourTreecore.AddNodeChild(debugAttackSequence);

        }

        // Update is called once per frame
        void Update()
        {
            if (isActive)
            {
                retargetTimer += Time.deltaTime; // Increment the retarget timer
                if (retargetTimer >= retargetInterval) // Check if the retarget timer has reached the interval
                {
                    retargetTimer = 0f; // Reset the retarget timer
                    targetingSystem.Retarget(); // Retarget the enemy

                    if (targetingSystem.Tracker.HasTarget)
                    {
                        targetEnemy = targetingSystem.Tracker.TargetTransform; //Get the target enemy transform
                        enemyStats = targetingSystem.Tracker.TargetStats; //Get the target enemy stats
                    }
                    else
                    {
                        targetEnemy = null; // Set the target enemy to null if no target is found
                        enemyStats = null; // Set the target enemy stats to null if no target is found
                        battleBehaviourTreecore.Reset(); // Reset the behaviour tree if no target is found. A fallback for if there are no enemies in the scene.
                    }
                }

                Debug.Log("BattleBehaviourTree: Processing Battle Behaviour Tree");
                battleBehaviourTreecore.Process(); // Process the behaviour tree
            }
        }

        //public void FindClosestEnemy()
        //{
        //    var allEnemies = GameObject.FindGameObjectsWithTag("Enemy"); // Find all enemies in the scene
        //    float areaReach = 15f;
        //    Transform best = null;
        //    float bestDistance = float.MaxValue; // Initialize the best distance to the maximum value

        //    foreach (var go in allEnemies)
        //    {
        //        var pos = go.transform.position; // Get the position of the enemy
        //        if (NavMesh.SamplePosition(pos, out NavMeshHit hit, areaReach, navMeshAgent.areaMask))
        //        {
        //            float dist = Vector3.Distance(transform.position, hit.position); // Calculate the distance to the enemy
        //            if (dist < bestDistance)
        //            {
        //                bestDistance = dist; // Update the best distance
        //                best = go.transform; // Update the best enemy transform
        //            }
        //        }
        //    }

        //    if (best != null)
        //    {
        //        targetEnemy = best;
        //        Debug.Log("BattleBehaviourTree: Target Enemy Found: " + targetEnemy.name); // Log the target enemy
        //    }
        //    else
        //    {
        //        Debug.LogWarning("BattleBehaviourTree: No enemies found in the area"); // Log a warning if no enemies are found
        //        battleBehaviourTreecore.Reset(); // Reset the behaviour tree
        //        return;
        //    }
        //}

        public void StartBattle()
        {
            isActive = true; // Set the battle behaviour tree to active
            Debug.Log("BattleBehaviourTree: Battle Behaviour Tree Started");
        }

        public void StopBattle()
        {
            isActive = false; // Set the battle behaviour tree to inactive
            Debug.Log("BattleBehaviourTree: Battle Behaviour Tree Stopped");
        }

    }
}
