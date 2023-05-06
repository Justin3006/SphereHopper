using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Boosts movementspeed and knockback for a few seconds.
/// </summary>
public class AbilityGear : MonoBehaviour, IAbility
{
    const int usesMax = 2;
    static int usesRemaining;
    const float abilityUseTimeMax = 0.5f;
    float abilityUseTimeRemaining;
    const float buffTimeMax = 10;
    float buffTimeRemaining;


    public string GetName() 
    {
        return "Gear";
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

        return true;
    }

    public void ResetUses()
    {
        usesRemaining = usesMax;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (abilityUseTimeRemaining > 0)
        {
            abilityUseTimeRemaining -= Time.fixedDeltaTime;
            if (abilityUseTimeRemaining <= 0) 
            {
                buffTimeRemaining = buffTimeMax;

                gameObject.GetComponent<PlayerMotor>().SetAttackStunModifier(1.5f);
                gameObject.GetComponent<PlayerMotor>().SetAttackImpactModifier(1.5f);
                gameObject.GetComponent<PlayerMotor>().SetGeneralSpeedModifier(2);
            }
        }

        if (buffTimeRemaining > 0) 
        {
            buffTimeRemaining -= Time.fixedDeltaTime;
            if (buffTimeRemaining < 0) 
            {
                gameObject.GetComponent<PlayerMotor>().SetAttackStunModifier(1);
                gameObject.GetComponent<PlayerMotor>().SetAttackImpactModifier(1);
                gameObject.GetComponent<PlayerMotor>().SetGeneralSpeedModifier(1);
            }
        }
    }
}
