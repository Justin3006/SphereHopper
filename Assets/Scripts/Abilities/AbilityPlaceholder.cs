using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Just a placeholder for testing purposes.
/// </summary>
public class AbilityPlaceholder : MonoBehaviour, IAbility 
{
    const int usesMax = 0;
    static int usesRemaining;
    const float abilityUseTimeMax = 0;
    float abilityUseTimeRemaining;


    public string GetName()
    {
        return "-";
    }

    public int GetUsesMax()
    {
        return usesMax;
    }

    public int GetUsesRemaining()
    {
        return usesRemaining;
    }

    public bool IsUsed()
    {
        if (abilityUseTimeRemaining > 0)
            return true;
        else 
            return false;
    }

    public bool Use()
    {
        if (usesRemaining <= 0 || abilityUseTimeRemaining > 0)
            return false;
        usesRemaining--;
        abilityUseTimeRemaining = abilityUseTimeMax;
        Debug.Log("Ability used " + Time.time);
        return true;
    }

    public void AddUses(int uses)
    {
        usesRemaining += uses;
        if (usesRemaining > usesMax)
            usesRemaining = usesMax;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (abilityUseTimeRemaining > 0)
            abilityUseTimeRemaining -= Time.fixedDeltaTime;
    }
}
