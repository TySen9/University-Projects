using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System;


namespace PartyAISystem 
{
    //Template for the strategy pattern to be used in the behaviour tree.
    public interface InterfaceStrategy
    {
        BehaviourTreeNode.NodeStatus Process();
        void Reset()
        {
            //No default implementation
        }
    }

    //An empty Strategy to help understand how to use stratergies and can used for debugging and testing or even filled out with own personal method.
    public class ActionStrategy : InterfaceStrategy
    {
        readonly Action doAction;

        public ActionStrategy(Action doAction)
        {
            this.doAction = doAction;
        }

        public BehaviourTreeNode.NodeStatus Process()
        {
            doAction();
            return BehaviourTreeNode.NodeStatus.Success;
        }
    }

    //A condition strategy to check if a condition is met and return success or failure.
    //The condition that is a bool can be passed in as a function when creating the strategy in the tree.
    public class StratCondition : InterfaceStrategy
    {
        readonly Func<bool> predicate; // The condition to check if the condition is met

        public StratCondition(Func<bool> predicate)
        {
            this.predicate = predicate;
        }

        public BehaviourTreeNode.NodeStatus Process() => predicate() ? BehaviourTreeNode.NodeStatus.Success : BehaviourTreeNode.NodeStatus.Failure; // Return success if the condition is met, otherwise return failure

    }

    //A strategy to heal the player using magic
    public class HealPlayerMagicStrategy : InterfaceStrategy
    {
        readonly Statistics playerStats;
        readonly Statistics companionStats;
        private const string abilityKey = "HealMagic"; // The key for the attack ability
        private const string groupKey = "Defense"; // The key for the attack group
        private const float abilityCooldown = 10f; // The cooldown for the attack ability
        private const float groupCooldown = 8f; // The cooldown for the attack group

        public HealPlayerMagicStrategy(Statistics playerStats, Statistics companionStats)
        {
            this.playerStats = playerStats;
            this.companionStats = companionStats;

        }

        public BehaviourTreeNode.NodeStatus Process() {

            if (!CooldownManager.CanUse(abilityKey, groupKey))
            {
                
                return BehaviourTreeNode.NodeStatus.Running; // Return running
            }

            playerStats.HealAction(70); // Heal the player
            companionStats.UseManaHealCost(); // Use the mana for the heal spell
            Debug.Log("HealPlayerMagicStrategy: Healing player with magic"); // Log the healing
            CooldownManager.TriggerCooldownWithGroup(abilityKey, abilityCooldown, groupKey, groupCooldown); // Trigger the cooldown for the ability and group
            UIManager.Instance.UpdateBehaviourBattleActionText("Healing Player - Magic"); // Update the UI text for the attack action
            UIManager.Instance.ShowRandomHealSubtitle(); // Show a random heal subtitle
            return BehaviourTreeNode.NodeStatus.Success; // Return success
        }
    }

    //A strategy to heal the player using an item
    public class HealPlayerItemStrategy : InterfaceStrategy
    {
        readonly Statistics playerStats; // The statistics of the player
        readonly Statistics companionStats; // The statistics of the companion
        private const string abilityKey = "HealItem"; // The key for the individual ability
        private const string groupKey = "Defense"; // The key for the ability group
        private const float abilityCooldown = 8f; // The cooldown for the individual ability
        private const float groupCooldown = 6f; // The cooldown for the ability group
        public HealPlayerItemStrategy(Statistics playerStats, Statistics companionStats)
        {
            this.playerStats = playerStats;
            this.companionStats = companionStats;
        }
        public BehaviourTreeNode.NodeStatus Process()
        {
            if (!CooldownManager.CanUse(abilityKey, groupKey))
            {

                return BehaviourTreeNode.NodeStatus.Running; // Return running
            }

            playerStats.HealAction(10); // Heal the player
            companionStats.UseItemHealCost(); // Use the item for the heal spell
            Debug.Log("HealPlayerItemStrategy: Healing player with item"); // Log the healing
            CooldownManager.TriggerCooldownWithGroup(abilityKey, abilityCooldown, groupKey, groupCooldown); // Trigger the cooldown for the ability and group
            UIManager.Instance.UpdateBehaviourBattleActionText("Healing Player - Item"); // Update the UI text for the attack action
            UIManager.Instance.ShowRandomHealSubtitle();
            return BehaviourTreeNode.NodeStatus.Success; // Return success
        }
    }

    //A strategy to attack the enemy using a melee attack
    public class AttackEnemyStrategy : InterfaceStrategy
    {
        readonly TargetTracker targetTracker;
        private const string abilityKey = "NormalAttack"; // The key for the attack ability
        private const string groupKey = "Physical"; // The key for the attack group
        private const float abilityCooldown = 5f; // The cooldown for the attack ability
        private const float groupCooldown = 4f; // The cooldown for the attack group

        public AttackEnemyStrategy(TargetTracker targetTracker)
        {

            this.targetTracker = targetTracker;
        }
        public BehaviourTreeNode.NodeStatus Process()
        {
            if (!CooldownManager.CanUse(abilityKey, groupKey))
            {
                //Debug.Log($"AttackEnemyStrategy: Ability {abilityKey} is on cooldown for {CooldownManager.TimeRemaining(abilityKey)} seconds"); // Log the cooldown
                return BehaviourTreeNode.NodeStatus.Running; // Return running
            }

            targetTracker.TargetStats.TakeDamage(40); // Attack the enemy
            Debug.Log("AttackEnemyStrategy: Attacking enemy with melee"); // Log the attack
            CooldownManager.TriggerCooldownWithGroup(abilityKey, abilityCooldown, groupKey, groupCooldown); // Trigger the cooldown for the attack ability and group
            UIManager.Instance.UpdateBehaviourBattleActionText("Attacking Enemy - Physical"); // Update the UI text for the attack action
            UIManager.Instance.ShowRandomBattleSubtitle();
            return BehaviourTreeNode.NodeStatus.Success; // Return success
        }
    }

    //A strategy to attack the enemy using a magic attack
    public class MagicAttackEnemyStrategy : InterfaceStrategy
    {
        readonly TargetTracker targetTracker; // The target tracker for the enemy
        readonly Statistics companionStats; // The statistics of the companion
        private const string abilityKey = "MagicAttack"; // The key for the individual ability
        private const string groupKey = "Magic"; // The key for the ability group
        private const float abilityCooldown = 12f; // The cooldown for the individual ability
        private const float groupCooldown = 10f; // The cooldown for the ability group
        public MagicAttackEnemyStrategy(TargetTracker targetTracker, Statistics companionStats)
        {
            this.targetTracker = targetTracker;
            this.companionStats = companionStats;
        }
        public BehaviourTreeNode.NodeStatus Process()
        {
            // Check if cooldown is available for the ability and group
            if (!CooldownManager.CanUse(abilityKey, groupKey))
            {

                return BehaviourTreeNode.NodeStatus.Running; // Return running
            }

            targetTracker.TargetStats.TakeDamage(60); // Attack the enemy and current target takes damage
            companionStats.UseManaAttackCost(); // Use the mana for the attack spell
            Debug.Log("MagicAttackEnemyStrategy: Attacking enemy with magic"); // Log the attack
            CooldownManager.TriggerCooldownWithGroup(abilityKey, abilityCooldown, groupKey, groupCooldown); // Trigger the cooldown for the ability and group
            UIManager.Instance.UpdateBehaviourBattleActionText("Attacking Enemy - Magic"); // Update the UI text for the attack action
            UIManager.Instance.ShowRandomMagicSubtitle(); // Show a random battle subtitle
            return BehaviourTreeNode.NodeStatus.Success; // Return success
        }
    }

    //A strategy to get the companion in attack range of the enemy
    public class GetInAttackRangeStrategy : InterfaceStrategy
    {
        readonly NavMeshAgent navMeshAgent;
        readonly TargetTracker targetTracker;
        readonly Transform companionEntity;
        public float attackRange = 2.0f; // The range of the attack
        public float moveSpeed = 6.0f; // The speed of the NavMeshAgent
        public GetInAttackRangeStrategy(Transform companionEntity, NavMeshAgent navMeshAgent, TargetTracker targetTracker, float attackRange = 3)
        {
            this.navMeshAgent = navMeshAgent; // Get the NavMeshAgent component
            this.targetTracker = targetTracker; // Get the enemy transform
            this.companionEntity = companionEntity; // Get the companion entity
        }
        // The companion will move to the enemy and attack
        public BehaviourTreeNode.NodeStatus Process()
        {
            var targetEnemy = targetTracker.TargetTransform; // Get the target enemy transform
            if (targetEnemy == null) // Check if the target enemy is null
            {
                Debug.Log("GetInAttackRangeStrategy: Target enemy is null"); // Log the error
                return BehaviourTreeNode.NodeStatus.Failure; // Return failure
            }
    
            Vector3 direction = (targetEnemy.position - companionEntity.position); // Get the direction to the enemy
            direction.y = 0; // Set the y position to 0
            float distance = direction.magnitude; // Get the distance to the enemy
            navMeshAgent.speed = moveSpeed; // Set the speed of the NavMeshAgent

            if (distance <= attackRange + navMeshAgent.stoppingDistance + 0.1f) // Check if the distance is less than the attack range
            {
                Debug.Log("GetInAttackRangeStrategy: In attack range"); // Log the attack range
                return BehaviourTreeNode.NodeStatus.Success; // Return success
            }

            Vector3 desiredPosition = targetEnemy.position - direction.normalized * attackRange; // Get the desired position to move to

            if(!navMeshAgent.pathPending && Vector3.Distance(companionEntity.position, desiredPosition) <= navMeshAgent.stoppingDistance + 0.1f) // Check if the distance is less than 0.1f
            {
                Debug.Log("GetInAttackRangeStrategy: Too close, stopping"); // Log the attack range
                return BehaviourTreeNode.NodeStatus.Success; // Return success
            }

            navMeshAgent.SetDestination(desiredPosition); // Set the destination of the NavMeshAgent to the enemy position
            companionEntity.LookAt(targetEnemy.position.With(y:companionEntity.position.y)); // Makes the companion look at the enemy position and only changes the y position
            Debug.Log("GetInAttackRangeStrategy: Moving to attack enemy"); // Log the movement

            return BehaviourTreeNode.NodeStatus.Running; // Return running
        }

        public void Reset()
        {
            navMeshAgent.ResetPath(); // Reset the path of the NavMeshAgent
        }
    }

    //A strategy to follow the player when in exploration mode.
    public class FollowExplorePointStrategy : InterfaceStrategy
    {
        readonly NavMeshAgent navMeshAgent; // The NavMeshAgent component for the companion
        readonly Transform exploreStandPoint; // The transform of the explore stand point
        readonly Transform companionEntity; // The transform of the companion entity
        readonly Transform lookPoint; // The transform of the player object
        readonly ExploreBehaviourTree exploreBehaviourTree; // The behaviour tree of the companion
        public float followSpeed; // Speed of the NavMeshAgent
        bool isPathFollowed;

        public FollowExplorePointStrategy(NavMeshAgent navMeshAgent, Transform exploreStandPoint, Transform lookPoint, Transform companionEntity, ExploreBehaviourTree exploreBehaviourTree, float followSpeed = 5.5f)
        {
            this.companionEntity = companionEntity; // Get the companion entity
            this.navMeshAgent = navMeshAgent; // Get the NavMeshAgent component
            this.exploreStandPoint = exploreStandPoint; // Get the explore stand point
            this.lookPoint = lookPoint; // Get the look point
            this.followSpeed = followSpeed; // Set the speed of the NavMeshAgent
            this.exploreBehaviourTree = exploreBehaviourTree; // Get the behaviour tree of the companion
        }

        // The companion will follow the player when in exploration mode
        public BehaviourTreeNode.NodeStatus Process()
        {
            var targetPosition = exploreStandPoint.position;
            var lookTargetPosition = lookPoint.position;
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = followSpeed; // Set the speed of the NavMeshAgent
            navMeshAgent.SetDestination(targetPosition);
            companionEntity.LookAt(lookTargetPosition); // Make the companion look at the target position

            if(!exploreBehaviourTree.isActive)
            {
                return BehaviourTreeNode.NodeStatus.Failure; // The exploration state is not active
            }

            // Check if the companion is close to the explore point
            if (isPathFollowed && navMeshAgent.remainingDistance < 0.1f)
            {
                isPathFollowed = false;
            }

            // Check if the NavMeshAgent is still calculating the path
            if (navMeshAgent.pathPending)
            {
                isPathFollowed = true;
            }

            Debug.Log("FollowExplorePointStrategy: Following the player");
            UIManager.Instance.UpdateBehaviourExploreActionText("Following Player"); // Update the UI text for the explore action
            UIManager.Instance.ShowRandomExploreSubtitle(); // Update the UI text for the explore action
            return BehaviourTreeNode.NodeStatus.Running; // The companion is moving to the explore point

        }

        public void Reset()
        {
            isPathFollowed = false;
            navMeshAgent.ResetPath(); // Reset the path of the NavMeshAgent
        }
    }

    //An Idle Strategy for the companion to do nothing and wander around the area during battle.
    public class IdleStrategy : InterfaceStrategy
    {
        readonly NavMeshAgent navMeshAgent;// // The NavMeshAgent component for the companion
        readonly Transform companionEntity;// // The transform of the companion entity
        public float idleSpeed = 2.0f; // The speed of the NavMeshAgent
        public IdleStrategy(NavMeshAgent navMeshAgent, Transform companionEntity)
        {
            this.navMeshAgent = navMeshAgent; // Get the NavMeshAgent component
            this.companionEntity = companionEntity; // Get the companion entity
        }
        /// The companion will idle in the area and not move
        public BehaviourTreeNode.NodeStatus Process()
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = idleSpeed; // Set the speed of the NavMeshAgent
            Debug.Log("IdleStrategy: Companion is idling");
            UIManager.Instance.UpdateBehaviourBattleActionText("Battle Idle"); // Update the UI text for the attack action
            return BehaviourTreeNode.NodeStatus.Running; // The companion is idling
        }
    }

    //A Debug Strategy to help understand what is happening in the game/tree.
    public class DebugAttackStrategy : InterfaceStrategy
    {
        public DebugAttackStrategy()
        {
           
        }

        public BehaviourTreeNode.NodeStatus Process()
        {
            Debug.Log("DebugAttackStrategy: Attacking enemy with debug");
            return BehaviourTreeNode.NodeStatus.Success; // Return success
        }
    }
}
