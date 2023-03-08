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
    void Update()
    {
        transform.Translate(0,0, direction * speed * Time.deltaTime);
        transform.Rotate(0, direction * rotation * Time.deltaTime, 0);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.GetComponent<Vulnerable>().Hit())
            direction *= -1;
    }
}
