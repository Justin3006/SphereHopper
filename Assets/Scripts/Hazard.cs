using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField]
    float timeTillHitMax = 1;
    [SerializeField]
    bool startOnCDActive = false;
    List<Vulnerable> enteredCharacters = new List<Vulnerable>();
    List<float> timeTillHitRemaining = new List<float>();

    [SerializeField]
    int damage;
    [SerializeField]
    float execution;


    private void FixedUpdate()
    {
        for (int i = 0; i < enteredCharacters.Count; i++) 
        {
            timeTillHitRemaining[i] -= Time.fixedDeltaTime;
            if (timeTillHitRemaining[i] <= 0) 
            {
                enteredCharacters[i].Hit(Vector3.zero, damage, execution, 0, 0);
                timeTillHitRemaining[i] = timeTillHitMax;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Vulnerable v = other.GetComponent<Vulnerable>();
        if (v != null) 
        {
            enteredCharacters.Add(v);
            if (startOnCDActive)
                timeTillHitRemaining.Add(timeTillHitMax);
            else
                timeTillHitRemaining.Add(0);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Vulnerable v = other.GetComponent<Vulnerable>();
        if (v != null)
        {
            int i = enteredCharacters.IndexOf(v);
            enteredCharacters.RemoveAt(i);
            timeTillHitRemaining.RemoveAt(i);
        }
    }
}
