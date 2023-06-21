using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    bool adjusted = false;

    private void FixedUpdate()
    {
        if (!adjusted)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, -transform.up, out hit);

            if (hit.collider != null) 
            {
                transform.Translate(0, hit.point.y - transform.position.y, 0);
            }

            adjusted = true;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        PlayerMotor p = other.gameObject.GetComponent<PlayerMotor>();
        if (p != null)
        {
            p.SetExecuteImmunity(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        PlayerMotor p = other.gameObject.GetComponent<PlayerMotor>();
        if (p != null)
        {
            p.SetExecuteImmunity(false);
        }
    }
}
