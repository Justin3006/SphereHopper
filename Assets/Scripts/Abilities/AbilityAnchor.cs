using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Marks current position at first activation, teleports there on second acitvation
/// </summary>
public class AbilityAnchor : MonoBehaviour, IAbility
{
    const int usesMax = 3;
    int usesRemaining;
    const float abilityUseTimeMax = 0.5f;
    float abilityUseTimeRemaining;
    Vector3 savedPosition;
    bool saved;


    public string GetName()
    {
        return "Anchor";
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

        abilityUseTimeRemaining = abilityUseTimeMax;
        if (!saved)
            savedPosition = PlayerManager.GetTransform().position;
        else
            usesRemaining--;

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
                if (saved)
                {
                    PlayerManager.GetTransform().position = savedPosition;
                }
                saved = !saved;
            }
        }
    }
}
