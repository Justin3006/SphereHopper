using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [SerializeField]
    Vector3 dir;
    [SerializeField]
    float impact = 100;
    [SerializeField]
    float stun = 10;

    public void OnTriggerEnter(Collider collision)
    {
        Vulnerable v = collision.gameObject.GetComponent<Vulnerable>();
        if (v != null) 
        {
            v.Hit(v.transform.position - dir, 0, 0, impact, stun);
            //TODO: finally add knockback to player...
        }
    }
}
