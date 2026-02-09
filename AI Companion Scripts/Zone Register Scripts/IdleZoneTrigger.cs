using UnityEngine;


namespace PartyAISystem
{
    /// This class is used to trigger the idle zone for the companion.
    [RequireComponent(typeof(Collider))]
    public class IdleZoneTrigger : MonoBehaviour
    {
        // The companion controller that will be triggered when the player enters the idle zone
        [SerializeField] CompanionController companionController;
        // This method is called when the player enters the trigger collider
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                companionController.isInIdleZone = true;
            }
        }
        // This method is called when the player exits the trigger collider
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                companionController.isInIdleZone = false;
            }
        }
    }
}
