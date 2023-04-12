using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPlaceholder : MonoBehaviour, IAbility 
{
    int usesMax = 3;
    int usesRemaining;
    float abilityUseTimeMax = 1;
    float abilityUseTimeRemaining;


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

    // Start is called before the first frame update
    void Start()
    {
        usesRemaining = usesMax;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (abilityUseTimeRemaining > 0)
            abilityUseTimeRemaining -= Time.fixedDeltaTime;
    }
}
