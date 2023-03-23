using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoProjectile : MonoBehaviour
{
    [SerializeField]
    int direction = 1;
    [SerializeField]
    float speed = 10;
    [SerializeField]
    float rotation = 360;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(0,0, direction * speed * Time.fixedDeltaTime);
        transform.Rotate(0, direction * rotation * Time.fixedDeltaTime, 0);
    }

    public void OnTriggerEnter(Collider other)
    {
        Vulnerable vul = other.gameObject.GetComponent<Vulnerable>();
        if (vul != null && !other.gameObject.GetComponent<Vulnerable>().Hit(transform.position))
            direction *= -1;
    }
}
