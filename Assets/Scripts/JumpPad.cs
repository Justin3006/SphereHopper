using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField]
    float modifier = 2;

    
    public void OnTriggerEnter(Collider other)
    {
        PlayerMotor p = other.gameObject.GetComponent<PlayerMotor>();
        if (p != null) 
        {
            p.SetJumpModifier(modifier);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        PlayerMotor p = other.gameObject.GetComponent<PlayerMotor>();
        if (p != null)
        {
            p.SetJumpModifier(1);
        }
    }
}
