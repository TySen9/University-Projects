using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using AdvancedAI.BehaviourTree; // Import the namespace of the BehaviourTree to use its classes.

namespace AdvancedAI.BehaviourTree
{
    public class HunterAI : MonoBehaviour
    {

        [Header("Perception Settings")]
        public PerceptionBehaviour Perception;

        [Header("Aggression/Despawn Settings")]
        public float despawnThreshold = 25f; // When the aggression is below this then the hunter will despawn.

        private BehaviourTreeNodes behaviourTree;
        private Transform player;
        private float aggressionValue; //This is updated through the perception system.

        private UnityEngine.AI.NavMeshAgent navAgent;

        [Header("Patrol Points")]
        public List <Transform> patrolTargets = new List<Transform>(); // The patrol points that the hunter will move between.

        //investigation variables
        private bool isInvestigating = false;
        private Vector3 investigationPoint;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

            navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>(); //Gets the NavMeshAgent component from the game object.

            //Get all patrol points in the scene.
            List<Vector3> patrolPositions = new List<Vector3>();

            foreach(Transform point in patrolTargets)
            {
                if (point != null)
                {
                    patrolPositions.Add(point.position);
                }  
            }

            // Finds the player using a tag.
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
                player = playerObject.transform;
            else
                Debug.LogError("Player not found");

            // Gets the perception component from the game object.
            Perception = GetComponent<PerceptionBehaviour>();

            // Initialises the behaviour tree below.
            // The behaviour tree is a sequence of nodes that are evaluated in a specific order and that that order is:
            // First which is the engagement sequence, if the player is seen then the hunter will chase the player 
            // and if In attack change it will attack.
            // Then the investigation sequence which is if a noise is heard then it will investigate the noise.
            // Then it will patrol the area.
            // Finally, if the aggression is below a certain threshold then the hunter will despawn from the area.

            BehaviourTreeNodes engagementSequence = new BehaviourTSequence(new List<BehaviourTreeNodes>
        {
            new CheckPlayerSighted(this),
            new BehaviourSelector(new List<BehaviourTreeNodes>
            {
                new BehaviourTSequence(new List<BehaviourTreeNodes>
                {
                    new CheckPlayerInAttackRange(this),
                    new AttackPlayer(this)
                }),
                new ChasePlayer(this),
            })
        });

            BehaviourTreeNodes investigationSequence = new BehaviourTSequence(new List<BehaviourTreeNodes>
        {
            new CheckInvestigation(this),
            new InvestigatingArea(this)
        });

            BehaviourTreeNodes patrol = new Patrol(this, patrolPositions);

            // A top-level selector that the order bellow determines the priority of the actions.
            behaviourTree = new BehaviourSelector(new List<BehaviourTreeNodes>
        {
            engagementSequence,
            investigationSequence,
            patrol,
            new Despawn(this)
        });
        }

        // Update is called once per frame
        void Update()
        {
            if (player != null)
            {
                Perception.UpdatePerception(player);
                aggressionValue = Perception.GetAggressionValue();
            }

            behaviourTree.Evaluate();

            if (aggressionValue < despawnThreshold)
            {
                Debug.Log("HunterAI: The aggression is low enough, the enemy can despawn now.");
            }

        }

        public bool IsPlayerInAttackRange()
        {
            if (player == null)
                return false;
            float distance = Vector3.Distance(player.position, transform.position);
            //Defines the attack range of the hunter (in meters).
            return distance < 2.5f;
        }

        public void Attack()
        {
            Debug.Log("HunterAI: Attacking the Player.");
        }

        public void Investigate()
        {
            Debug.Log("HunterAI: Investigating the Stimulus");
        }

        public bool ShouldDespawn()
        {
            return aggressionValue < despawnThreshold;
        }

        public void OnDestroy()
        {
            Debug.Log("HunterAI: The hunter has been despawned.");
        }

        public void Despawn()
        {
            Debug.Log("HunterAI: Despawning the Hunter");
        }

        public Transform GetPlayerTransform()
        {
            return player;
        }

        public bool IsInvestigating()
        {
            return isInvestigating;
        }

        public void StartInvestigating(Vector3 point)
        {
            isInvestigating = true;
            investigationPoint = point;
            navAgent.SetDestination(investigationPoint);
            Debug.Log("HunterAI: Started investigating the Area.");
        }

        public void FinishInvestigating()
        {
            isInvestigating = false;
            Debug.Log("HunterAI: Finished investigating the Area.");
        }

        public Vector3 GetRandomInvestigationPoint()
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * 5f;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 5f, UnityEngine.AI.NavMesh.AllAreas))
            {
                return hit.position;
            }
            return transform.position; // A fallback to the current position if there is no valid point that is found.
        }
    }
}
