using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    int damage = 0;
    [SerializeField]
    float execPerc = 15;
    [SerializeField]
    float speed = 25;
    [SerializeField]
    float impact = 5;
    [SerializeField]
    float stun = 0.015f;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(0, 0, speed * Time.fixedDeltaTime);
    }

    public void OnTriggerStay(Collider other)
    {
        Vulnerable target = other.gameObject.GetComponent<Vulnerable>();
        if (target != null) 
        {
            //TODO: change transform.position to something, that properly applies knockback
            target.Hit(transform.position, damage, execPerc, impact, stun);
        }
        Destroy(gameObject);
    }
}
