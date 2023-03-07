using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0,0, 10f * Time.deltaTime);
        transform.Rotate(0, 360 * Time.deltaTime, 0);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.GetComponent<Vulnerable>().Hit())
            Destroy(gameObject);
    }
}
