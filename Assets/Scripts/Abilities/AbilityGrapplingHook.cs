using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Draws you towards a target position and knocks back enemies on the way.
/// </summary>
public class AbilityGrapplingHook : MonoBehaviour, IAbility
{
    const int usesMax = 10;
    int usesRemaining;
    float abilityUseTimeRemaining;
    // recovery time prevents you from glitching through walls too easily
    float recoveryTimeMax = 0.1f;
    float recoveryTimeRemaining;

    Vector3 targetLocation;
    Rigidbody rb;
    const float range = 25;
    const float speed = 25;

    Vulnerable targetCharacter;
    Vector3 targetCharacterOriginalPosition;

    const float impact = 10f;
    const float stun = 0.025f;

    public string GetName()
    {
        return "Grapplinghook";
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
        if (abilityUseTimeRemaining > 0 || recoveryTimeRemaining > 0)
            return true;
        else
            return false;
    }

    public bool Use()
    {
        if (usesRemaining <= 0 || abilityUseTimeRemaining > 0)
            return false;
        usesRemaining--;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position + 0.4f * Camera.main.transform.forward, Camera.main.transform.forward, out hit, range))
        {
            targetLocation = hit.point;
            abilityUseTimeRemaining = (targetLocation - Camera.main.transform.position).magnitude / speed;
            PlayerManager.GetMotor().LockMovement(abilityUseTimeRemaining);
            rb.useGravity = false;
            PlayerManager.GetCollider().isTrigger = true;
            if (hit.collider.gameObject.GetComponent<Vulnerable>() != null) 
            {
                targetCharacter = hit.collider.gameObject.GetComponent<Vulnerable>();
                targetCharacterOriginalPosition = targetCharacter.transform.position;
            }
        }

        return true;
    }

    void Start()
    {
        usesRemaining = usesMax;
        rb = PlayerManager.GetRigidbody();
    }

    void FixedUpdate()
    {
        if (recoveryTimeRemaining > 0) 
        {
            recoveryTimeRemaining -= Time.fixedDeltaTime;
        }

        if (abilityUseTimeRemaining > 0) 
        {
            if (targetCharacter != null)
            {
                targetLocation += targetCharacter.transform.position - targetCharacterOriginalPosition;
                targetCharacterOriginalPosition = targetCharacter.transform.position;
                abilityUseTimeRemaining = (targetLocation - Camera.main.transform.position).magnitude / speed;
                PlayerManager.GetMotor().LockMovement(abilityUseTimeRemaining);
            }

            abilityUseTimeRemaining -= Time.fixedDeltaTime;
        
            Vector3 diff = targetLocation - Camera.main.transform.position;
            Vector3 newPos = PlayerManager.GetTransform().position + speed * Time.fixedDeltaTime * (diff).normalized;
            rb.MovePosition(newPos);
            
            if (diff.magnitude <= 1.5f)
            {
                rb.useGravity = true;
                PlayerManager.GetCollider().isTrigger = false;
                abilityUseTimeRemaining = 0;
                if (targetCharacter != null) 
                {
                    targetCharacter.Knockback(transform.position, impact, stun);
                    targetCharacter = null;
                }
                recoveryTimeRemaining = recoveryTimeMax;
            }
         }
    }


    private void OnTriggerEnter(Collider other)
    {
        Vulnerable v = other.GetComponent<Vulnerable>();
        if(v != null)
            v.Knockback(PlayerManager.GetTransform().position, impact, stun);
    }
}
