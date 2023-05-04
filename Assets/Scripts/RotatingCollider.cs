using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCollider : MonoBehaviour
{
    [SerializeField]
    int direction = 1;
    [SerializeField]
    float rotationSpeed = 360;


    // Update is called once per frame
    void FixedUpdate()
    {
        // Define the point and axis around which the object should rotate
        Vector3 rotationPoint = transform.position;
        Vector3 rotationAxis = Vector3.up;
        // Rotate the object around the defined point and axis, based on the direction and rotation speed.
        transform.RotateAround(rotationPoint, rotationAxis, direction * rotationSpeed * Time.fixedDeltaTime);
    }

    public void OnTriggerEnter(Collider other)
    {
        Vulnerable vul = other.gameObject.GetComponent<Vulnerable>();
        if (vul != null && !other.gameObject.GetComponent<Vulnerable>().Hit(transform.position, 1, 0, 1, 0.1f))
            direction *= -1;
    }
}
