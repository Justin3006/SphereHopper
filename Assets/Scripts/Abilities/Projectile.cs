using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float dmg = 15;
    float speed = 25;

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
            target.Execute(dmg);
        }
        Destroy(gameObject);
    }
}
