using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyLabyrinthStalker : Vulnerable
{
    Rigidbody rb;
    [SerializeField]
    float speed = 2.5f;
    [SerializeField]
    float maxRangeToTravel = 5f;
    float rangeToTravel;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rangeToTravel > Time.fixedDeltaTime * speed)
        {
            rb.MovePosition(transform.position + Time.fixedDeltaTime * speed * transform.forward);
            rangeToTravel -= Time.fixedDeltaTime * speed;
        }
        else if (rangeToTravel > 0)
        {
            rb.MovePosition(transform.position + rangeToTravel * transform.forward);
            rangeToTravel = 0;
        }
        else
        {
            bool[] dir = new bool[4];
            //TODO: Exclude players or you can get stuck in an infinite loop
            dir[0] = !Physics.Raycast(transform.position + transform.up, transform.forward, maxRangeToTravel);
            dir[1] = !Physics.Raycast(transform.position + transform.up, transform.right, maxRangeToTravel);
            dir[2] = !Physics.Raycast(transform.position + transform.up, -transform.forward, maxRangeToTravel);
            dir[3] = !Physics.Raycast(transform.position + transform.up, -transform.right, maxRangeToTravel);

            bool chosen = false;
            while (!chosen)
            {
                int rn = Random.Range(0, 4);
                switch (rn)
                {
                    case 0: if (dir[0]) { chosen = true; } break;
                    case 1: if (dir[1]) { chosen = true; transform.Rotate(0, 90, 0); } break;
                    case 2: if (dir[2]) { chosen = true; transform.Rotate(0, 180, 0); } break;
                    case 3: if (dir[3]) { chosen = true; transform.Rotate(0, -90, 0); } break;
                }
            }

            rangeToTravel = maxRangeToTravel;
        }
        //TODO: Recognize and attack the player. Test, if within vision by using PlayerManager.GetPosition() and Vector3.Angle(a,b), then see, if Physics.RayCast finds an Object in between. Ataack by Execution.
    }
}
