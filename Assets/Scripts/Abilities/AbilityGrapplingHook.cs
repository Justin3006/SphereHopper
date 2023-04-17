using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityGrapplingHook : MonoBehaviour, IAbility
{
    int usesMax = 10;
    int usesRemaining;
    float abilityUseTimeRemaining;

    Vector3 targetLocation;
    Rigidbody rb;
    float range = 25;
    float speed = 25;

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
            Debug.Log(abilityUseTimeRemaining);
            PlayerManager.GetMotor().LockMovement(abilityUseTimeRemaining);
            rb.useGravity = false;
        }
        //PlayerManager.GetMotor().MoveToTarget(hit.point, speed);

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
        if (abilityUseTimeRemaining > 0) {
            abilityUseTimeRemaining -= Time.fixedDeltaTime;
        
            Vector3 diff = targetLocation - Camera.main.transform.position;
            Vector3 newPos = PlayerManager.GetTransform().position + speed * Time.fixedDeltaTime * (diff).normalized;
            rb.MovePosition(newPos);
            
            if (diff.magnitude <= 1.5f)
            {
                rb.useGravity = true;
                abilityUseTimeRemaining = 0;
            }
         }
    }
}
