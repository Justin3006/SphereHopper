using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float execPerc = 15;
    float speed = 25;
    float impact = 5;
    float stun = 0.015f;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(0, 0, speed * Time.fixedDeltaTime);
    }

    public void OnTriggerEnter(Collider other)
    {
        Vulnerable target = other.gameObject.GetComponent<Vulnerable>();
        if (target != null) 
        {
            //TODO: change transform.position to something, that properly applies knockback
            target.Hit(transform.position, 0, execPerc, impact, stun);
        }
        Destroy(gameObject);
    }
}
