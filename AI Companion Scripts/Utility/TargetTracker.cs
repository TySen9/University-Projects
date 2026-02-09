using UnityEngine;

namespace PartyAISystem
{
    /// This class is used to track the target of the AI companion. It holds a reference to the target's transform and stats.
    public class TargetTracker
    {
        public Transform TargetTransform { get; set; }
        public Statistics TargetStats { get; set; }

        public bool HasTarget => TargetTransform != null && TargetStats != null;
    }
}