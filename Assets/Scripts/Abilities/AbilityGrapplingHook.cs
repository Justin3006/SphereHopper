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
            gameObject.GetComponent<PlayerMotor>().LockMovement(abilityUseTimeRemaining);
            rb.useGravity = false;
            gameObject.GetComponent<Collider>().isTrigger = true;
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
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (recoveryTimeRemaining > 0) 
        {
            recoveryTimeRemaining -= Time.fixedDeltaTime;
            if (recoveryTimeRemaining <= 0) 
            {
                Collider[] colls = Physics.OverlapCapsule(transform.position, transform.position + 2 * transform.up, 0.5f);
                foreach (Collider c in colls)
                {
                    //TODO: This is an incredibly ugly solution, there has to be a better way. If this part is taken out, you can glitch through the ground when you use the hook at the wrong angle.
                    transform.Translate(0, c.ClosestPointOnBounds(transform.position + 999 * Vector3.up).y - transform.position.y, 0);
                }
            }
        }

        if (abilityUseTimeRemaining > 0) 
        {
            if (targetCharacter != null)
            {
                targetLocation += targetCharacter.transform.position - targetCharacterOriginalPosition;
                targetCharacterOriginalPosition = targetCharacter.transform.position;
                abilityUseTimeRemaining = (targetLocation - Camera.main.transform.position).magnitude / speed;
                gameObject.GetComponent<PlayerMotor>().LockMovement(abilityUseTimeRemaining);
            }

            abilityUseTimeRemaining -= Time.fixedDeltaTime;
        
            Vector3 diff = targetLocation - Camera.main.transform.position;
            Vector3 newPos = transform.position + speed * Time.fixedDeltaTime * (diff).normalized;
            rb.MovePosition(newPos);
            
            if (diff.magnitude <= 1.5f)
            {
                rb.useGravity = true;
                gameObject.GetComponent<Collider>().isTrigger = false;

                abilityUseTimeRemaining = 0;
                if (targetCharacter != null) 
                {
                    targetCharacter.Hit(transform.position, 0, 0, impact, stun);
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
            v.Hit(transform.position, 0, 0, impact, stun);
    }
}
