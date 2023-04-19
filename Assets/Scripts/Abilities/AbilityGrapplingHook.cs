using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityGrapplingHook : MonoBehaviour, IAbility
{
    const int usesMax = 10;
    int usesRemaining;
    float abilityUseTimeRemaining;

    Vector3 targetLocation;
    Rigidbody rb;
    const float range = 25;
    const float speed = 25;

    GameObject targetCharacter;
    Vector3 targetCharacterOriginalPosition;

    [SerializeField] private float knockbackForce = 20f;    

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

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position + 2 * Camera.main.transform.forward, Camera.main.transform.forward, out hit, range))
        {
            targetLocation = hit.point;
            abilityUseTimeRemaining = (targetLocation - Camera.main.transform.position).magnitude / speed;
            PlayerManager.GetMotor().LockMovement(abilityUseTimeRemaining);
            rb.useGravity = false;
            if (hit.collider.gameObject.GetComponent<Vulnerable>() != null) 
            {
                targetCharacter = hit.collider.gameObject;
                targetCharacterOriginalPosition = targetCharacter.transform.position;

                // Apply knockback force to the target character
                Rigidbody targetRb = targetCharacter.GetComponent<Rigidbody>();
                if (targetRb != null && !targetCharacter.CompareTag("Player"))
                {
                    Vector3 knockbackDirection = (targetCharacter.transform.position - Camera.main.transform.position).normalized;
                    targetRb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
                }
            }
        }

        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        usesRemaining = usesMax;
        rb = PlayerManager.GetRigidbody();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
                abilityUseTimeRemaining = 0;
                targetCharacter = null;
            }
         }
    }

    public void SetKnockbackForce(float force)
    {
        knockbackForce = force;
    }
}
