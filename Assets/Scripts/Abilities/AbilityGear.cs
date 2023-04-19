using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityGear : MonoBehaviour, IAbility
{
    const int usesMax = 2;
    int usesRemaining;
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

    // Start is called before the first frame update
    void Start()
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
                PlayerManager.GetMotor().SetAttackStunModifier(5);
                PlayerManager.GetMotor().SetAttackImpactModifier(5);
                PlayerManager.GetMotor().SetGeneralSpeedModifier(2);
            }
        }

        if (buffTimeRemaining > 0) 
        {
            buffTimeRemaining -= Time.fixedDeltaTime;
            if (buffTimeRemaining < 0) 
            {
                PlayerManager.GetMotor().SetAttackStunModifier(1);
                PlayerManager.GetMotor().SetAttackImpactModifier(1);
                PlayerManager.GetMotor().SetGeneralSpeedModifier(1);

            }
        }
    }
}
