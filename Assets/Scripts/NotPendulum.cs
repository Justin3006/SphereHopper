using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotPendulum : MonoBehaviour
{
    //TODO: It currently rotates together with the object it's attached to, but it shouldn't
    //TODO: Also, please find a better name!
    [SerializeField]
    int direction = 1;
    [SerializeField]
    float rotationSpeed = 360;


    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0, direction * rotationSpeed * Time.fixedDeltaTime, 0);
    }

    public void OnTriggerEnter(Collider other)
    {
        Vulnerable vul = other.gameObject.GetComponent<Vulnerable>();
        if (vul != null && !other.gameObject.GetComponent<Vulnerable>().Hit(transform.position, 1, 0.1f))
            direction *= -1;
    }
}
