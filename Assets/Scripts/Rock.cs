using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
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
