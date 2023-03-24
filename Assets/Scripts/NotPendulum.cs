using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotPendulum : MonoBehaviour
{
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
        if (vul != null && !other.gameObject.GetComponent<Vulnerable>().Hit(transform.position))
            direction *= -1;
    }
}
