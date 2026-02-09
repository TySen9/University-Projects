using UnityEngine;
using UnityEngine.AI;  

namespace AdvancedAI.BehaviourTree
{
    public class InvestigatingArea : BehaviourTreeNodes
    {
        private HunterAI hunterAI;
        private NavMeshAgent navAgent;
        private Vector3 investigationPoint;
        private float InvestigateSpeed = 4f;

        public InvestigatingArea(HunterAI hunterAI)
        {
            this.hunterAI = hunterAI;
            this.navAgent = hunterAI.GetComponent<NavMeshAgent>();
        }
        public override NodeState Evaluate()
        {
            //Check if there is a noise location to investigate
            Vector3 noiseLocation = hunterAI.Perception.GetLastHeardNoiseLocation();

            if(noiseLocation != Vector3.zero)
            {
                hunterAI.StartInvestigating(noiseLocation);
                Debug.Log("InvestigatingArea: Noise was heard, moving to investigate.");
                //Set chase speed
                navAgent.speed =  InvestigateSpeed;
                return NodeState.Running;
            }

            // Receiving a noise stimulus or investigating a point.
            if (hunterAI.IsInvestigating())
            {
                Debug.Log("InvestigatingArea: Investigating the Stimulus/Area");

                //While the AI hunter is still investigating, keep them going.
                if (!navAgent.pathPending && navAgent.remainingDistance < 0.5f)
                {
                    Debug.Log("InvestigatingArea: Investigated the Area.");
                    hunterAI.FinishInvestigating();
                    return NodeState.Success;
                }

                return NodeState.Running;
            }
            return NodeState.Failure;
        }
    }
}
