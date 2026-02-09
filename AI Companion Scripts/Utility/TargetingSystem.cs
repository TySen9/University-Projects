using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PartyAISystem
{
    // A class to track the target and its statistics to make sure the right stats are used in the right context when targeting
    public class TargetingSystem : MonoBehaviour
    {
        public float searchRadius = 20f; // The radius to search for targets
        public LayerMask enemyLayer; // The layer mask to use for the enemy layer

        public TargetTracker Tracker { get; private set; } = new TargetTracker(); // The target tracker to track the target and its statistics. Gets and sets the target and its statistics

        private Transform ownerTransform; // The transform of the owner of the targeting system
        private NavMeshAgent navMeshAgent; // The navmesh agent for pathfinding

        private void Awake()
        {
            ownerTransform = transform; // 
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        // Retarget the target if the target is not in range to make sure there is always a target
        public void Retarget()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float closestDistance = Mathf.Infinity; // The closest distance to the target
            Transform closestEnemy = null;

            foreach (GameObject enemy in enemies) // Loops through all the enemies in the scene
            {
                Vector3 dir = enemy.transform.position - ownerTransform.position; // The direction from the owner to the enemy
                float distance = dir.magnitude; // The distance from the owner to the enemy
                // Check if the enemy is within the search radius and if the enemy is on the navmesh
                if (distance < closestDistance && NavMesh.SamplePosition(enemy.transform.position, out NavMeshHit hit, searchRadius, navMeshAgent.areaMask))
                {
                    closestDistance = distance; // Update the closest distance
                    closestEnemy = enemy.transform; // Update and sets the closest enemy
                }
            }

            if (closestEnemy != null)
            {
                Tracker.TargetTransform = closestEnemy; // Sets the target transform to the closest enemy
                Tracker.TargetStats = closestEnemy.GetComponent<Statistics>(); // Sets the target stats to the closest enemy's statistics
                Debug.Log($"Targeting System: Targeting {Tracker.TargetTransform.name}");
            }
            else
            {
                Tracker.TargetTransform = null;
                Tracker.TargetStats = null;
                Debug.Log("Targeting System: No targets found in range.");
            }
        }
    }
}