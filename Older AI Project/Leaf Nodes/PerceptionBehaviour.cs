using UnityEngine;
using System.Collections.Generic;

public class PerceptionBehaviour : MonoBehaviour
{
    [Header("Vision Settings")]
    public float viewDistance = 10f;
    public float viewAngle = 45f; //in degrees
    public LayerMask obstacleMask;

    [Header("Hearing Settings")]
    public float hearingArea = 5f;

    [Header("Detection Rates")]
    public float detectionRate = 1f; // How fast the detection goes up.
    public float detectionDecayRate = 0.5f; // How fast the detection goes down.

    private float aggressionLevel = 0f; // The aggression level of the hunter.
    private bool playerSpotted = false; // If the player is spotted then the hunter will chase the player.
    private bool noiseHeard = false; // If a noise is heard then the hunter will investigate the noise.

    private Vector3 lastHeardNoiseLocation; // The last location of a thats noise heard.

    public void UpdatePerception(Transform player)
    {
        playerSpotted = false;
        noiseHeard = false;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float angleBetween = Vector3.Angle(transform.forward, directionToPlayer);

        Color rayColour = Color.red; //Default colour if the player has not been spotted.

        // Vision cone checking.
        if (distanceToPlayer < viewDistance && angleBetween < viewAngle)
        {
            if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleMask))
            {
                playerSpotted = true;
                rayColour = Color.green; // Sets the ray colour to green if seen.
                Debug.Log("PerceptionBehavior: Player is spotted in vision of the hunter.");
            }

        }

        if (distanceToPlayer < 3f)
        {
            if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleMask))
            {
                playerSpotted = true;
                rayColour = Color.green; // Sets the ray colour to green if close to the enemy.
                Debug.Log("PerceptionBehavior: Player is spotted in close proximity of the hunter.");
            }
        }

        Debug.DrawRay(transform.position, directionToPlayer * distanceToPlayer, rayColour);

        //Checks the hearing area (which is simulating the noise heard).
        if (distanceToPlayer < hearingArea)
        {
            if (!noiseHeard)
            {
                Debug.Log("PerceptionBehavior: Noise is heard by the hunter. At: " + lastHeardNoiseLocation);
            }
            noiseHeard = true;
            lastHeardNoiseLocation = player.position; // In this case using the player's last position as the noise location.
        }

        // Updating Aggression Level
        if (playerSpotted || noiseHeard)
        {
            aggressionLevel += detectionRate * Time.deltaTime;
            aggressionLevel = Mathf.Clamp01(aggressionLevel);
        }
        else
        {
            aggressionLevel -= detectionDecayRate * Time.deltaTime;
            aggressionLevel = Mathf.Clamp01(aggressionLevel);
        }
    }

    public bool IsPlayerSpotted()
    {
        return playerSpotted;
    }

    public bool IsNoiseHeard()
    {
        return noiseHeard;
    }
    public float GetAggressionValue()
    {
        return aggressionLevel;
    }
    public Vector3 GetLastHeardNoiseLocation()
    {
        return lastHeardNoiseLocation;
    }
}
