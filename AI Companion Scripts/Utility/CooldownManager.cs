using System.Collections.Generic;
using UnityEngine;

// The CooldownManager class manages cooldowns for abilities and groups of abilities.
// Through this class, you can trigger cooldowns, check if an ability is on cooldown, and get the time remaining for a cooldown.
// It is also a store for the cooldown timers, which are stored in a dictionary with the ability key as the key and the cooldown end time as the value.
// It allows to check if the ability is on cooldown and to reset the cooldown.
// Also checks if an ability can be used based on its cooldown status.
public static class CooldownManager
{
    private static Dictionary<string, float> cooldownTimers = new(); // Dictionary to store cooldown timers

    /// Trigger a cooldown for a specific ability key with a specified duration
    public static void TriggerCooldown(string key, float duration)
    {
        cooldownTimers[key] = Time.time + duration;
    }

    // / Check if a specific ability key is on cooldown
    public static bool IsOnCooldown(string key)
    {
        return cooldownTimers.ContainsKey(key) && Time.time < cooldownTimers[key];
    }

    /// Check if a specific ability key is on cooldown and return the time remaining
    public static float TimeRemaining(string key)
    {
        return cooldownTimers.ContainsKey(key) ? Mathf.Max(0f, cooldownTimers[key] - Time.time) : 0f;
    }

    // Reset the cooldown for a specific ability key
    public static void ResetCooldown(string key)
    {
        cooldownTimers.Remove(key);
    }

    // / Trigger a cooldown for a specific ability key and a group key with specified durations
    public static void TriggerCooldownWithGroup (string abilityKey, float abilityDuration, string groupKey, float groupDuration)
    {
        TriggerCooldown(abilityKey, abilityDuration);
        if (!string.IsNullOrEmpty(groupKey))
            TriggerCooldown(groupKey, groupDuration);
    }

    // / Check if an ability can be used based on its cooldown status
    public static bool CanUse(string abilityKey, string groupKey = null)
    {
        if (IsOnCooldown(abilityKey)) return false;
        if (!string.IsNullOrEmpty(groupKey) && IsOnCooldown(groupKey)) return false;
        return true;
    }
}