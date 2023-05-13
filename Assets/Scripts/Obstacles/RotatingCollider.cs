using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A colider that rotates around a fixed point and hits Vulnerables
/// </summary>
public class RotatingCollider : MonoBehaviour
{
    Vector3 lastEuler;
    float lastY;
    [SerializeField]
    int direction = 1;
    [SerializeField]
    float rotationSpeed = 360;
    [SerializeField]
    int damage = 1;
    [SerializeField]
    float exec = 0;
    [SerializeField]
    float knockback = 1;
    [SerializeField]
    float stun = 0.1f;


    private void Start()
    {
        lastEuler = transform.eulerAngles;
        int dirAbs = Mathf.Abs(direction);
        if (dirAbs != 1)
            direction /= dirAbs;
    }

    private void FixedUpdate()
    {
        transform.eulerAngles = lastEuler;
        transform.RotateAround(transform.position, transform.up, direction * rotationSpeed * Time.fixedDeltaTime);
        lastEuler = transform.eulerAngles;
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, lastY + direction * rotationSpeed * Time.fixedDeltaTime, transform.eulerAngles.z);
        //lastY = transform.eulerAngles.y;
    }

    public void OnTriggerEnter(Collider other)
    {
        Vulnerable vul = other.gameObject.GetComponent<Vulnerable>();
        if (vul != null && !other.gameObject.GetComponent<Vulnerable>().Hit(transform.position, damage, exec, knockback, stun))
            direction *= -1;
    }
}
