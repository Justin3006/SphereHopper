using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shoots seven bullets, if all connect to a target, it dies.
/// </summary>
public class AbilityDischarge : MonoBehaviour, IAbility
{
    const int usesMax = 3;
    static int usesRemaining;
    const float abilityUseTimeMax = 7 * 0.15f;
    float abilityUseTimeRemaining;
    
    GameObject projectile;
    int projectilesMax = 7;
    int projectilesRemaining;
    float projectileCDMax = 0.15f;
    float projectileCD;


    public string GetName()
    {
        return "Discharge";
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

        projectilesRemaining = projectilesMax;
        projectileCD = projectileCDMax;

        return true;
    }

    public void AddUses(int uses) 
    {
        usesRemaining += uses;
        if (usesRemaining > usesMax)
            usesRemaining = usesMax;
    }

    // Start is called before the first frame update
    void Start()
    {
        //TODO: Change object path to something like "Prefabs/Abilities/Projectile"
        projectile = (GameObject)Resources.Load("Projectile", typeof(GameObject));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (abilityUseTimeRemaining > 0)
            abilityUseTimeRemaining -= Time.fixedDeltaTime;

        if (projectileCD > 0) {
            projectileCD -= Time.fixedDeltaTime;
            if (projectileCD <= 0 && projectilesRemaining > 0)
            {
                projectilesRemaining--;
                projectileCD = projectileCDMax;
                Vector3 offset = new Vector3(Random.Range(-0.15f, 0.15f), Random.Range(-0.15f, 0.15f), 0);
                Instantiate(projectile, Camera.main.transform.position + Camera.main.transform.forward + Camera.main.transform.rotation * offset, Camera.main.transform.rotation);
            }
        }
    }
}
